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
            OrdersDetails = new HashSet<OrdersDetails>();
            OrdersReview = new HashSet<OrdersReview>();
            RecyleSubscribeOrders = new HashSet<RecyleSubscribeOrders>();
            SingleShippingMethod = new HashSet<SingleShippingMethod>();
        }

        public int OrdersId { get; set; }
        public int SubTotal { get; set; }
        public int? SubCost { get; set; }
        public double? SubDiscount { get; set; }
        public DateTime? SetOrdersDatetime { get; set; }
        public string ShipAddress { get; set; }
        public string OrderStatus { get; set; }
        public int? ShippingMethodId { get; set; }
        public int? AccountId { get; set; }
        public int? CouponId { get; set; }
        public int? CustomerPaymentId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ReMark { get; set; }
        public string Email { get; set; }
        public string BuyMethod { get; set; }

        public virtual Account Account { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual ICollection<OrdersDetails> OrdersDetails { get; set; }
        public virtual ICollection<OrdersReview> OrdersReview { get; set; }
        public virtual ICollection<RecyleSubscribeOrders> RecyleSubscribeOrders { get; set; }
        public virtual ICollection<SingleShippingMethod> SingleShippingMethod { get; set; }
    }
}