﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class SingleShippingMethod
    {
        public SingleShippingMethod()
        {
            ComeStore2TakeSingleOrders = new HashSet<ComeStore2TakeSingleOrders>();
            ShippingSingleOrders = new HashSet<ShippingSingleOrders>();
        }

        public int ShippingMethodId { get; set; }
        public string ShippingMethod { get; set; }
        public int OrdersId { get; set; }

        public virtual Orders Orders { get; set; }
        public virtual ICollection<ComeStore2TakeSingleOrders> ComeStore2TakeSingleOrders { get; set; }
        public virtual ICollection<ShippingSingleOrders> ShippingSingleOrders { get; set; }
    }
}