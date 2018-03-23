using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealState.Areas.Admin.Models
{
    public class AccountModel
    {
        public AccountModel()
        {
            resultModel = new ResultModel();

        }
        public long UserID { get; set; }
        //[Required(ErrorMessage = "Please Enter First Name")]
        //[Display(Name = "FirstName")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",ErrorMessage = "Please Enter Correct Email Address")]
        public string Email { get; set; }
        public string password { get; set; }
        public bool Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Role { get; set; }
        //[Required(ErrorMessage = "Please Enter Mobile No")]
        //[Display(Name = "Mobile")]
        //[StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)]
        public string MobileNo { get; set; }
        public ResultModel resultModel { get; set; }
    }
}

