﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class Orders
    {
        public Orders()
        {
            CustomerPayment = new HashSet<CustomerPayment>();
            OrdersDetails = new HashSet<OrdersDetails>();
            OrdersReview = new HashSet<OrdersReview>();
            SingleShippingMethod = new HashSet<SingleShippingMethod>();
        }

        public int OrdersId { get; set; }
        public int? SubTotal { get; set; }
        public int? SubCost { get; set; }
        public double? SubDiscount { get; set; }
        public DateTime? SetOrdersDatetime { get; set; }
        public string ShipAddress { get; set; }
        public string OrderStatus { get; set; }
        public int? ShippingMethodId { get; set; }
        public int? AccountId { get; set; }
        public int? CouponId { get; set; }
        public int? OrdersDetailsId { get; set; }
        public int? CustomerPaymentId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual ICollection<CustomerPayment> CustomerPayment { get; set; }
        public virtual ICollection<OrdersDetails> OrdersDetails { get; set; }
        public virtual ICollection<OrdersReview> OrdersReview { get; set; }
        public virtual ICollection<SingleShippingMethod> SingleShippingMethod { get; set; }
    }
}