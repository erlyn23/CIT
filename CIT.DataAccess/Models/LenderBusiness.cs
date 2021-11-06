﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Models
{
    public class LenderBusiness
    {
        public LenderBusiness()
        {
            Users = new HashSet<User>();
            Roles = new HashSet<Role>();
            Loans = new HashSet<Loan>();
            Logs = new HashSet<Log>();
            Vehicles = new HashSet<Vehicle>();
        }
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Rnc { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public int EntityInfoId { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Loan> Loans { get; set; }
        public ICollection<Log> Logs { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public Useraddress UserAddress { get; set; }
    }
}
