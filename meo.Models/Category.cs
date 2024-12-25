using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace meo.Models
{
    public class Category 
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("CategoryName")]
        public  string CategoryName { get; set; }
        [DisplayName("Order")]
        [Range(1,100,ErrorMessage = "oder in 1 - 100")]
        public int CategoryOrder { get; set; }
    }
}
