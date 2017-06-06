using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class ProductCategoryLogic : BaseLogic
    {
        public List<SubscriptionCategory> GetAll()
        {
            List<SubscriptionCategory> productCategory = new List<SubscriptionCategory>();
            var categoryList = Work.SubscriptionCategoryRepo.GetData().ToList();
            foreach (var category in categoryList)
                productCategory.Add(AutoMapper.Mapper.Map<SubscriptionCategory>(category));
            return productCategory;
        }

        public SubscriptionCategory GetById(int id)
        {
            var obj = Work.SubscriptionCategoryRepo.GetById(id);
            return AutoMapper.Mapper.Map<SubscriptionCategory>(obj);
        }

        public SubscriptionCategory Create(SubscriptionCategory obj)
        {
            var categoryObj = AutoMapper.Mapper.Map<LicenseServer.Core.Model.SubscriptionCategory>(obj);
            categoryObj = Work.SubscriptionCategoryRepo.Create(categoryObj);
            Work.SubscriptionCategoryRepo.Save();
            return AutoMapper.Mapper.Map<SubscriptionCategory>(categoryObj);
        }

        public SubscriptionCategory Update(int id, SubscriptionCategory obj)
        {
            var categoryObj = Work.SubscriptionCategoryRepo.GetById(id);
            categoryObj.Name = obj.Name;
            categoryObj.Description = obj.Description;
            categoryObj = Work.SubscriptionCategoryRepo.Update(categoryObj);
            Work.SubscriptionCategoryRepo.Save();
            return AutoMapper.Mapper.Map<SubscriptionCategory>(categoryObj);
        }

        public bool Delete(int id)
        {
            var status = Work.SubscriptionCategoryRepo.Delete(id);
            Work.SubscriptionCategoryRepo.Save();
            return status;
        }

    }
}
