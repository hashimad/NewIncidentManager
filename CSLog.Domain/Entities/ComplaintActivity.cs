namespace CSLog.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComplaintActivity")]
    public partial class ComplaintActivity
    {
        public int ComplaintActivityId { get; set; }
        [Required]
        public int ComplaintId { get; set; }

        //[Display(Name = "Solution Type")]
        //public int SolutionTypeId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [Display(Name = "Solution Details")]
        public string SolutionDetails { get; set; }

        [Display(Name = "Solution Status")]
        public int SolutionStatusId { get; set; }

        [Display(Name = "Recorded By")]
        public int RecordedBy { get; set; }


        public DateTime Date { get; set; }

        public virtual Complaint Complaint { get; set; }

        public virtual SolutionStatus SolutionStatus { get; set; }

        //public virtual SolutionType SolutionType { get; set; }
    }
}
