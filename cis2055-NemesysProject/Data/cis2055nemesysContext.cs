using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using cis2055_NemesysProject.Models;
#nullable disable

namespace cis2055_NemesysProject.Data
{
    public partial class cis2055nemesysContext : DbContext
    {
        public cis2055nemesysContext()
        {
        }

        public cis2055nemesysContext(DbContextOptions<cis2055nemesysContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hazard> Hazards { get; set; }
        public virtual DbSet<Investigation> Investigations { get; set; }
        public virtual DbSet<LogInvestigation> LogInvestigations { get; set; }
        public virtual DbSet<Pinpoint> Pinpoints { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportHazard> ReportHazards { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StatusCategory> StatusCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=cis2055-nemesys;Integrated Security=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Hazard>(entity =>
            {
                entity.HasKey(e => e.HarzardId);

                entity.Property(e => e.HarzardId).HasColumnName("Harzard_ID");

                entity.Property(e => e.HazardType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Investigation>(entity =>
            {
                entity.Property(e => e.InvestigationId).HasColumnName("Investigation_ID");

                entity.Property(e => e.ReportId).HasColumnName("Report_ID");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.Investigations)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Investigations_Reports");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Investigations)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Investigations_StatusCategory");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Investigations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Investigations_Users");
            });

            modelBuilder.Entity<LogInvestigation>(entity =>
            {
                entity.Property(e => e.LogInvestigationId).HasColumnName("LogInvestigation_ID");

                entity.Property(e => e.DateOfAction).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.InvestigationId).HasColumnName("Investigation_ID");

                entity.HasOne(d => d.Investigation)
                    .WithMany(p => p.LogInvestigations)
                    .HasForeignKey(d => d.InvestigationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LogInvestigations_Investigations");
            });

            modelBuilder.Entity<Pinpoint>(entity =>
            {
                entity.Property(e => e.PinpointId).HasColumnName("Pinpoint_ID");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.ReportId).HasColumnName("Report_ID");

                entity.Property(e => e.DateOfReport).HasColumnType("date");

                entity.Property(e => e.DateTimeHazard).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.PinpointId).HasColumnName("Pinpoint_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.Pinpoint)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PinpointId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reports_Pinpoints");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reports_Users");
            });

            modelBuilder.Entity<ReportHazard>(entity =>
            {
                entity.HasKey(e => new { e.HazardId, e.ReportId });

                entity.ToTable("ReportHazard");

                entity.Property(e => e.HazardId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("Hazard_ID");

                entity.Property(e => e.ReportId).HasColumnName("Report_ID");

                entity.HasOne(d => d.Hazard)
                    .WithMany(p => p.ReportHazards)
                    .HasForeignKey(d => d.HazardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportHazard_Hazard");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ReportHazards)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportHazard_Reports");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.RoleType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StatusCategory>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_Status Category");

                entity.ToTable("StatusCategory");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.StatusType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
