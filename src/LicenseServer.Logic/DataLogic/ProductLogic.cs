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
            var products = new List<Product>();
            IEnumerable<Core.Model.Product> data = Work.ProductRepository.GetData();
            products = data.Select(p => AutoMapper.Mapper.Map<Product>(p)).ToList();
            return products;
        }

        public Product GetProductById(int id)
        {
            Core.Model.Product pro = Work.ProductRepository.GetData(f => f.Id == id, null, "Categories,Features").FirstOrDefault();
            return AutoMapper.Mapper.Map<DataModel.Product>(pro);
        }

        public List<Product> GetProductByCategoryId(int categoryId)
        {
            List<Product> products = new List<Product>();
            var data = Work.SubscriptionCategoryRepo.GetById(categoryId);
            products = data.Products.Select(p => AutoMapper.Mapper.Map<Product>(p)).ToList();
            return products;
        }

        public Product CreateProduct(Product pro)
        {
            Core.Model.Product obj = AutoMapper.Mapper.Map<Product, Core.Model.Product>(pro);
            obj.Categories = new List<Core.Model.SubscriptionCategory>();
            obj.Categories = pro.Categories.Select(c => Work.SubscriptionCategoryRepo.GetById(c.Id)).ToList();
            obj.Features = pro.Features.Select(f => Work.FeaturesRepository.GetById(f.Id)).ToList();           
            obj = Work.ProductRepository.Create(obj);
            Work.ProductRepository.Save();
            return AutoMapper.Mapper.Map<Product>(obj);
        }

        public Product UpdateProduct(int id, Product pro)
        {
            var obj = Work.ProductRepository.GetData(f => f.Id == id, null, "Categories,Features").FirstOrDefault();
            obj.Name = pro.Name;
            obj.Description = pro.Description;
            obj.Price = pro.Price;
            obj.ProductCode = pro.ProductCode;
            obj.ModifiedDate = pro.ModifiedDate;
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
                var category = Work.SubscriptionCategoryRepo.GetById(c.Id);
                obj.Categories.Add(category);
            }

            if (obj.Features.Count > 0)
            {
                var idList = obj.Features.Select(s => s.Id).ToList();
                foreach (int featureid in idList)
                {
                    if (!pro.Features.Any(o => o.Id == featureid))
                    {
                        var featureObj = obj.Features.FirstOrDefault(c => c.Id == featureid);
                        obj.Features.Remove(featureObj);
                    }
                    else
                    {
                        var featureObj = pro.Features.FirstOrDefault(c => c.Id == featureid);
                        pro.Features.Remove(featureObj);
                    }
                }
                foreach (var f in pro.Features)
                {
                    var feature = Work.FeaturesRepository.GetById(f.Id);
                    obj.Features.Add(feature);
                }
            }


            obj = Work.ProductRepository.Update(obj);
            Work.ProductRepository.Save();
            return AutoMapper.Mapper.Map<Product>(obj);
        }

        public List<Product> GetCMMSProducts()
        {
            List<Product> products = new List<Product>();
            var proList = Work.ProductRepository.GetData(p => p.Categories.Count == 0).ToList();
            products = proList.Select(p => AutoMapper.Mapper.Map<Product>(p)).ToList();
            return products;
        }

        public bool DeleteProduct(int id)
        {
            var status = Work.ProductRepository.Delete(id);
            Work.ProductRepository.Save();
            return status;
        }

        public List<Product> GetProductUpdatesByProductId(List<Product> products)
        {
            List<Product> productList = new List<Product>();
            var proList = Work.ProductRepository.GetData().ToList();
            proList = proList.Where(p => products.Any(tp => p.Id == tp.Id && p.ModifiedDate > tp.ModifiedDate)).ToList();
            foreach (var pro in proList)
            {
                var proObj = AutoMapper.Mapper.Map<Product>(pro);
                proObj.Quantity = products.FirstOrDefault(p => p.Id == proObj.Id).Quantity;
                productList.Add(proObj);
            }
            return productList;
        }

    }
}
