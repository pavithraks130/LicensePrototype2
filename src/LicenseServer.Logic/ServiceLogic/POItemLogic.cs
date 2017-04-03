using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;

namespace LicenseServer.Logic
{
    public class POItemLogic : BaseLogic
    {
        public bool CreateItem(PurchaseOrderItem item)
        {
            var itemObj = AutoMapper.Mapper.Map<Core.Model.PurchaseOrderItem>(item);
            var obj = Work.POItemRepository.Create(itemObj);
            Work.POItemRepository.Save();
            return obj.Id > 0;
        }

        public bool CreateItem(List<PurchaseOrderItem> itemlist, int poId)
        {
            int i = 0;
            foreach (var item in itemlist)
            {
                var itemObj = AutoMapper.Mapper.Map<Core.Model.PurchaseOrderItem>(item);
                itemObj.PurchaseOrderId = poId;
                var obj = Work.POItemRepository.Create(itemObj);
                i++;
            }
            if (i > 0)
                Work.POItemRepository.Save();
            return true;
        }

        public List<PurchaseOrderItem> GetItemByPO(int poId)
        {
            List<PurchaseOrderItem> items = new List<PurchaseOrderItem>();
            var itemList = Work.POItemRepository.GetData(f => f.PurchaseOrderId == poId);
            foreach (var obj in itemList)
            {
                var item = AutoMapper.Mapper.Map<DataModel.PurchaseOrderItem>(obj);
                items.Add(item);
            }
            return items;
        }

        public bool DeletePOItem(int poId)
        {
            var itemList = Work.POItemRepository.GetData(f => f.PurchaseOrderId == poId);
            int i = 0;
            foreach (var obj in itemList)
            {
                Work.POItemRepository.Delete(obj);
                i++;
            }
            if (i > 0)
                Work.POItemRepository.Save();
            return true;
        }
    }
}
