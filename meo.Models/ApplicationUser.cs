using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace meo.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string name { get; set; }

        public string ? streetAddress { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? portalCode { get; set; }

        public string? userImage { get; set; }


        public int ? CompanyID { get; set; }
        // nulable companyID is really important here because it is possible that a user may not have a company
        [ForeignKey("CompanyID")]
        [ValidateNever]
        public Company? Company { get; set; }

        [NotMapped]
        public string Role { get; set; }

    }
}
