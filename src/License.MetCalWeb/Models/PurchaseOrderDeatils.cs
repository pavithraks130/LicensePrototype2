using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class PurchaseOrderDeatils
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PurchaseOrderNo { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsSynched { get; set; }
        public string ApprovedBy { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }
        public double Total { get; set; }
        public ICollection<PurchaseOrderItemDeatils> OrderItems { get; set; }

        public PurchaseOrderDeatils()
        {
            CreatedDate = new DateTime(1900, 1, 1);
            UpdatedDate = new DateTime(1900, 1, 1);
        }
    }
}