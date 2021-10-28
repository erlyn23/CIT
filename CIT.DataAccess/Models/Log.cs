using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Log
    {
        public Log()
        {
            Id = Guid.NewGuid().ToString().Trim('-');
        }
        public string Id { get; set; }
        public string Operation { get; set; }
        public string UserId { get; set; }
        public string ResultMessageOrObject { get; set; }
        public DateTime LogDate { get; set; }

        public virtual User User { get; set; }
    }
}
