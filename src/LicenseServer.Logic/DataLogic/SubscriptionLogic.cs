using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;
using LicenseServer.Logic.Common;

namespace LicenseServer.Logic
{
    public class SubscriptionLogic : BaseLogic
    {
        public List<Subscription> GetSubscription(string userId = "")
        {
            List<Subscription> subscriptionTypes = new List<Subscription>();
            List<Core.Model.Subscription> listSubscription = null;
            listSubscription = Work.SubscriptionRepository.GetData(s => String.IsNullOrEmpty(s.CreatedBy) == true || s.CreatedBy == userId).ToList();
            subscriptionTypes = listSubscription.Select(obj => AutoMapper.Mapper.Map<DataModel.Subscription>(obj)).ToList();
            return subscriptionTypes;
        }

        public Subscription CreateSubscription(Subscription subType)
        {
            var coreSubType = AutoMapper.Mapper.Map<Core.Model.Subscription>(subType);
            coreSubType = Work.SubscriptionRepository.Create(coreSubType);
            Work.SubscriptionRepository.Save();
            return AutoMapper.Mapper.Map<Subscription>(coreSubType);
        }

        /// <summary>
        /// Function will be used if the Subscription data provided with Product details.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Subscription CreateSubscriptionWithProduct(Subscription type)
        {
            var coreSubscription = AutoMapper.Mapper.Map<Core.Model.Subscription>(type);
            var categoryObj = Work.SubscriptionCategoryRepo.GetById(type.Category.Id);
            coreSubscription.Category = categoryObj;
            coreSubscription = Work.SubscriptionRepository.Create(coreSubscription);
            Work.SubscriptionRepository.Save();
            if (coreSubscription.Id > 0 && type.Products.Count() > 0)
            {
                int i = 0;
                foreach (var pro in type.Products)
                {
                    int productId = pro.Id;
                    if (pro.Id == 0)
                    {
                        var featureId = pro.Features.Select(s => s.Id).ToList();
                        var featuresList = Work.FeaturesRepository.GetData(f => featureId.Contains(f.Id)).ToList();
                        foreach (var proObj in categoryObj.Products)
                        {
                            var status = Helpers.CompareList<Core.Model.Feature>(proObj.Features.ToList(), featuresList);
                            if (status)
                            {
                                productId = proObj.Id;
                                break;
                            }
                        }

                    }
                    Core.Model.SubscriptionDetail detail = new Core.Model.SubscriptionDetail()
                    {
                        SubscriptionId = coreSubscription.Id,
                        ProductId = productId,
                        Quantity = pro.Quantity
                    };
                    Work.SubscriptionDetailResitory.Create(detail);
                    i++;
                }
                if (i > 0)
                    Work.SubscriptionDetailResitory.Save();
            }
            return AutoMapper.Mapper.Map<DataModel.Subscription>(coreSubscription);
        }

        public Subscription GetById(int id)
        {
            var data = Work.SubscriptionRepository.GetById(id);
            return AutoMapper.Mapper.Map<Subscription>(data);
        }
    }

}
