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
        private readonly ILoanService _loanService;
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, 
            ILoanService loanService, 
            IEntitiesInfoService entitiesInfoService,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _loanService = loanService;
            _entitiesInfoService = entitiesInfoService;
            _mapper = mapper;
        }
        public async Task<PaymentDto> AddPaymentAsync(PaymentDto payment, int lenderBusinessId)
        {
            var paymentEntity = _mapper.Map<Payment>(payment);
            var loan = await _loanService.GetloanByIdAsync(payment.LoanId);

            if(loan != null)
            {
                var entityInfo = await _entitiesInfoService.AddEntityInfoAsync();
                payment.EntityInfoId = entityInfo.Id;
                payment.LenderBusinessId = lenderBusinessId;

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
            var payment = await _paymentRepository.FirstOrDefaultWithRelationsAsync(p => p.Id == paymentId);

            if(payment != null)
            {
                var paymentDto = _mapper.Map<PaymentDto>(payment);
                return paymentDto;
            }

            throw new Exception("Este pago no existe");
        }

        public async Task<List<PaymentDto>> GetPaymentsAsync(int lenderBusinessId)
        {
            var payments = await _paymentRepository.GetAllByFilterWithRelationsAsync(p => p.LenderBusinessId == lenderBusinessId);

            var paymentsDto = _mapper.Map<List<PaymentDto>>(payments);
            return paymentsDto;
        }

        public async Task<List<PaymentDto>> GetPaymentsByUserIdAsync(int userId)
        {
            var payments = await _paymentRepository.GetAllByFilterWithRelationsAsync(p => p.UserId == userId);

            var paymentsDto = _mapper.Map<List<PaymentDto>>(payments);
            return paymentsDto;
        }

        public async Task<PaymentDto> UpdatePaymentAsync(PaymentDto payment)
        {
            var loan = await _loanService.GetloanByIdAsync(payment.LoanId);

            if(loan != null)
            {
                var paymentInDb = await _paymentRepository.FirstOrDefaultAsync(p => p.Id == payment.Id);
                paymentInDb.Date = payment.Date;
                paymentInDb.Pay = payment.Pay;

                _paymentRepository.Update(paymentInDb);
                await _paymentRepository.SaveChangesAsync();
                return payment;
            }

            throw new Exception("El préstamo seleccionado no existe");
        }
    }
}
