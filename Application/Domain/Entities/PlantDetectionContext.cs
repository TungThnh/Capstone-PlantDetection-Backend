using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public partial class PlantDetectionContext : DbContext
{
    public PlantDetectionContext()
    {
    }

    public PlantDetectionContext(DbContextOptions<PlantDetectionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassLabel> ClassLabels { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Plant> Plants { get; set; }

    public virtual DbSet<PlantCategory> PlantCategories { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionExam> QuestionExams { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentClass> StudentClasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07B3913FDD");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC07680D71EB");

            entity.ToTable("Class");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(256);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Note).HasMaxLength(256);
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Manager).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Class__ManagerId__6EF57B66");
        });

        modelBuilder.Entity<ClassLabel>(entity =>
        {
            entity.HasKey(e => new { e.ClassId, e.LabelId }).HasName("PK__ClassLab__E88EC57C61469D3A");

            entity.ToTable("ClassLabel");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassLabels)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClassLabe__Class__693CA210");

            entity.HasOne(d => d.Label).WithMany(p => p.ClassLabels)
                .HasForeignKey(d => d.LabelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClassLabe__Label__6A30C649");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC07F67D7956");

            entity.ToTable("Exam");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.Exams)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exam__StudentId__04E4BC85");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC074D995419");

            entity.ToTable("Image");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Plant).WithMany(p => p.Images)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Image__PlantId__6B24EA82");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Label__3214EC07EC55B55E");

            entity.ToTable("Label");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3214EC0743DFD152");

            entity.ToTable("Manager");

            entity.HasIndex(e => e.Email, "UQ__Manager__A9D1053417DF5BC0").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DayOfBirth).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Plant__3214EC0780A7ED13");

            entity.ToTable("Plant");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(256);
            entity.Property(e => e.ConservationStatus).HasMaxLength(256);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discoverer).HasMaxLength(256);
            entity.Property(e => e.DistributionArea).HasMaxLength(256);
            entity.Property(e => e.FruitTime).HasMaxLength(256);
            entity.Property(e => e.Genus).HasMaxLength(256);
            entity.Property(e => e.LivingCondition).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Ph)
                .HasMaxLength(256)
                .HasColumnName("PH");
            entity.Property(e => e.ScienceName).HasMaxLength(256);
            entity.Property(e => e.Size).HasMaxLength(256);
            entity.Property(e => e.Species).HasMaxLength(256);
            entity.Property(e => e.Status).HasMaxLength(256);
            entity.Property(e => e.Uses).HasMaxLength(256);
        });

        modelBuilder.Entity<PlantCategory>(entity =>
        {
            entity.HasKey(e => new { e.CategoryId, e.PlantId }).HasName("PK__PlantCat__C086D99E4BA6DA59");

            entity.ToTable("PlantCategory");

            entity.HasOne(d => d.Category).WithMany(p => p.PlantCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlantCate__Categ__6C190EBB");

            entity.HasOne(d => d.Plant).WithMany(p => p.PlantCategories)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlantCate__Plant__6D0D32F4");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC0700C45194");

            entity.ToTable("Question");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<QuestionExam>(entity =>
        {
            entity.HasKey(e => new { e.QuestionId, e.ExamId }).HasName("PK__Question__6F573DB0D0DF4485");

            entity.ToTable("QuestionExam");

            entity.HasOne(d => d.Exam).WithMany(p => p.QuestionExams)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionE__ExamI__09A971A2");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionExams)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionE__Quest__08B54D69");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07A96F2DE0");

            entity.ToTable("Report");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Label).WithMany(p => p.Reports)
                .HasForeignKey(d => d.LabelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Label");

            entity.HasOne(d => d.Student).WithMany(p => p.Reports)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__StudentI__5629CD9C");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3214EC07FCD17FD1");

            entity.ToTable("Result");

            entity.HasIndex(e => e.ReportId, "UQ__Result__D5BD48046B86B560").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Plant).WithMany(p => p.Results)
                .HasForeignKey(d => d.PlantId)
                .HasConstraintName("FK__Result__PlantId__6FE99F9F");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC079B6EBA29");

            entity.ToTable("Student");

            entity.HasIndex(e => e.Email, "UQ__Student__A9D1053439DE6B8C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.College).HasMaxLength(256);
            entity.Property(e => e.DayOfBirth).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.Status).HasMaxLength(256);
        });

        modelBuilder.Entity<StudentClass>(entity =>
        {
            entity.HasKey(e => new { e.ClassId, e.StudentId }).HasName("PK__StudentC__48357579EE19572F");

            entity.ToTable("StudentClass");

            entity.HasIndex(e => e.StudentId, "UQ__StudentC__32C52B98D4EA130E").IsUnique();

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.JoinAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(256);

            entity.HasOne(d => d.Class).WithMany(p => p.StudentClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentCl__Class__73BA3083");

            entity.HasOne(d => d.Student).WithOne(p => p.StudentClass)
                .HasForeignKey<StudentClass>(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentCl__Stude__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
