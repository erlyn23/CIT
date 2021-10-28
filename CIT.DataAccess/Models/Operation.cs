﻿using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Operation
    {
        public Operation()
        {
            Id = Guid.NewGuid().ToString().Trim('-');
        }
        public string Id { get; set; }
        public string OperationName { get; set; }

        public virtual Rolepermission Rolepermission { get; set; }
    }
}
