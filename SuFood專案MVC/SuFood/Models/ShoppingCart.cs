﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class ShoppingCart
    {
        public int CartId { get; set; }
        public int AccountId { get; set; }
        public int? CartQuantity { get; set; }
        public int ProductId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Products Product { get; set; }
    }
}