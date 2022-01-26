using System.Linq.Expressions;
using System.Reflection;

namespace RDPMonWebGUI.Extensions;

public static class OrderByExtension
{
    /// <summary>
    /// Extension method that makes it possible to use strings to create an IEnumerable with an OrderBy.
    /// </summary>
    /// <typeparam name="TSource">The Entity/Model to create the OrderBy for.</typeparam>
    /// <param name="query">The IEnumerable to create the OrderBy for.</param>
    /// <param name="propertyName">The propertyname as string to order by. This can be a "." seperated string, to order deeper in the Entity/Model.</param>
    /// <returns>An List containing the order provided.</returns>
    public static List<TSource> OrderBy<TSource>(this IEnumerable<TSource> query, string propertyName) =>
        GetMethodInfoForOrderBy(query.AsQueryable(), propertyName, "OrderBy");

    /// <summary>
    /// Extension method that makes it possible to use strings to create an IEnumerable with an OrderByDescending.
    /// </summary>
    /// <typeparam name="TSource">The Entity/Model to create the OrderBy for.</typeparam>
    /// <param name="query">The IEnumerable to create the OrderBy for.</param>
    /// <param name="propertyName">The propertyname as string to order by. This can be a "." seperated string, to order deeper in the Entity/Model.</param>
    /// <returns>An List containing the order provided.</returns>
    public static List<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> query, string propertyName) =>
        GetMethodInfoForOrderBy(query.AsQueryable(), propertyName, "OrderByDescending");

    /// <summary>
    /// Builds up the Expression tree to create the ordering with.
    /// </summary>
    /// <typeparam name="TSource">The Entity/Model to create the OrderBy for.</typeparam>
    /// <param name="query">The query to create the OrderBy for.</param>
    /// <param name="propertyName">The propertyname as string to order by. This can be a "." seperated string, to order deeper in the Entity/Model.</param>
    /// <param name="methodName">The method to order by. Valid values are "OrderBy" or "OrderByDescending".</param>
    /// <returns>The List containing the requested order.</returns>
    private static List<TSource> GetMethodInfoForOrderBy<TSource>(IQueryable<TSource> query, string propertyName, string methodName)
    {
        List<string> propertyNameParts = propertyName.Split(new char[] { '.' }).ToList();
        Type entityType = typeof(TSource);
        PropertyInfo propertyInfo = entityType.GetProperty(propertyName);

        ParameterExpression parameter = Expression.Parameter(entityType, "x");
        Expression expression = parameter;
        Type subEntityType = null;

        for (int i = 0; i < propertyNameParts.Count; i++)
        {
            if (i != propertyNameParts.Count - 1)
            {
                subEntityType = entityType.GetProperty(propertyNameParts[i]).GetType();
            }

            expression = Expression.PropertyOrField(expression, propertyNameParts[i]);
        }

        if (subEntityType != null)
        {
            propertyInfo = subEntityType.GetProperty(propertyNameParts.Last());
        }

        if (propertyInfo.PropertyType == typeof(DateTime))
        {
            var selector = Expression.Lambda<Func<TSource, DateTime>>(expression, parameter);
            query = methodName == "OrderBy" ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }
        else if (propertyInfo.PropertyType == typeof(DateTime?))
        {
            var selector = Expression.Lambda<Func<TSource, DateTime?>>(expression, parameter);
            query = methodName == "OrderBy" ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }
        else if (propertyInfo.PropertyType == typeof(int))
        {
            var selector = Expression.Lambda<Func<TSource, int>>(expression, parameter);
            query = methodName == "OrderBy" ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }
        else if (propertyInfo.PropertyType == typeof(Int64))
        {
            var selector = Expression.Lambda<Func<TSource, Int64>>(expression, parameter);
            query = methodName == "OrderBy" ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }
        else if (propertyInfo.PropertyType == typeof(bool))
        {
            var selector = Expression.Lambda<Func<TSource, bool>>(expression, parameter);
            query = methodName == "OrderBy" ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }
        else
        {
            var selector = Expression.Lambda<Func<TSource, object>>(expression, parameter);
            query = methodName == "OrderBy" ? query.OrderBy(selector) : query.OrderByDescending(selector);
        }

        return query.ToList();
    }
}