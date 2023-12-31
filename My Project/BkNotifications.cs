﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace My_Project
{
    public partial class BkNotifications
    {
        [Key]
        [Column("BkID")]
        public int BkId { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        [Required]
        [StringLength(160)]
        public string BkName { get; set; }
        [Required]
        [StringLength(160)]
        public string BkAuthor { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime BkLeasedDate { get; set; }
    }
}