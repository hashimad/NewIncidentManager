namespace CSLog.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vComplaint")]
    public partial class vComplaint
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ComplaintId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Service Type")]
        public int ComplaintTypeId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Code { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(11)]
        [Display(Name ="Mobile Number")]
        public string MobileNo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(200)]
        public string Title { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "text")]
        public string Details { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StatusId { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolutionStatusId { get; set; }

        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string ComplaintOwnerName { get; set; }

        [StringLength(50)]
        [Display(Name = "Customer Email")]
        public string ComplaintOwnerEmail { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RegisteredBy { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? ResolvedBy { get; set; }

        public DateTime? ResolvedDate { get; set; }

        [Key]
        [Column(Order = 9)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        [Display(Name = "Complaint Type")]
        public string ComplaintType { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string ComplaintStatus { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(50)]
        [Display(Name = "Solution Status")]
        public string SolutionStatus { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(256)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(50)]
        public string DisplayName { get; set; }
        public string SupportUser { get; set; }
    }
}
