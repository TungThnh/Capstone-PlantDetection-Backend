using System;
using System.Collections.Generic;
using Microsoft.EntityFrameWorkCore;

namespace PlantDetection_Service.Domain.Entities

public partial class PlantDetectionContext : DbContext
{
    public PlantDetectionContext() { }

    public PlantDetectionContext(DbContextOptions<PlantDetectionContext> options)
        : base (options) { }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Plant> Plants { get; set; }

    public virtual DbSet<PlantCategory> PlantCategories { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentClass> StudentClasses { get; set; }


    protected override void OnModeCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC0788CEECB2");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC0718DE59BC");

            entity.ToTable("Class");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e =>e.Name).HasMaxLength(256);
        })
    }
}

