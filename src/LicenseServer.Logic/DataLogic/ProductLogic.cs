using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class ProductLogic : BaseLogic
    {
        public List<Product> GetProducts()
        {
            var obj = new List<Product>();
            IEnumerable<Core.Model.Product> products = Work.ProductRepository.GetData(null, null, null);
            foreach (var pro in products)
                obj.Add(AutoMapper.Mapper.Map<Core.Model.Product, DataModel.Product>(pro));
            return obj;
        }

        public Product GetProductById(int id)
        {
            Core.Model.Product pro = Work.ProductRepository.GetData(f => f.Id == id, null, "Categories,AssociatedFeatures").FirstOrDefault();
            return AutoMapper.Mapper.Map<Core.Model.Product, DataModel.Product>(pro);
        }

        public List<Product> GetProductByCategoryId(int categoryId)
        {
            List<Product> products = new List<Product>();
            var data = Work.ProductCategoryRepository.GetData(p => p.Id == categoryId).FirstOrDefault();
            foreach (var pro in data.Products)
            {
                products.Add(AutoMapper.Mapper.Map<Product>(pro));
            }
            return products;
        }

        public bool CreateProduct(Product pro)
        {
            Core.Model.Product obj = AutoMapper.Mapper.Map<Product, Core.Model.Product>(pro);
            obj.Categories = new List<Core.Model.ProductCategory>();
            foreach (var c in pro.Categories)
            {
                var category = Work.ProductCategoryRepository.GetById(c.Id);
                obj.Categories.Add(category);
            }
            obj.AssociatedFeatures = new List<Core.Model.Feature>();
            foreach (var f in pro.AssociatedFeatures)
            {
                var feature = Work.FeaturesRepository.GetById(f.Id);
                obj.AssociatedFeatures.Add(feature);
            }
            obj = Work.ProductRepository.Create(obj);
            Work.ProductRepository.Save();
            return obj.Id > 0;
        }

        public bool UpdateProduct(int id, Product pro)
        {
            var obj = Work.ProductRepository.GetData(f => f.Id == id, null, "Categories,AssociatedFeatures").FirstOrDefault();
            obj.Name = pro.Name;
            obj.Description = pro.Description;
            obj.Price = pro.Price;
            if (obj.Categories.Count > 0)
            {
                var idList = obj.Categories.Select(s => s.Id).ToList();
                foreach (int catid in idList)
                {
                    if (!pro.Categories.Any(o => o.Id == catid))
                    {
                        var categoryObj = obj.Categories.FirstOrDefault(c => c.Id == catid);
                        obj.Categories.Remove(categoryObj);
                    }
                    else
                    {
                        var categoryObj = pro.Categories.FirstOrDefault(c => c.Id == catid);
                        pro.Categories.Remove(categoryObj);
                    }
                }
            }
            foreach (var c in pro.Categories)
            {
                var category = Work.ProductCategoryRepository.GetById(c.Id);
                obj.Categories.Add(category);
            }

            if(obj.AssociatedFeatures.Count > 0)
            {
                var idList = obj.AssociatedFeatures.Select(s => s.Id).ToList();
                foreach (int featureid in idList)
                {
                    if (!pro.AssociatedFeatures.Any(o => o.Id == featureid))
                    {
                        var featureObj = obj.AssociatedFeatures.FirstOrDefault(c => c.Id == featureid);
                        obj.AssociatedFeatures.Remove(featureObj);
                    }
                    else
                    {
                        var featureObj = pro.AssociatedFeatures.FirstOrDefault(c => c.Id == featureid);
                        pro.AssociatedFeatures.Remove(featureObj);
                    }
                }
                foreach (var f in pro.AssociatedFeatures)
                {
                    var feature = Work.FeaturesRepository.GetById(f.Id);
                    obj.AssociatedFeatures.Add(feature);
                }
            }
           
           
            obj = Work.ProductRepository.Update(obj);
            Work.ProductRepository.Save();
            return obj.Id > 0;
        }

        public bool DeleteProduct(int id)
        {
            var status = Work.ProductRepository.Delete(id);
            Work.ProductRepository.Save();
            return status;
        }
    }
}
