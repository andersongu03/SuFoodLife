﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class ShippingSingleOrders
    {
        public int SsingleOrdersId { get; set; }
        public DateTime ShippingDate { get; set; }
        public int ShippingCost { get; set; }
        public int ShippingMethodId { get; set; }

        public virtual SingleShippingMethod ShippingMethod { get; set; }
    }
}