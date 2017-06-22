using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
namespace LicenseServer.Logic.BusinessLogic
{
    public class ProductBO
    {
        private FeaturesLogic featureLogic = null;
        private SubscriptionCategoryLogic productCategoryLogic = null;
        private ProductLogic proLogic = null;
        private ProductAdditionalOptionLogic proOptionLogic = null;
        public ProductBO()
        {
            featureLogic = new FeaturesLogic();
            productCategoryLogic = new SubscriptionCategoryLogic();
            proLogic = new ProductLogic();
            proOptionLogic = new ProductAdditionalOptionLogic();
        }

        /// <summary>
        /// Get Category and Feature List based  for the Product Creation
        /// </summary>
        /// <returns></returns>
        public ProductDependency GetDependencyDetails()
        {
            ProductDependency dependency = new ProductDependency();
            dependency.Categories = productCategoryLogic.GetAll();
            dependency.Features = featureLogic.GetFeatureList();
            return dependency;
        }

        /// <summary>
        /// Create Product with Options
        /// </summary>
        /// <param name="pro"></param>
        /// <returns>returns the Product object</returns>
        public Product Creat(Product pro)
        {
            var dbPro = proLogic.CreateProduct(pro);
            pro.AdditionalOption.ToList().ForEach(o => o.ProductId = dbPro.Id);
            if (pro.AdditionalOption != null || pro.AdditionalOption.Count != 0)
            {
                proOptionLogic.DeleteByProductId(dbPro.Id);
                dbPro.AdditionalOption = proOptionLogic.Create(pro.AdditionalOption.ToList());
            }
            return dbPro;
        }

        /// <summary>
        /// Update Product based on the ID. Here the existin Options deleted and New options will be updated for the product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pro"></param>
        /// <returns>Returns the Updated Product</returns>
        public Product Update(int id, Product pro)
        {
            var dbPro = proLogic.UpdateProduct(id, pro);
            pro.AdditionalOption.ToList().ForEach(o => o.ProductId = dbPro.Id);
            if (pro.AdditionalOption != null || pro.AdditionalOption.Count != 0)
            {
                proOptionLogic.DeleteByProductId(dbPro.Id);
                dbPro.AdditionalOption = proOptionLogic.Create(pro.AdditionalOption.ToList());
            }
            return dbPro;
        }
    }
}
