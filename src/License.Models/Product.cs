using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace License.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ICollection<SubscriptionCategory> Categories { get; set; }
        public ICollection<Feature> Features { get; set; }
        public List<ProductAdditionalOption> AdditionalOption { get; set; }
        public int AvailableCount { get; set; }
        [JsonIgnore]
        public bool IsLocal { get; set; } = false;
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public bool InitialState { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool ShouldSerializeQuantity()
        {
            return !IsLocal;
        }

        public bool ShouldSerializeCreatedDate()
        {
            return !IsLocal;
        }
        public bool ShouldSerializeModifiedDate()
        {
            return !IsLocal;
        }

        public bool ShouldSerializeAvailableCount()
        {
            return !IsLocal;
        }
        public bool ShouldSerializeIsSelected()
        {
            return !IsLocal;
        }
        public bool ShouldSerializeIsDisabled()
        {
            return !IsLocal;
        }
        public bool ShouldSerializeInitialState()
        {
            return !IsLocal;
        }

        public bool ShouldSerializeExpireDate()
        {
            return !IsLocal;
        }

    }
}
