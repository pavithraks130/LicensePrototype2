using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;

namespace LicenseServer.Logic
{
    public class ProductAdditionalOptionLogic : BaseLogic
    {
        /// <summary>
        /// Get the Addiotional option list by ProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Return List of additional Options</returns>
        public List<ProductAdditionalOption> GetByProductId(int productId)
        {
            List<ProductAdditionalOption> optionList = null;
            var dataList = Work.ProdAdditionalOptionRepository.GetData(o => o.ProductId == productId).ToList();
            if (dataList != null)
                optionList = dataList.Select(o => AutoMapper.Mapper.Map<ProductAdditionalOption>(o)).ToList();
            return optionList;
        }

        /// <summary>
        /// Create the Additional Option record for the product
        /// </summary>
        /// <param name="option"></param>
        /// <returns>returns a created option records</returns>
        public ProductAdditionalOption Create(ProductAdditionalOption option)
        {
            var optionData = AutoMapper.Mapper.Map<Core.Model.ProductAdditionalOption>(option);
            optionData = Work.ProdAdditionalOptionRepository.Create(optionData);
            Work.ProdAdditionalOptionRepository.Save();
            return AutoMapper.Mapper.Map<ProductAdditionalOption>(optionData);
        }

        /// <summary>
        /// Create the List of the Additional Option in Db for the product
        /// </summary>
        /// <param name="optionList"></param>
        /// <returns>returns the list of options for the product</returns>
        public List<ProductAdditionalOption> Create(List<ProductAdditionalOption> optionList)
        {
            List<ProductAdditionalOption> responseOption = new List<ProductAdditionalOption>();
            foreach (var opt in optionList)
            {
                var option = Create(opt);
                responseOption.Add(option);
            }
            return responseOption;
        }

        /// <summary>
        /// Update the  Additional OPtion based on the id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option"></param>
        /// <returns> return the updated objbect </returns>
        public ProductAdditionalOption Update(int id, ProductAdditionalOption option)
        {
            var optionData = Work.ProdAdditionalOptionRepository.GetById(id);
            optionData.Key = option.Key;
            optionData.Value = option.Value;
            optionData.ValueType = option.ValueType;
            optionData.ProductId = option.ProductId;
            optionData = Work.ProdAdditionalOptionRepository.Update(optionData);
            Work.ProdAdditionalOptionRepository.Save();
            return AutoMapper.Mapper.Map<ProductAdditionalOption>(optionData);
        }

        /// <summary>
        /// Update the list of Product AddionalOptions based on the id
        /// </summary>
        /// <param name="optionList"></param>
        /// <returns>returns updated list of Additional Options</returns>
        public List<ProductAdditionalOption> Update(List<ProductAdditionalOption> optionList)
        {
            List<ProductAdditionalOption> responseOption = new List<ProductAdditionalOption>();
            foreach (var opt in optionList)
            {
                var option = Update(opt.Id, opt);
                responseOption.Add(option);
            }
            return responseOption;
        }

        /// <summary>
        /// Delete the option record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns the object record which is deleted from Db.</returns>
        public ProductAdditionalOption Delete(int id)
        {
            var obj = Work.ProdAdditionalOptionRepository.GetById(id);
            Work.ProdAdditionalOptionRepository.Delete(id);
            Work.ProdAdditionalOptionRepository.Save();
            return AutoMapper.Mapper.Map<ProductAdditionalOption>(obj);
        }

        /// <summary>
        /// Delete the Additional option based on the Object 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProductAdditionalOption Delete(Core.Model.ProductAdditionalOption obj)
        {
            Work.ProdAdditionalOptionRepository.Delete(obj.Id);
            Work.ProdAdditionalOptionRepository.Save();
            return AutoMapper.Mapper.Map<ProductAdditionalOption>(obj);
        }

        /// <summary>
        /// Delete the option record by Product Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns the object record which is deleted from Db.</returns>
        public List<ProductAdditionalOption> DeleteByProductId(int productId)
        {
            List<ProductAdditionalOption> deletedObject = new List<ProductAdditionalOption>();
            var deleteObjects = Work.ProdAdditionalOptionRepository.GetData(o => o.ProductId == productId).ToList();
            deleteObjects.ForEach(o => {
                var response = Delete(o);
                deletedObject.Add(response);
            });
            return deletedObject;
        }


    }
}
