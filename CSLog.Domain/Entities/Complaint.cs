namespace CSLog.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Complaint")]
    public partial class Complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Complaint()
        {
            ComplaintActivities = new HashSet<ComplaintActivity>();
        }
        [Key]
        public int ComplaintId { get; set; }

        [Required]
        [Display(Name ="Service Type")]
        public int ComplaintTypeId { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Complaint Title")]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Details { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Solution Status")]
        public int SolutionStatusId { get; set; }

        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string ComplaintOwnerName { get; set; }

        [StringLength(50)]
        [Display(Name = "Customer Email")]
        public string ComplaintOwnerEmail { get; set; }

        //[Required]
        [StringLength(50)]
        public string Location { get; set; }

        [Display(Name = "Recorded By")]
        public int RegisteredBy { get; set; }

        [Display(Name = "Resolved By")]
        public int? ResolvedBy { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime Date { get; set; }
      

        public virtual ComplaintStatus ComplaintStatus { get; set; }

        public virtual ComplaintType ComplaintType { get; set; }

        public virtual SolutionStatus SolutionStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplaintActivity> ComplaintActivities { get; set; }
    }
}
