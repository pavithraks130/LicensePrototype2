﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class ProductLogic :BaseLogic
    {
        public List<Product> GetProducts()
        {
            var obj = new List<Product>();
            IEnumerable<Core.Model.Product> products = Work.ProductRepository.GetData();
            foreach (var pro in products)
                obj.Add(AutoMapper.Mapper.Map<Core.Model.Product,DataModel.Product>(pro));
            return obj;
        }

        public Product GetProductById(int id)
        {
            Core.Model.Product pro = Work.ProductRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.Product, DataModel.Product>(pro);
        }


        public bool CreateProduct(Product pro)
        {
            Core.Model.Product obj = AutoMapper.Mapper.Map<Product, Core.Model.Product>(pro);
            obj = Work.ProductRepository.Create(obj);
            Work.ProductRepository.Save();
            return obj.Id > 0;
        }
    }
}