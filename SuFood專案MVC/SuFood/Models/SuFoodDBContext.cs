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
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<ComeStore2TakeSingleOrders> ComeStore2TakeSingleOrders { get; set; }
        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<CouponUsedList> CouponUsedList { get; set; }
        public virtual DbSet<CustomerPayment> CustomerPayment { get; set; }
        public virtual DbSet<FreeChoicePlans> FreeChoicePlans { get; set; }
        public virtual DbSet<FreeChoiceProducts> FreeChoiceProducts { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersDetails> OrdersDetails { get; set; }
        public virtual DbSet<OrdersReview> OrdersReview { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsOfPlans> ProductsOfPlans { get; set; }
        public virtual DbSet<RecyleSubscribeOrders> RecyleSubscribeOrders { get; set; }
        public virtual DbSet<RetailsList> RetailsList { get; set; }
        public virtual DbSet<ShippingSingleOrders> ShippingSingleOrders { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<ShoppingMethod> ShoppingMethod { get; set; }
        public virtual DbSet<SingleShippingMethod> SingleShippingMethod { get; set; }
        public virtual DbSet<SubscribeList> SubscribeList { get; set; }

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

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.AnnouncementId).HasColumnName("Announcement_Id");

                entity.Property(e => e.AnnouncementContent)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("Announcement_Content");

                entity.Property(e => e.AnnouncementEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Announcement_EndDate");

                entity.Property(e => e.AnnouncementImage).HasColumnName("Announcement_Image");

                entity.Property(e => e.AnnouncementStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Announcement_StartDate");

                entity.Property(e => e.AnnouncementStatus).HasColumnName("Announcement_Status");
            });

            modelBuilder.Entity<ComeStore2TakeSingleOrders>(entity =>
            {
                entity.HasKey(e => e.ComeStoreId);

                entity.ToTable("ComeStore2Take_SingleOrders");

                entity.Property(e => e.ComeStoreId).HasColumnName("ComeStore_Id");

                entity.Property(e => e.ComeStoreDate)
                    .HasColumnType("date")
                    .HasColumnName("ComeStore_date");

                entity.Property(e => e.ShippingMethodId).HasColumnName("Shipping_method_Id");

                entity.HasOne(d => d.ShippingMethod)
                    .WithMany(p => p.ComeStore2TakeSingleOrders)
                    .HasForeignKey(d => d.ShippingMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SingleShipping_method_TO_ComeStore2Take_SingleOrders");
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

            modelBuilder.Entity<CustomerPayment>(entity =>
            {
                entity.ToTable("Customer_Payment");

                entity.Property(e => e.CustomerPaymentId).HasColumnName("Customer_PaymentId");

                entity.Property(e => e.AccountId).HasColumnName("Account_Id");

                entity.Property(e => e.CreditCardExpiryDate)
                    .HasMaxLength(10)
                    .HasColumnName("CreditCard_ExpiryDate")
                    .IsFixedLength();

                entity.Property(e => e.CreditCardHolder).HasColumnName("CreditCard_Holder");

                entity.Property(e => e.CreditCardNumber).HasColumnName("CreditCard_Number");

                entity.Property(e => e.OrdersId).HasColumnName("Orders_Id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.CustomerPayment)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Customer_Payment_Account");

                entity.HasOne(d => d.Orders)
                    .WithMany(p => p.CustomerPayment)
                    .HasForeignKey(d => d.OrdersId)
                    .HasConstraintName("FK_Customer_Payment_Orders");
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

            modelBuilder.Entity<FreeChoiceProducts>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.PlanId })
                    .HasName("PK_FreeChoiceProducts_1");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Product_Id");

                entity.Property(e => e.ProductDescription)
                    .HasMaxLength(50)
                    .HasColumnName("Product_Description");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("Product_Name");

                entity.Property(e => e.ProductPrice).HasColumnName("Product_Price");

                entity.HasOne(d => d.Plan)
                    .WithMany(p => p.FreeChoiceProducts)
                    .HasForeignKey(d => d.PlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FreeChoiceProducts_FreeChoicePlans");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.OrdersId).HasColumnName("Orders_Id");

                entity.Property(e => e.AccountId).HasColumnName("Account_Id");

                entity.Property(e => e.CouponId).HasColumnName("Coupon_Id");

                entity.Property(e => e.CustomerPaymentId).HasColumnName("CustomerPayment_Id");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(10)
                    .HasColumnName("Order_Status");

                entity.Property(e => e.OrdersDetailsId).HasColumnName("Orders_Details_Id");

                entity.Property(e => e.SetOrdersDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("SetOrders_Datetime");

                entity.Property(e => e.ShipAddress).HasMaxLength(50);

                entity.Property(e => e.ShippingMethodId).HasColumnName("Shipping_method_Id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Account_TO_Orders");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Coopon_TO_Orders");
            });

            modelBuilder.Entity<OrdersDetails>(entity =>
            {
                entity.ToTable("Orders_details");

                entity.Property(e => e.OrdersDetailsId).HasColumnName("Orders_Details_Id");

                entity.Property(e => e.CouponId).HasColumnName("Coupon_Id");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("Product_Name");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrdersDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Orders_details_Orders");
            });

            modelBuilder.Entity<OrdersReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.ToTable("Orders_Review");

                entity.Property(e => e.ReviewId)
                    .ValueGeneratedNever()
                    .HasColumnName("Review_Id");

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.OrdersId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Orders_Id");

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

            modelBuilder.Entity<RecyleSubscribeOrders>(entity =>
            {
                entity.HasKey(e => e.ReSubOrdersId);

                entity.ToTable("RecyleSubscribe_Orders");

                entity.Property(e => e.ReSubOrdersId).HasColumnName("ReSubOrders_Id");

                entity.Property(e => e.OrdersId).HasColumnName("Orders_Id");

                entity.Property(e => e.ShipCumulativeFrequency).HasColumnName("Ship_CumulativeFrequency");

                entity.Property(e => e.ShipEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Ship_EndDate");

                entity.Property(e => e.ShipStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Ship_StartDate");

                entity.Property(e => e.ShipTotalFrequency).HasColumnName("Ship_TotalFrequency");

                entity.Property(e => e.SingleShipCost).HasColumnName("singleShip_Cost");

                entity.Property(e => e.SingleShipDate).HasColumnName("singleShip_date");
            });

            modelBuilder.Entity<RetailsList>(entity =>
            {
                entity.HasKey(e => e.RetailsId);

                entity.ToTable("Retails_List");

                entity.Property(e => e.RetailsId).HasColumnName("Retails_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RetailsList)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_TO_Retails_List");
            });

            modelBuilder.Entity<ShippingSingleOrders>(entity =>
            {
                entity.HasKey(e => e.SsingleOrdersId);

                entity.ToTable("Shipping_SingleOrders");

                entity.Property(e => e.SsingleOrdersId).HasColumnName("SSingleOrders_Id");

                entity.Property(e => e.ShippingCost).HasColumnName("Shipping_Cost");

                entity.Property(e => e.ShippingDate)
                    .HasColumnType("date")
                    .HasColumnName("Shipping_Date");

                entity.Property(e => e.ShippingMethodId).HasColumnName("Shipping_method_Id");

                entity.HasOne(d => d.ShippingMethod)
                    .WithMany(p => p.ShippingSingleOrders)
                    .HasForeignKey(d => d.ShippingMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SingleShipping_method_TO_Shipping_SingleOrders");
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

            modelBuilder.Entity<ShoppingMethod>(entity =>
            {
                entity.ToTable("Shopping_method");

                entity.HasIndex(e => e.Methods, "UQ_methods")
                    .IsUnique();

                entity.Property(e => e.ShoppingMethodId)
                    .HasMaxLength(1)
                    .HasColumnName("Shopping_method_Id");

                entity.Property(e => e.Free2ChoiceId).HasColumnName("Free2Choice_Id");

                entity.Property(e => e.Methods)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("methods");

                entity.Property(e => e.RetailsId).HasColumnName("Retails_Id");

                entity.Property(e => e.SubscribeId).HasColumnName("Subscribe_Id");

                entity.HasOne(d => d.Retails)
                    .WithMany(p => p.ShoppingMethod)
                    .HasForeignKey(d => d.RetailsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Retails_List_TO_Shopping_method");

                entity.HasOne(d => d.Subscribe)
                    .WithMany(p => p.ShoppingMethod)
                    .HasForeignKey(d => d.SubscribeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subscribe_List_TO_Shopping_method");
            });

            modelBuilder.Entity<SingleShippingMethod>(entity =>
            {
                entity.HasKey(e => e.ShippingMethodId);

                entity.ToTable("SingleShipping_method");

                entity.HasIndex(e => e.ShippingMethod, "UQ_Shipping_method")
                    .IsUnique();

                entity.Property(e => e.ShippingMethodId).HasColumnName("Shipping_method_Id");

                entity.Property(e => e.OrdersId).HasColumnName("Orders_Id");

                entity.Property(e => e.ShippingMethod)
                    .HasMaxLength(15)
                    .HasColumnName("Shipping_method");

                entity.HasOne(d => d.Orders)
                    .WithMany(p => p.SingleShippingMethod)
                    .HasForeignKey(d => d.OrdersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_TO_SingleShipping_method");
            });

            modelBuilder.Entity<SubscribeList>(entity =>
            {
                entity.HasKey(e => e.SubscribeId);

                entity.ToTable("Subscribe_List");

                entity.Property(e => e.SubscribeId).HasColumnName("Subscribe_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SubscribeList)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_TO_Subscribe_List");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}