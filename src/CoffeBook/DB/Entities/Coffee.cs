﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Entities
{
    public class Coffee
    {
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public byte[] Picture { get; set; }
    }
}
