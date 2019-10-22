using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLog.Models
{
    public class DashboardViewModel
    {
        public int TotalComplaint { get; set; }
        public int TotalResolved { get; set; }
        public int TotalUnresolved { get; set; }
        public int TotalClosed { get; set; }
    }
}