using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LicenseServer.Core.Model
{
    public class ProductAdditionalOption
    {
        public int Id { get; set; }
        public string Key { get; set; }
        [Column(TypeName = "Text")]
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
