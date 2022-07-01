using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Tools
{
    public class DateTimeOperationsTool
    {
        public static int GetLastMonthDay()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime neededTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            do
            {
                dates.Add(neededTime);
                neededTime = neededTime.AddDays(1);
            } while (neededTime.Month == DateTime.Now.Month);

            return dates.LastOrDefault().Day;
        }
    }
}
