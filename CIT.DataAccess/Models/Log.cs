using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Log
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string ResultMessageOrObject { get; set; }
        public int LenderBusinessId { get; set; }
        public DateTime LogDate { get; set; }

        public virtual LenderBusiness LenderBusiness { get; set; }
    }
}
