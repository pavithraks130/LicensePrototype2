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
        public List<ProductCategory> GetAll()
        {
            List<ProductCategory> productCategory = new List<ProductCategory>();
            var categoryList = Work.ProductCategoryRepository.GetData().ToList();
            foreach (var category in categoryList)
                productCategory.Add(AutoMapper.Mapper.Map<ProductCategory>(category));
            return productCategory;
        }

        public ProductCategory GetById(int id)
        {
            var obj = Work.ProductCategoryRepository.GetById(id);
            return AutoMapper.Mapper.Map<ProductCategory>(obj);
        }

        public bool Create(ProductCategory obj)
        {
            var categoryObj = AutoMapper.Mapper.Map<LicenseServer.Core.Model.ProductCategory>(obj);
            categoryObj = Work.ProductCategoryRepository.Create(categoryObj);
            Work.ProductCategoryRepository.Save();
            return categoryObj.Id > 0;
        }

        public bool Update(int id, ProductCategory obj)
        {
            var categoryObj = Work.ProductCategoryRepository.GetById(id);
            categoryObj.Name = obj.Name;
            categoryObj.Description = obj.Description;
            categoryObj = Work.ProductCategoryRepository.Update(categoryObj);
            Work.ProductCategoryRepository.Save();
            return categoryObj.Id > 0;
        }

        public bool Delete(int id)
        {
            var status = Work.ProductCategoryRepository.Delete(id);
            Work.ProductCategoryRepository.Save();
            return status;
        }

    }
}
