using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace meo.Models
{
    public class Product
    {

        //Id = 1,
        //            Title = "Fortune of Time",
        //            Author = "Billy Spark",
        //            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
        //            ISBN = "SWD9999001",
        //            ListPrice
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
       

        [Required]
        [Range(1,1000)]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }


        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 1-50")]
        public double Price { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 50+")]
        public double Price50 { get; set; }


        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for 100+")]
        public double Price100 { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        [ValidateNever]
        public  Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
