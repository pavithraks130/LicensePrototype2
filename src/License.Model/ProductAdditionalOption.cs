using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.DataModel
{
    public class ProductAdditionalOption
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int ProductId { get; set; }
    }
}
