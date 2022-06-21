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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, 
            IEntitiesInfoService entitiesInfoService,
            ILoanRepository loanRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _loanRepository = loanRepository;
            _entitiesInfoService = entitiesInfoService;
            _mapper = mapper;
        }
        public async Task<PaymentDto> AddPaymentAsync(PaymentDto payment, int lenderBusinessId)
        {
            var paymentEntity = _mapper.Map<Payment>(payment);
            var loan = await _loanRepository.FirstOrDefaultAsync(l => l.Id == payment.LoanId);

            if(loan != null)
            {
                var entityInfo = await _entitiesInfoService.AddEntityInfoAsync();
                paymentEntity.EntityInfoId = entityInfo.Id;
                paymentEntity.LenderBusinessId = lenderBusinessId;

                await _paymentRepository.AddAsync(paymentEntity);
                await _paymentRepository.SaveChangesAsync();
                payment.Id = paymentEntity.Id;
                return payment;
            }
            

            throw new Exception("El préstamo no existe en la base de datos");
        }
        public async Task DeletePaymentAsync(int paymentId)
        {
            var payment = await GetPaymentAsync(paymentId);

            if(payment != null)
                await _entitiesInfoService.UpdateEntityInfo(payment.EntityInfoId, 0);
        }

        public async Task DeletePaymentsByLoanAsync(int loanId)
        {
            var payments = await _paymentRepository.GetAllWithFilterAsync(p => p.LoanId == loanId);

            if(payments.Count > 0)
                foreach(var payment in payments)
                    await _entitiesInfoService.UpdateEntityInfo(payment.EntityInfoId, 0);
        }

        public async Task<PaymentDto> GetPaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.FirstOrDefaultWithRelationsAsync(p => p.Id == paymentId && p.EntityInfo.Status == 1);

            if(payment != null)
            {
                var paymentDto = _mapper.Map<PaymentDto>(payment);
                return paymentDto;
            }

            throw new Exception("Este pago no existe");
        }

        public async Task<List<PaymentDto>> GetPaymentsAsync(int lenderBusinessId)
        {
            var payments = await _paymentRepository.GetAllByFilterWithRelationsAsync(p => p.LenderBusinessId == lenderBusinessId && p.EntityInfo.Status == 1);

            var paymentsDto = _mapper.Map<List<PaymentDto>>(payments);
            return paymentsDto;
        }

        public async Task<List<PaymentDto>> GetPaymentsByUserIdAsync(int lenderBusinessId, int userId)
        {
            var payments = await _paymentRepository.GetAllByFilterWithRelationsAsync(p => p.LenderBusinessId == lenderBusinessId && p.UserId == userId && p.EntityInfo.Status == 1);

            var paymentsDto = _mapper.Map<List<PaymentDto>>(payments);
            return paymentsDto;
        }

        public async Task<PaymentDto> UpdatePaymentAsync(PaymentDto payment)
        {
            var loan = await _loanRepository.FirstOrDefaultAsync(l => l.Id == payment.LoanId);

            if(loan != null)
            {
                var paymentInDb = await _paymentRepository.FirstOrDefaultAsync(p => p.Id == payment.Id);
                paymentInDb.Date = payment.Date;
                paymentInDb.Pay = payment.Pay;

                await _entitiesInfoService.UpdateEntityInfo(paymentInDb.EntityInfoId, 1);

                _paymentRepository.Update(paymentInDb);
                await _paymentRepository.SaveChangesAsync();
                return payment;
            }

            throw new Exception("El préstamo seleccionado no existe");
        }
    }
}
