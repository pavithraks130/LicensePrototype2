using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.Logic.Common
{
    public static class Helpers
    {
        public static bool CompareList<T>(List<T> list1, List<T> list2)
        {
            bool flag = true;
            if (list1.Count != list2.Count)
                return false;
            foreach (T obj in list1)
            {
                if (!obj.Equals(list2[list1.IndexOf(obj)]))
                {
                    flag = false;
                }
            }

            return flag;
        }


    }
}
