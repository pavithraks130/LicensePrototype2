using System.ComponentModel.DataAnnotations;

namespace License.MetCalWeb.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        //[Required, StringLength(10000), Display(Name = "Product Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Price")]
        public double? UnitPrice { get; set; }

        public int? CategoryID { get; set; }


    }
}