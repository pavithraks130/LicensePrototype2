using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;


namespace License.Logic.DataLogic
{
    /// <summary>
    /// History
    /// Creator: 
    /// Created Date:
    /// Purpose : Contains the DB functionality related to Product License 
    ///         1. Communcating directly with DB 
    ///         2. Performing CRUD functionality on the data in DB for the Product License Table
    /// </summary>    

    public class ProductLicenseLogic : BaseLogic
    {
        /// <summary>
        /// Gets the License keys based on the usersubscription Id
        /// </summary>
        /// <param name="userSubscriptionId"></param>
        /// <returns></returns>
        public List<ProductLicense> GetLicenseList(int userSubscriptionId)
        {
            List<ProductLicense> dataList = new List<ProductLicense>();
            var list = Work.ProductLicenseRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId);
            dataList = list.Select(l => AutoMapper.Mapper.Map<ProductLicense>(l)).ToList();
            return dataList;
        }

        /// <summary>
        /// Gets the unassigned product License keys based on the product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductLicense GetUnassignedLicense(int productId)
        {
            var obj = Work.ProductLicenseRepository.GetData(f => f.ProductId == productId && f.IsMapped == false).FirstOrDefault();
            return AutoMapper.Mapper.Map<ProductLicense>(obj);
        }

        /// <summary>
        /// Updating the Available status for the existing license Id. 
        /// </summary>
        /// <param name="licId"></param>
        /// <param name="status"></param>
        public void UpdateLicenseStatus(int licId, bool status)
        {
            var obj = Work.ProductLicenseRepository.GetById(licId);
            obj.IsMapped = status;
            Work.ProductLicenseRepository.Update(obj);
            Work.ProductLicenseRepository.Save();
        }

        /// <summary>
        /// Creating Product License record in db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ProductLicense CreateLicenseData(ProductLicense data)
        {
            var obj = AutoMapper.Mapper.Map<License.Core.Model.ProductLicense>(data);
            obj = Work.ProductLicenseRepository.Create(obj);
            Work.ProductLicenseRepository.Save();
            return AutoMapper.Mapper.Map<ProductLicense>(obj);
        }

        /// <summary>
        /// Creating the Bulk Product License in DB 
        /// </summary>
        /// <param name="dataList"></param>
        public void CreateLicenseData(List<ProductLicense> dataList)
        {
            foreach (var data in dataList)
                CreateLicenseData(data);
        }


        /// <summary>
        /// Get Product License based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductLicense GetLicenseById(int id)
        {
            var obj = Work.ProductLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<ProductLicense>(obj);
        }

        /// <summary>
        /// Updating the License Keys with new set of license Keys after Renewel of the subscription
        /// </summary>
        /// <param name="licKeysMapping"></param>
        /// <param name="userSubscriptionId"></param>
        public void UpdateRenewalLicenseKeys(List<LicenseKeyProductMapping> licKeysMapping, int userSubscriptionId)
        {
            // Get existing license Key for the User Subscription and get the Product Ids.
            var licenseDatalist = Work.ProductLicenseRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId);
            var productIdList = licKeysMapping.Select(s => s.ProductId).Distinct();

            // based on the product Ids update the License Keys
            foreach (var pro in productIdList)
            {
                int i = 0;

                // gets the key from both existing license keys and new Llicense keys based on the Product Id.
                // if the existing subscription does not have the license Keys for the product or it has less number of License keys then new keys will be created.
                // else existing keys will be updated.

                var renewalKeys = licKeysMapping.Where(l => l.ProductId == pro).ToList();
                var existingKeys = licenseDatalist.Where(l => l.ProductId == pro).ToList();
                foreach (var keys in renewalKeys)
                {
                    if (i < existingKeys.Count)
                    {
                        existingKeys[i].LicenseKey = keys.LicenseKey;
                        Work.ProductLicenseRepository.Update(existingKeys[i]);
                    }
                    else
                    {
                        Core.Model.ProductLicense licenseData = new Core.Model.ProductLicense()
                        {
                            LicenseKey = keys.LicenseKey,
                            ProductId = keys.ProductId,
                            UserSubscriptionId = userSubscriptionId
                        };
                        Work.ProductLicenseRepository.Create(licenseData);
                    }
                    i++;
                }
                if (i > 0)
                    Work.ProductLicenseRepository.Save();

            }


        }

        /// <summary>
        /// Get the Available License Count based on the product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetAvailableLicenseCountByProduct(int productId)
        {
            var licList = Work.ProductLicenseRepository.GetData(l => l.ProductId == productId && l.IsMapped == false).ToList();
            if (licList != null && licList.Count != 0)
                return licList.Count;
            else
                return 0;
        }

        public List<Core.Model.ProductLicense> GetLicenseData()
        {
            return Work.ProductLicenseRepository.GetData().ToList();
        }

    }
}
