using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ILoanService _loanService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly IVehicleAssignmentService _vehicleAssignmentService;
        private readonly int _currentYear = DateTime.Now.Year;
        private readonly int _currentMonth = DateTime.Now.Month;


        public DashboardService(ILoanService loanService, 
            IPaymentService paymentService,
            IUserService userService,
            IVehicleAssignmentService vehicleAssignmentService)
        {
            _loanService = loanService;
            _paymentService = paymentService;
            _userService = userService;
            _vehicleAssignmentService = vehicleAssignmentService;
        }


        public async Task<DashboardDto> GetLenderBusinessDashboardAsync(int lenderBusinessId)
        {
            var users = await _userService.GetUsersAsync(lenderBusinessId);
            var loans = await _loanService.GetLoansByLenderBusinessAsync(lenderBusinessId);
            var payments = await _paymentService.GetPaymentsAsync(lenderBusinessId);
            var vehiclesAssignments = await _vehicleAssignmentService.GetVehiclesAssignmentsAsync(lenderBusinessId);


            var usersPerDay = GetUsersCountPerDay(users);
            var loansPerDay = GetLoansCountPerDay(loans);
            var paymentsPerDay = GetPaymentsCountPerDay(payments);
            var assignmentsPerDay = GetVehicleAssignmentsPerDay(vehiclesAssignments);

            return new DashboardDto()
            {
                TotalUsersQuantity = usersPerDay.Sum(x => x.Count),
                TotalLoansQuantity = loansPerDay.Sum(x => x.Count),
                TotalPaymentsQuantity = paymentsPerDay.Sum(x => x.Count),
                TotalVehicleAssignmentsQuantity = assignmentsPerDay.Sum(x => x.Count),

                UserQuantityPerDay = usersPerDay,
                LoanQuantityPerDay = loansPerDay,
                PaymentQuantityPerDay = paymentsPerDay,
                VehicleAssignmentsQuantityPerDay = assignmentsPerDay
            };
        }

        public async Task<DashboardDto> GetUserDashboardAsync(int lenderBusinessId, int userId)
        {
            var loans = await _loanService.GetLoansByUserAsync(lenderBusinessId, userId);
            var payments = await _paymentService.GetPaymentsByUserIdAsync(lenderBusinessId, userId);


            var loansPerDay = GetLoansCountPerDay(loans);
            var paymentsPerDay = GetPaymentsCountPerDay(payments);

            return new DashboardDto()
            {
                TotalLoansQuantity = loansPerDay.Sum(x => x.Count),
                TotalPaymentsQuantity = paymentsPerDay.Sum(x => x.Count),

                LoanQuantityPerDay = loansPerDay,
                PaymentQuantityPerDay = paymentsPerDay
            };
        }

        private List<PerDayDto> GetUsersCountPerDay(List<UserDto> users)
        {
            var usersQuantityPerDay = users.Where(u => DateTime.Now.DayOfYear - u.EntityInfo.CreatedAt.DayOfYear < 30).GroupBy(u => FormatDateWithoutHour(u.EntityInfo)).Select(x => new { Key = x.Key, TotalUsers = x.Count() });

            var usersPerDay = new List<PerDayDto>();
            
            for(int day = 1; day <= DateTime.DaysInMonth(_currentYear, _currentMonth); day++)
            {
                var countPerDay = usersQuantityPerDay.Where(x => x.Key.Month == _currentMonth && x.Key.Year == _currentYear && x.Key.Day == day).FirstOrDefault();

                var userPerDay = new PerDayDto()
                {
                    Day = new DateTime(_currentYear, _currentMonth, day)
                };

                if (countPerDay != null) userPerDay.Count = countPerDay.TotalUsers;
                else userPerDay.Count = 0;

                usersPerDay.Add(userPerDay);
            }

            return usersPerDay;
        }

        private List<PerDayDto> GetLoansCountPerDay(List<LoanDto> loans)
        {
            var loansQuantityPerDay = loans.Where(u => DateTime.Now.DayOfYear - u.EntityInfo.CreatedAt.DayOfYear < 30).GroupBy(u => FormatDateWithoutHour(u.EntityInfo)).Select(x => new { Key = x.Key, TotalLoans = x.Count() });

            var loansPerDay = new List<PerDayDto>();
            for (int day = 1; day <= DateTime.DaysInMonth(_currentYear, _currentMonth); day++)
            {
                var countPerDay = loansQuantityPerDay.Where(x => x.Key.Month == _currentMonth && x.Key.Year == _currentYear && x.Key.Day == day).FirstOrDefault();


                var loanPerDay = new PerDayDto
                {
                    Day = new DateTime(_currentYear, _currentMonth, day)
                };

                if (countPerDay != null) loanPerDay.Count = countPerDay.TotalLoans;
                else loanPerDay.Count = 0;


                loansPerDay.Add(loanPerDay);
            }

            return loansPerDay;
        }

        private List<PerDayDto> GetPaymentsCountPerDay(List<PaymentDto> payments)
        {
            var paymentsQuantityPerDay = payments.Where(u => DateTime.Now.DayOfYear - u.EntityInfo.CreatedAt.DayOfYear < 30).GroupBy(u => FormatDateWithoutHour(u.EntityInfo)).Select(x => new { Key = x.Key, TotalPayments = x.Count() });

            var paymentsPerDay = new List<PerDayDto>();
            for (int day = 1; day <= DateTime.DaysInMonth(_currentYear, _currentMonth); day++)
            {
                var countPerDay = paymentsQuantityPerDay.Where(x => x.Key.Month == _currentMonth && x.Key.Year == _currentYear && x.Key.Day == day).FirstOrDefault();


                var paymentPerDay = new PerDayDto
                {
                    Day = new DateTime(_currentYear, _currentMonth, day)
                };

                if (countPerDay != null) paymentPerDay.Count = countPerDay.TotalPayments;
                else paymentPerDay.Count = 0;


                paymentsPerDay.Add(paymentPerDay);
            }

            return paymentsPerDay;
        }

        private List<PerDayDto> GetVehicleAssignmentsPerDay(List<VehicleAssignmentDto> vehicleAssignments)
        {
            var assignmentsQuantityPerDay = vehicleAssignments.Where(u => DateTime.Now.DayOfYear - u.AssignmentDate.DayOfYear < 30).GroupBy(u => u.AssignmentDate).Select(x => new { Key = x.Key, TotalAssignments = x.Count() });

            var assignmentsPerDay = new List<PerDayDto>();
            for (int day = 1; day <= DateTime.DaysInMonth(_currentYear, _currentMonth); day++)
            {
                var countPerDay = assignmentsQuantityPerDay.Where(x => x.Key.Month == _currentMonth && x.Key.Year == _currentYear && x.Key.Day == day).FirstOrDefault();


                var assignmentPerDay = new PerDayDto() { Day = new DateTime(_currentYear, _currentMonth, day) };

                if (countPerDay != null) assignmentPerDay.Count = countPerDay.TotalAssignments;
                else assignmentPerDay.Count = 0;

                assignmentsPerDay.Add(assignmentPerDay);
            }

            return assignmentsPerDay;
        }


        private DateTime FormatDateWithoutHour(EntityInfoDto entityInfo) => 
            new DateTime(entityInfo.CreatedAt.Year, entityInfo.CreatedAt.Month, entityInfo.CreatedAt.Day);
    }
}
