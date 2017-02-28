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
    }
    /// <summary>
    /// This class holds credit card details 
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CardDetails()
        {

        }
        /// <summary>
        /// Credit card holder name 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Credit card holder Number
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Credit card holder Expiry month
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// Credit card expiry Year
        /// </summary>
        public short Years { get; set; }
        /// <summary>
        /// Credit card CVV number
        /// </summary>
        public short CVVNum { get; set; }
    }
}