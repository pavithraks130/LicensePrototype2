using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.Logic.ServiceLogic
{
    public class ProductLogic : BaseLogic
    {
        public List<Product> GetProducts()
        {
            var obj = new List<Product>();
            IEnumerable<Core.Model.Product> products = Work.ProductLicenseRepository.GetData();
            foreach (var pro in products)
                obj.Add(AutoMapper.Mapper.Map<Core.Model.Product, Model.Product>(pro));
            return obj;
        }

        public Product GetProductById(int id)
        {
            Core.Model.Product pro = Work.ProductLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.Product, Model.Product>(pro);
        }


        public bool CreateProduct(Product pro)
        {
            Core.Model.Product obj = AutoMapper.Mapper.Map<Product, Core.Model.Product>(pro);
            obj = Work.ProductLicenseRepository.Create(obj);
            Work.ProductLicenseRepository.Save();
            return obj.Id > 0;
        }

        public void CreateProduct(List<Product> prod)
        {
            foreach (var pro in prod)
            {
                var obj = Work.ProductLicenseRepository.GetById(pro.Id);
                if (obj == null)
                    CreateProduct(pro);
            }
        }
    }
}
