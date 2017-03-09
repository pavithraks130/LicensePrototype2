using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace License.Core.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
    }

    public class ProductSubscriptionMapping
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SubscriptionId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("SubscriptionId")]
        public Subscription Subscription { get; set; }
    }
}