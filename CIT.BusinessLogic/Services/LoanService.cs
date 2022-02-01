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
        public LoanService(ILoanRepository loanRepository, IMapper mapper, IEntitiesInfoService entitiesInfoService)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _entitiesInfoService = entitiesInfoService;
        }

        public async Task<LoanDto> AddLoanAsync(LoanDto loanDto, int lenderBusinessId)
        {
            var entityInfo = await _entitiesInfoService.AddEntityInfoAsync();

            loanDto.StartDate = DateTime.Now;
            loanDto.EntityInfoId = entityInfo.Id;
            loanDto.LenderBusinessId = lenderBusinessId;
            var loanEntity = _mapper.Map<Loan>(loanDto);

            var savedLoan = await _loanRepository.AddAsync(loanEntity);
            await _loanRepository.SaveChangesAsync();
            loanDto.Id = savedLoan.Id;
            return loanDto;
           
        }
        
        public Task DeleteLoanAsync(int loanId)
        {
            throw new NotImplementedException();
        }

        public async Task<LoanDto> GetloanByIdAsync(int loanId)
        {
            var loan = _mapper.Map<LoanDto>(await _loanRepository.GetAllWithFilterAndWithRelationsAsync(l => l.Id == loanId));

            if (loan != null)
                return loan;

            throw new Exception("Este prestamo no se encuentra o no existe.");
        }

        public async Task<List<LoanDto>> GetLoanssAsync(int lenderBusinessId)
        {
            var loan = _mapper.Map<List<LoanDto>>(await _loanRepository.GetAllWithFilterAndWithRelationsAsync(v => v.LenderBusiness.Id == lenderBusinessId));

            return loan;
        }

        public async Task<LoanDto> UpdateLoanAsync(LoanDto _loan)
        {
            var loanEntity =  await _loanRepository.FirstOrDefaultAsync(v => v.Id == _loan.Id);
            if (loanEntity != null)
            {
                
                loanEntity.DuesQuantity = _loan.DuesQauntity;
                loanEntity.TotalLoan = _loan.TotalLoan;
                loanEntity.EndDate = _loan.EndDate;
                loanEntity.PayDay = _loan.PayDay;
                loanEntity.InterestRate = _loan.InterestRate;
                loanEntity.MensualPay = _loan.MensualPay;
                _loanRepository.Update(loanEntity);
                await _loanRepository.SaveChangesAsync();
                return _loan;
            }
            throw new Exception("Error, este prestamo no se encuentra registrado o activo..");
        }
    }
}
