﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace My_Project
{
    public partial class TeacherApproving
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(160)]
        public string TeacherName { get; set; }
        [Required]
        [StringLength(160)]
        public string TeacherMail { get; set; }
        [Required]
        [StringLength(160)]
        public string Approval { get; set; }
    }
}