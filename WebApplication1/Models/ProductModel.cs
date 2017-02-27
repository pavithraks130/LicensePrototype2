using System.ComponentModel.DataAnnotations;

namespace License.MetCalWeb.Models
{
    public class Product
    {

        public Model.Model.Product ModelProduct;

        public Product()
        {
            ModelProduct = new Model.Model.Product();
        }

        public int ProductID
        {
            get
            {
                return ModelProduct.Id;
            }
        }

        public string ProductName { get { return ModelProduct.Name; } set { ModelProduct.Name = value; } }

        public string Description { get { return ModelProduct.Description; } set { ModelProduct.Description = value; } }

        public string ImagePath { get { return ModelProduct.ImagePath; } set { ModelProduct.ImagePath = value; } }

        [Display(Name = "Price")]
        public double? UnitPrice { get { return ModelProduct.UnitPrice; } set { ModelProduct.UnitPrice = value; } }

    }
}