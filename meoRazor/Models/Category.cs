using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace meoRazor.Models
{
    public class Category
    {
        [Key]
        public int categoryID { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("CategoryName")]
        public string categoryName { get; set; }
        [DisplayName("Order")]
        [Range(1, 100, ErrorMessage = "oder in 1 - 100")]
        public int categoryOrder { get; set; }
    }
}
