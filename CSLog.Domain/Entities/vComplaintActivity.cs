namespace CSLog.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vComplaintActivity")]
    public partial class vComplaintActivity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ComplaintId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ComplaintActivityId { get; set; }

        //[Key]
        //[Column(Order = 2)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int SolutionTypeId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "text")]
        public string SolutionDetails { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolutionStatusId { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecordedBy { get; set; }
        [Key]
        [Column(Order = 5)]
        public DateTime Date { get; set; }

     

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string SolutionStatus { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(256)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string DisplayName { get; set; }
    }
}
