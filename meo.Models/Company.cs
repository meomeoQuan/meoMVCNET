using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace meo.Models
{
    public class Company 
    {
        [Key]
        public int CompanyID { get; set; }
        [Required]
        public string ? CompanyName { get; set; }
        public string ? CompanyStreetAddress { get; set; }
        public string? CompanyCity { get; set; }
        public string ? CompanyState { get; set; }
        public string ? CompanyPostalCode { get; set; }

        [ValidateNever]
        public string ? companyImage {  get; set; }

    }
}
