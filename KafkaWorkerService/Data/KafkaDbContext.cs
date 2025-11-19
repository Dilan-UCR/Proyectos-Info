using Microsoft.EntityFrameworkCore;
using KafkaWorkerService.Models;

namespace KafkaWorkerService.Data;

public class KafkaDbContext : DbContext
{
    public KafkaDbContext(DbContextOptions<KafkaDbContext> options) : base(options)
    {
    }

    public DbSet<KafkaLog> KafkaLogs { get; set; } = null!;
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;
    public DbSet<HangfireLog> HangfireLogs { get; set; } = null!;
    public DbSet<PdfLog> PdfLogs { get; set; } = null!;
    public DbSet<StorageLog> StorageLogs { get; set; } = null!;
    public DbSet<MessageLog> MessageLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //config todos logs
        modelBuilder.Entity<KafkaLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Topic).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.HasIndex(e => e.Topic);
            entity.HasIndex(e => e.CreatedAt);
        });
        //config email
        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CorrelationId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CustomerId).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RecipientEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.Date).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Time).IsRequired().HasMaxLength(10);
            entity.HasIndex(e => e.CorrelationId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // config Hangfire
        modelBuilder.Entity<HangfireLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CorrelationId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Service).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Endpoint).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Success).IsRequired();
            entity.HasIndex(e => e.CorrelationId).IsUnique();
            entity.HasIndex(e => e.CreatedAt);
        });

        // config de PdfLog
        modelBuilder.Entity<PdfLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CorrelationId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Service).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Endpoint).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Success).IsRequired();
            entity.HasIndex(e => e.CorrelationId).IsUnique();
            entity.HasIndex(e => e.CreatedAt);
        });

        // Config storage
        modelBuilder.Entity<StorageLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.CorrelationId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.HasIndex(e => e.CorrelationId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Config MessageLog
        modelBuilder.Entity<MessageLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CorrelationId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Platform).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ChatId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Message).IsRequired();
            entity.HasIndex(e => e.CorrelationId);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}
