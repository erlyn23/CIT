using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class PageDto
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string IconClass { get; set; }
        public string Route { get; set; }
    }
}
