﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class ComeStore2TakeSingleOrders
    {
        public int ComeStoreId { get; set; }
        public DateTime ComeStoreDate { get; set; }
        public int ShippingMethodId { get; set; }

        public virtual SingleShippingMethod ShippingMethod { get; set; }
    }
}