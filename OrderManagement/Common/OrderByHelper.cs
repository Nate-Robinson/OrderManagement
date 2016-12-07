using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;


namespace OrderManagement.Common
{
    public static class OrderByHelper<T>
    {
        /// <summary>
        /// 动态排序
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="sortFieldName">排序字段名</param>
        /// <param name="sortDir">desc:降序；asc:升序</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy(IQueryable<T> data, string sortFieldName, string sortDir)
        {
            string sortingDir = string.Empty;
            if (sortDir.ToUpper().Trim() == "ASC")
            {
                sortingDir = "OrderBy";
            }
            else if (sortDir.ToUpper().Trim() == "DESC")
            {
                sortingDir = "OrderByDescending";
            }
            ParameterExpression param = Expression.Parameter(typeof(T), sortFieldName);
            PropertyInfo pi = typeof(T).GetProperty(sortFieldName);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, data.Expression, Expression.Lambda(Expression.Property(param, sortFieldName), param));
            IQueryable<T> query = data.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;

        }
    }

}