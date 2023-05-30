﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SuFood.Models
{
    public partial class SuFoodDBContext : DbContext
    {
        public SuFoodDBContext()
        {
        }

        public SuFoodDBContext(DbContextOptions<SuFoodDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<CouponUsedList> CouponUsedList { get; set; }
        public virtual DbSet<FreeChoicePlans> FreeChoicePlans { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersReview> OrdersReview { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsOfPlans> ProductsOfPlans { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("Account_Id");

                entity.Property(e => e.Account1).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CreateDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("Create_Datetime");

                entity.Property(e => e.DefaultCreditCardHolder)
                    .HasMaxLength(10)
                    .HasColumnName("Default_CreditCard_Holder");

                entity.Property(e => e.DefaultCreditCardNumber)
                    .HasMaxLength(10)
                    .HasColumnName("Default_CreditCard_Number");

                entity.Property(e => e.DefaultShipAddress)
                    .HasMaxLength(50)
                    .HasColumnName("Default_ShipAddress");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(15)
                    .HasColumnName("First_Name");

                entity.Property(e => e.Gender).HasMaxLength(5);

                entity.Property(e => e.LastName)
                    .HasMaxLength(15)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.LasttImeLogin)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("LasttIme_Login");

                entity.Property(e => e.Phone).HasMaxLength(10);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.CouponId).HasColumnName("Coupon_Id");

                entity.Property(e => e.CouponDescription)
                    .HasMaxLength(50)
                    .HasColumnName("Coupon_Description");

                entity.Property(e => e.CouponEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Coupon_EndDate");

                entity.Property(e => e.CouponMinusCost)
                    .HasColumnType("decimal(5, 0)")
                    .HasColumnName("Coupon_MinusCost");

                entity.Property(e => e.CouponName)
                    .HasMaxLength(50)
                    .HasColumnName("Coupon_Name");

                entity.Property(e => e.CouponStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Coupon_StartDate");

                entity.Property(e => e.MinimumPurchasingAmount).HasColumnName("Minimum_PurchasingAmount");
            });

            modelBuilder.Entity<CouponUsedList>(entity =>
            {
                entity.HasKey(e => e.CouponUsedId)
                    .HasName("PK_CooponUSE_List");

                entity.ToTable("CouponUsed_List");

                entity.Property(e => e.CouponUsedId).HasColumnName("CouponUsed_Id");

                entity.Property(e => e.CouponId).HasColumnName("Coupon_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.UseCouponDate)
                    .HasColumnType("date")
                    .HasColumnName("UseCoupon_Date");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.CouponUsedList)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Coopon_TO_CooponUSE_List");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CouponUsedList)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Account_TO_CooponUSE_List");
            });

            modelBuilder.Entity<FreeChoicePlans>(entity =>
            {
                entity.HasKey(e => e.PlanId)
                    .HasName("PK__FreeChoi__755C22B73A4C2535");

                entity.Property(e => e.PlanCanChoiceCount).HasColumnName("Plan_CanChoiceCount");

                entity.Property(e => e.PlanDescription).HasColumnName("Plan_Description");

                entity.Property(e => e.PlanName).HasColumnName("Plan_Name");

                entity.Property(e => e.PlanPrice).HasColumnName("Plan_Price");

                entity.Property(e => e.PlanStatus).HasColumnName("Plan_Status");

                entity.Property(e => e.PlanTotalCount).HasColumnName("Plan_TotalCount");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.OrdersId).HasColumnName("Orders_Id");

                entity.Property(e => e.AccountId).HasColumnName("Account_Id");

                entity.Property(e => e.CouponId).HasColumnName("Coupon_Id");

                entity.Property(e => e.DiscountId).HasColumnName("Discount_Id");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(10)
                    .HasColumnName("Order_Status");

                entity.Property(e => e.OrdersDetailsId).HasColumnName("Orders_Details_Id");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(10)
                    .HasColumnName("Payment_method");

                entity.Property(e => e.SetOrdersDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("SetOrders_Datetime");

                entity.Property(e => e.ShipAddress).HasMaxLength(50);

                entity.Property(e => e.ShippingMethodId).HasColumnName("Shipping_method_Id");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_TO_Orders");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Coopon_TO_Orders");
            });

            modelBuilder.Entity<OrdersReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.ToTable("Orders_Review");

                entity.Property(e => e.ReviewId).HasColumnName("Review_Id");

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.OrdersId).HasColumnName("Orders_Id");

                entity.Property(e => e.RatingStar).HasColumnName("rating_star");

                entity.HasOne(d => d.Orders)
                    .WithMany(p => p.OrdersReview)
                    .HasForeignKey(d => d.OrdersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_TO_Orders_Review");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Product_Description");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Product_Name");

                entity.Property(e => e.StockQuantity).HasColumnName("Stock_Quantity");

                entity.Property(e => e.StockUnit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("Stock_Unit");
            });

            modelBuilder.Entity<ProductsOfPlans>(entity =>
            {
                entity.HasKey(e => new { e.PlanId, e.ProductId });

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.ProductsOfPlans)
                    .HasForeignKey(d => d.PlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductsOfPlans_FreeChoicePlans");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsOfPlans)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductsOfPlans_Products");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.CartId);

                entity.ToTable("Shopping_Cart");

                entity.Property(e => e.CartId).HasColumnName("Cart_Id");

                entity.Property(e => e.AccountId).HasColumnName("Account_Id");

                entity.Property(e => e.CartQuantity).HasColumnName("Cart_Quantity");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ShoppingCart)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_TO_Shopping_Cart");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCart)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_TO_Shopping_Cart");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}