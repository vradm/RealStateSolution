using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealState.Areas.Admin.Models
{
    public class ResultModel
    {

        public long Id { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Status { get; set; }
        public string RedirectUrl { get; set; }
    }
}