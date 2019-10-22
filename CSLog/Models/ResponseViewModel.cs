using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLog.Models
{
    public class ResponseViewModel
    {
        public bool IsSuccessful { get; set; } = false;
        public string Msg { get; set; }
    }
}