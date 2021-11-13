using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class LogDto
    {
        public int LogId { get; set; }
        public string Operation { get; set; }
        public string ResultMessageOrObject { get; set; }
        public int LenderBusinessId { get; set; }
        public DateTime LogDate { get; set; }

    }
}
