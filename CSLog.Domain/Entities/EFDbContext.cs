namespace CSLog.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=EFDbContext")
        {
        }

        public  DbSet<AspNetRole> AspNetRoles { get; set; }
        public  DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<ComplaintActivity> ComplaintActivities { get; set; }
        public virtual DbSet<ComplaintStatus> ComplaintStatus { get; set; }
        public virtual DbSet<ComplaintType> ComplaintTypes { get; set; }
        public virtual DbSet<SolutionStatus> SolutionStatus { get; set; }
        //public virtual DbSet<SolutionType> SolutionTypes { get; set; }
        public virtual DbSet<vComplaint> vComplaints { get; set; }
        public virtual DbSet<vComplaintActivity> vComplaintActivities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.Title)
                .IsFixedLength();


            modelBuilder.Entity<Complaint>()
                .Property(e => e.Details)
                .IsUnicode(false);
            modelBuilder.Entity<Complaint>().
                Property(x => x.ResolvedBy).
                HasColumnName(@"ResolvedBy")
                .HasColumnType("int").IsOptional();
            modelBuilder.Entity<Complaint>()
                .Property(x => x.ResolvedDate)
                .HasColumnName(@"ResolvedDate")
                .HasColumnType("datetime").IsOptional();

            modelBuilder.Entity<ComplaintActivity>()
                .Property(e => e.SolutionDetails)
                .IsUnicode(false);

            //modelBuilder.Entity<ComplaintActivity>().
            //    Property(x => x.ResolvedBy).
            //    HasColumnName(@"ResolvedBy")
            //    .HasColumnType("int").IsOptional();
            //modelBuilder.Entity<ComplaintActivity>()
            //    .Property(x => x.ResolvedDate)
            //    .HasColumnName(@"ResolvedDate")
            //    .HasColumnType("datetime").IsOptional();

            modelBuilder.Entity<ComplaintStatus>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.ComplaintStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ComplaintType>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.ComplaintType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SolutionStatus>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.SolutionStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SolutionStatus>()
                .HasMany(e => e.ComplaintActivities)
                .WithRequired(e => e.SolutionStatus)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<SolutionType>()
            //    .HasMany(e => e.ComplaintActivities)
            //    .WithRequired(e => e.SolutionType)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<vComplaint>()
                .Property(e => e.Title)
                .IsFixedLength();

            modelBuilder.Entity<vComplaint>()
                .Property(e => e.Details)
                .IsUnicode(false);

            modelBuilder.Entity<vComplaint>()
                .Property(e => e.ComplaintType)
                .IsUnicode(false);

            modelBuilder.Entity<vComplaint>()
                .Property(e => e.SolutionStatus)
                .IsUnicode(false);

            modelBuilder.Entity<vComplaint>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<vComplaintActivity>()
                .Property(e => e.SolutionDetails)
                .IsUnicode(false);

            //modelBuilder.Entity<vComplaintActivity>()
            //    .Property(e => e.SolutionType)
            //    .IsUnicode(false);

            modelBuilder.Entity<vComplaintActivity>()
                .Property(e => e.SolutionStatus)
                .IsUnicode(false);

            modelBuilder.Entity<vComplaintActivity>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);
        }
    }
}
