using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSLog.Models
{
    public class NewComplaint
    {
        [Required]
        [Display(Name = "Service Type")]
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

        [Required]
        public string Details { get; set; }

        //[Required]
        //[Display(Name = "Status")]
        public int StatusId { get; set; }

        [StringLength(100)]  
        [Display(Name = "Customer Name")]
        public string ComplaintOwnerName { get; set; }

        [StringLength(50)]
        [Display(Name = "Customer Email")]
        public string ComplaintOwnerEmail { get; set; }

        //[Required]
        [StringLength(50)]
        public string Location { get; set; }


        //Complaint Activities
        [Display(Name = "Solution Type")]
        public int SolutionTypeId { get; set; }

        [Required]
        [Display(Name = "Solution Details")]
        public string SolutionDetails { get; set; }

        [Display(Name = "Solution Status")]
        public int SolutionStatusId { get; set; }
    }
}