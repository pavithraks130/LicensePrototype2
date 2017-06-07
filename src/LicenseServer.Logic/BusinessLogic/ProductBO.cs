using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic.BusinessLogic
{
    public class ProductBO
    {
        private FeaturesLogic featureLogic = null;
        private SubscriptionCategoryLogic productCategoryLogic = null;

        public ProductBO()
        {
            featureLogic = new FeaturesLogic();
            productCategoryLogic = new SubscriptionCategoryLogic();
        }

        public ProductDependency GetDependencyDetails()
        {
            ProductDependency dependency = new ProductDependency();
            dependency.Categories = productCategoryLogic.GetAll();
            dependency.Features = featureLogic.GetFeatureList();
            return dependency;
        }
    }
}
