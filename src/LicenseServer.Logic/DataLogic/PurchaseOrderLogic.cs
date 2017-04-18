using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;


namespace LicenseServer.Logic
{
    public class PurchaseOrderLogic : BaseLogic
    {
        public PurchaseOrder CreatePurchaseOrder(PurchaseOrder order)
        {
            var tempObj = AutoMapper.Mapper.Map<Core.Model.PurchaseOrder>(order);
            tempObj.PurchaseOrderNo = CreatePONumber();
            tempObj = Work.PurchaseOrderRepository.Create(tempObj);
            Work.PurchaseOrderRepository.Save();
            if (tempObj.Id > 0)
                return AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(tempObj);
            return null;
        }

        public PurchaseOrder UpdatePurchaseOrder(int id,PurchaseOrder order)
        {
            var tempObj = Work.PurchaseOrderRepository.GetById(id);
            tempObj.IsApproved = order.IsApproved;
            tempObj.ApprovedBy = order.ApprovedBy;
            tempObj.IsSynched = order.IsSynched;
            tempObj.UpdatedDate = DateTime.Now.Date;
            tempObj.Comment = order.Comment;
            tempObj = Work.PurchaseOrderRepository.Update(tempObj);
            Work.PurchaseOrderRepository.Save();
            return AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(tempObj);
        }

        public void UpdatePurchaseOrder(List<PurchaseOrder> orderList)
        {
            int i = 0;
            foreach (var order in orderList)
            {
                var tempObj = Work.PurchaseOrderRepository.GetById(order.Id);
                tempObj.IsApproved = order.IsApproved;
                tempObj.ApprovedBy = order.ApprovedBy;
                tempObj.Comment = order.Comment;
                tempObj = Work.PurchaseOrderRepository.Update(tempObj);
                i++;
            }
            if (i > 0)
                Work.PurchaseOrderRepository.Save();
        }

        public List<PurchaseOrder> GetPurchaseOrderByIds(List<int> poIdList)
        {
            List<DataModel.PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            var listItem = Work.PurchaseOrderRepository.GetData(f => poIdList.Contains(f.Id));
            foreach (var item in listItem)
            {
                var obj = AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(item);
                purchaseOrderList.Add(obj);
            }
            return purchaseOrderList;
        }

        public List<PurchaseOrder> GetAllPendingPurchaseOrder()
        {
            List<DataModel.PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            var listItem = Work.PurchaseOrderRepository.GetData(f => f.IsApproved == false && string.IsNullOrEmpty(f.ApprovedBy));
            foreach (var item in listItem)
            {
                var obj = AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(item);
                purchaseOrderList.Add(obj);
            }
            return purchaseOrderList;
        }

        public List<PurchaseOrder> GetPurchaseOrderByUser(string userId)
        {
            List<DataModel.PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            var listItem = Work.PurchaseOrderRepository.GetData(f => f.UserId == userId);
            foreach (var item in listItem)
            {
                var obj = AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(item);
                purchaseOrderList.Add(obj);
            }
            return purchaseOrderList;
        }

        public List<PurchaseOrder> GetPOToBeSynchedByUser(String userId)
        {
            List<DataModel.PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            var listItem = Work.PurchaseOrderRepository.GetData(f => f.UserId == userId && f.IsApproved == true && f.IsSynched == false);
            foreach (var item in listItem)
            {
                var obj = AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(item);
                purchaseOrderList.Add(obj);
            }
            return purchaseOrderList;
        }

        public bool DeletePurchaseOrder(int id)
        {
            POItemLogic itemLogic = new POItemLogic();
            itemLogic.DeletePOItem(id);
            var obj = Work.PurchaseOrderRepository.Delete(id);
            Work.PurchaseOrderRepository.Save();
            return obj;
        }

        public String CreatePONumber()
        {
            var listData = Work.PurchaseOrderRepository.GetData().ToList();
            if (listData.Count > 0)
            {
                var POId = listData[listData.Count - 1].PurchaseOrderNo;
                var dt = POId.Split(new char[] { '-' });
                int data = Convert.ToInt32(dt[2]) + 1;
                return "PO-SUB-" + data.ToString();
            }
            else
                return "PO-SUB-00001";
        }

        public PurchaseOrder GetPurchaseOrderById(int id)
        {
            var order = Work.PurchaseOrderRepository.GetById(id);
            return AutoMapper.Mapper.Map<DataModel.PurchaseOrder>(order);

        }
    }
}
