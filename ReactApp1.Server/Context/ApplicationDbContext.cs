using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<LiveChatMessage> LiveChatMessages { get; set; }
        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Converter cho bool <=> NUMBER(1)
            var boolToNumberConverter = new ValueConverter<bool, int>(
                v => v ? 1 : 0,
                v => v == 1);

            // Cấu hình ApplicationUser (IdentityUser mở rộng)
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.EmailConfirmed)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)");

                entity.Property(e => e.PhoneNumberConfirmed)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)");

                entity.Property(e => e.TwoFactorEnabled)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)");

                entity.Property(e => e.LockoutEnabled)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)");
            });

            // Khóa chính các bảng
            builder.Entity<Category>().HasKey(c => c.CategoryID);
            builder.Entity<Product>().HasKey(p => p.ProductID);
            builder.Entity<ProductOption>().HasKey(po => po.OptionID);
            builder.Entity<CartItem>().HasKey(ci => ci.CartItemID);
            builder.Entity<Order>().HasKey(o => o.OrderID);
            builder.Entity<OrderDetail>().HasKey(od => od.OrderDetailID);
            builder.Entity<Payment>().HasKey(p => p.PaymentID);
            builder.Entity<Reservation>().HasKey(r => r.ReservationID);
            builder.Entity<Delivery>().HasKey(d => d.DeliveryID);
            builder.Entity<Review>().HasKey(r => r.ReviewID);
            builder.Entity<Promotion>().HasKey(p => p.PromotionID);
            builder.Entity<LiveChatMessage>().HasKey(m => m.MessageID);
            builder.Entity<FAQ>().HasKey(f => f.FAQID);

            // Cấu hình entity Product với converter cho bool
            builder.Entity<Category>(entity =>
            {
                entity.Property(c => c.IsActive)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)");
            });
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductID);

                entity.Property(p => p.ProductName)
                      .IsRequired()
                      .HasColumnType("NVARCHAR2(2000)");

                entity.Property(p => p.Description)
                      .IsRequired()
                      .HasColumnType("NVARCHAR2(2000)");

                entity.Property(p => p.Price)
                      .HasPrecision(18, 2);

                entity.Property(p => p.ImageUrl)
                      .IsRequired()
                      .HasColumnType("NVARCHAR2(2000)");

                entity.Property(p => p.CategoryID)
                      .IsRequired()
                      .HasColumnType("NUMBER(10)");

                entity.Property(p => p.IsVegetarian)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)")
                      .IsRequired();

                entity.Property(p => p.IsBestseller)
                      .HasConversion(boolToNumberConverter)
                      .HasColumnType("NUMBER(1)")
                      .IsRequired();

                entity.Property(p => p.CreatedAt)
                      .HasColumnType("TIMESTAMP(7)")
                      .IsRequired();

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình entity Category
            builder.Entity<Category>(entity =>
            {
                entity.Property(c => c.IsActive)
                      .HasColumnType("NUMBER(1)")
                      .HasConversion(
                          v => v ? 1 : 0,
                          v => v == 1);
            });

            // Các quan hệ khác (ProductOption, CartItem, Order, OrderDetail, Payment, Reservation, Delivery, Review, LiveChatMessage)
            builder.Entity<ProductOption>()
                .HasOne(po => po.Product)
                .WithMany(p => p.ProductOptions)
                .HasForeignKey(po => po.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Option)
                .WithMany(po => po.CartItems)
                .HasForeignKey(ci => ci.OptionID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Option)
                .WithMany(po => po.OrderDetails)
                .HasForeignKey(od => od.OptionID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Delivery>()
                .HasOne(d => d.Order)
                .WithMany(o => o.Deliveries)
                .HasForeignKey(d => d.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LiveChatMessage>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LiveChatMessage>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình precision cho các trường số tiền
            builder.Entity<Order>(entity =>
            {
                entity.Property(e => e.TotalAmount)
                      .HasPrecision(18, 2);
            });

            builder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasPrecision(18, 2);
            });

            builder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Amount)
                      .HasPrecision(18, 2);
            });

            builder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasPrecision(18, 2);
            });

            builder.Entity<ProductOption>(entity =>
            {
                entity.Property(e => e.AdditionalPrice)
                      .HasPrecision(18, 2);
            });
        }
    }
}
