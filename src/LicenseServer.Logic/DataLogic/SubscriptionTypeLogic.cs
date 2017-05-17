using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;
using LicenseServer.Logic.Common;

namespace LicenseServer.Logic
{
    public class SubscriptionTypeLogic : BaseLogic
    {
        public List<SubscriptionType> GetSubscriptionType(string userId = "")
        {
            List<SubscriptionType> subscriptionTypes = new List<SubscriptionType>();
            List<Core.Model.SubscriptionType> listSubscription = null;
            listSubscription = Work.SubscriptionRepository.GetData(s => String.IsNullOrEmpty(s.CreatedBy) == true || s.CreatedBy == userId).ToList();
            foreach (var obj in listSubscription)
            {
                subscriptionTypes.Add(AutoMapper.Mapper.Map<Core.Model.SubscriptionType, DataModel.SubscriptionType>(obj));
            }

            return subscriptionTypes;
        }

        public bool CreateSubscription(SubscriptionType subType)
        {
            var coreSubType = AutoMapper.Mapper.Map<DataModel.SubscriptionType, Core.Model.SubscriptionType>(subType);
            coreSubType = Work.SubscriptionRepository.Create(coreSubType);
            Work.SubscriptionRepository.Save();
            return coreSubType.Id > 0;
        }

        /// <summary>
        /// Function will be used if the Subscription data provided with Product details.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SubscriptionType CreateSubscriptionWithProduct(SubscriptionType type)
        {
            var coreSubscriptionType = AutoMapper.Mapper.Map<Core.Model.SubscriptionType>(type);
            var categoryObj = Work.ProductCategoryRepository.GetById(type.Category.Id);
            coreSubscriptionType.Category = categoryObj;
            coreSubscriptionType = Work.SubscriptionRepository.Create(coreSubscriptionType);
            Work.SubscriptionRepository.Save();
            if (coreSubscriptionType.Id > 0 && type.Products.Count() > 0)
            {
                int i = 0;
                foreach (var pro in type.Products)
                {
                    int productId = pro.Id;
                    if (pro.Id == 0)
                    {
                        var featureId = pro.AssociatedFeatures.Select(s => s.Id).ToList();
                        var featuresList = Work.FeaturesRepository.GetData(f => featureId.Contains(f.Id)).ToList();
                        foreach (var proObj in categoryObj.Products)
                        {
                            var status = Helpers.CompareList<Core.Model.Feature>(proObj.AssociatedFeatures.ToList(), featuresList);
                            if (status)
                            {
                                productId = proObj.Id;
                                break;
                            }
                        }

                    }
                    Core.Model.SubscriptionDetail detail = new Core.Model.SubscriptionDetail()
                    {
                        SubscriptionTypeId = coreSubscriptionType.Id,
                        ProductId = productId,
                        Quantity = pro.Quantity
                    };
                    Work.SubscriptionDetailResitory.Create(detail);
                    i++;
                }
                if (i > 0)
                    Work.SubscriptionDetailResitory.Save();
            }
            return AutoMapper.Mapper.Map<DataModel.SubscriptionType>(coreSubscriptionType);
        }

        public SubscriptionType GetById(int id)
        {
            var data = Work.SubscriptionRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.SubscriptionType, SubscriptionType>(data);
        }
    }

}
