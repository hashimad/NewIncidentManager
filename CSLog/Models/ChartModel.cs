using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLog.Models
{
    public class ChartModel
    {
        public List<string> data { get; set; }
        public List<Series> series { get; set; }

    }
}