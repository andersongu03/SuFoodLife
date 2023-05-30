﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class Account
    {
        public Account()
        {
            CouponUsedList = new HashSet<CouponUsedList>();
            Orders = new HashSet<Orders>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int AccountId { get; set; }
        public string Account1 { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string DefaultShipAddress { get; set; }
        public string DefaultCreditCardNumber { get; set; }
        public string DefaultCreditCardHolder { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public byte[] LasttImeLogin { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
        public string HashPassword { get; set; }

        public virtual ICollection<CouponUsedList> CouponUsedList { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}