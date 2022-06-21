using AutoMapper;
using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IPaymentService _paymentService;

        public LoanService(ILoanRepository loanRepository, 
            IMapper mapper, 
            IEntitiesInfoService entitiesInfoService, 
            IPaymentService paymentService)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _entitiesInfoService = entitiesInfoService;
            _paymentService = paymentService;
        }

        public async Task<LoanDto> AddLoanAsync(LoanDto loanDto, int lenderBusinessId)
        {
            var loanExists = await ValidateIfLoanExistsAsync(loanDto.LoanName);
            if (!loanExists)
            {
                var entityInfo = await _entitiesInfoService.AddEntityInfoAsync();
                loanDto.EntityInfoId = entityInfo.Id;
                loanDto.LenderBusinessId = lenderBusinessId;
                loanDto.InterestRate /= 100;

                if (loanDto.EndDate <= loanDto.StartDate)
                    throw new Exception("La fecha final no puede ser menor o igual a la fecha inicial");

                var loanEntity = _mapper.Map<Loan>(loanDto);

                var savedLoan = await _loanRepository.AddAsync(loanEntity);
                await _loanRepository.SaveChangesAsync();
                loanDto.Id = savedLoan.Id;
                return loanDto;
            }

            throw new Exception("Ya existe un préstamo con este nombre, por favor, escribe un nombre diferente");
        }

        public async Task<LoanDto> UpdateLoanAsync(LoanDto loan)
        {
            var existsLoan = await ValidateIfLoanExistsAsync(loan.LoanName, loan.Id);
            if (!existsLoan)
            {
                var loanEntity = await _loanRepository.FirstOrDefaultAsync(v => v.Id == loan.Id);
                if (loanEntity != null)
                {

                    loanEntity.DuesQuantity = loan.DuesQuantity;
                    loanEntity.TotalLoan = loan.TotalLoan;
                    loanEntity.StartDate = loan.StartDate;
                    loanEntity.EndDate = loan.EndDate;
                    loanEntity.PayDay = loan.PayDay;
                    loanEntity.InterestRate = loan.InterestRate;
                    loanEntity.MensualPay = loan.MensualPay;
                    _loanRepository.Update(loanEntity);
                    await _loanRepository.SaveChangesAsync();
                    return loan;
                }
                throw new Exception("Error, este prestamo no se encuentra registrado o activo...");
            }
            throw new Exception("Ya existe un préstamo con este nombre, por favor, escribe un nombre diferente");
        }

        private async Task<bool> ValidateIfLoanExistsAsync(string loanName, int loanId = 0)
        {
            var loanInDb = await _loanRepository.FirstOrDefaultAsync(l => l.LoanName.Equals(loanName));

            if (loanId != 0)
                loanInDb = await _loanRepository.FirstOrDefaultAsync(l => l.LoanName.Equals(loanName) && l.Id != loanId);

            return loanInDb != null;
        }

        public async Task DeleteLoanAsync(int loanId)
        {
            var loan = await GetloanByIdAsync(loanId);

            if(loan != null)
            {
                var loanEntity = _mapper.Map<Loan>(loan);
                await _entitiesInfoService.UpdateEntityInfo(loanEntity.EntityInfoId, 0);
                await _paymentService.DeletePaymentsByLoanAsync(loanId);
            }
        }

        public async Task<LoanDto> GetloanByIdAsync(int loanId)
        {
            var loan = _mapper.Map<LoanDto>(await _loanRepository.FirstOrDefaultWithRelationsAsync(l => l.Id == loanId && l.EntityInfo.Status == 1));

            if (loan != null)
                return loan;

            throw new Exception("Este prestamo no se encuentra o no existe.");
        }

        public async Task<List<LoanDto>> GetLoansByLenderBusinessAsync(int lenderBusinessId)
        {
            var loans = _mapper.Map<List<LoanDto>>(await _loanRepository.GetAllWithFilterAndWithRelationsAsync(l => l.LenderBusinessId == lenderBusinessId && l.EntityInfo.Status == 1));

            return loans;
        }

        public async Task<List<LoanDto>> GetLoansByUserAsync(int lenderBusinessId, int userId)
        {
            var loans = _mapper.Map<List<LoanDto>>(await _loanRepository.GetAllWithFilterAndWithRelationsAsync(l => l.LenderBusinessId == lenderBusinessId && l.UserId == userId && l.EntityInfo.Status == 1));

            return loans;
        }

        public async Task<List<LoanDto>> GetLoansByNameAsync(int lenderBusinessId, string loanName, int userId = 0)
        {
            var loans = new List<LoanDto>();

            if (userId == 0)
                loans = _mapper.Map<List<LoanDto>>(await _loanRepository.GetAllWithFilterAndWithRelationsAsync(l => l.LenderBusinessId == lenderBusinessId && l.LoanName.ToLower().Contains(loanName.ToLower()) && l.EntityInfo.Status == 1));
            else
                loans = _mapper.Map<List<LoanDto>>(await _loanRepository.GetAllWithFilterAndWithRelationsAsync(l => l.LenderBusinessId == lenderBusinessId && l.UserId == userId && l.LoanName.ToLower().Contains(loanName.ToLower()) && l.EntityInfo.Status == 1));

            return loans;
        }
    }
}
