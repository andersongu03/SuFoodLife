﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SuFood.Models
{
    public partial class Products
    {
        public Products()
        {
            ProductsOfPlans = new HashSet<ProductsOfPlans>();
            ShoppingCart = new HashSet<ShoppingCart>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string StockUnit { get; set; }
        public int StockQuantity { get; set; }
        public int Price { get; set; }
        public int Cost { get; set; }
        public string Category { get; set; }
        public byte[] Img { get; set; }

        public virtual ICollection<ProductsOfPlans> ProductsOfPlans { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}