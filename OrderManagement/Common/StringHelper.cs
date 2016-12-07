using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagement.Common
{
    public static class StringHelper
    {
        public static bool IsNullOrEmpty(this string str)
        {
            if (str == null || str == string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}