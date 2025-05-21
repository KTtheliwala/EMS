using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.Permissions;

namespace TheTecniQ.Data.Extensions
{
    public enum OperatorComparer
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals = ExpressionType.Equal,
        GreaterThan = ExpressionType.GreaterThan,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        LessThan = ExpressionType.LessThan,
        LessThanOrEqual = ExpressionType.LessThanOrEqual,
        NotEqual = ExpressionType.NotEqual,
        InClause
    }
    public static class DynamicLinqExpressionBuilder
    {
        public static int ToInt(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static string ConvertToString(this object obj)
        {
            try
            {
                return Convert.ToString(obj);
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static bool ConvertToBool(this object obj)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static DateTime? ConvertToDate(this object obj)
        {
            if (obj == null)
                return null;
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<IPagedList<T>> BuildPredicate<T>(this IQueryable<T> query, GridRequestModel objGrid)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), typeof(T).Name);
            // Handle date range filters
            HandleDateRangeFilters(objGrid);

            // Apply filters to the query
            query = ApplyFilters(query, objGrid, parameterExpression);

            // Check for and apply the IsDeleted condition if it exists
            query = ApplyIsDeletedCondition(query, parameterExpression);

            // Apply sorting if necessary
            query = ApplySorting(query, objGrid);

            // Return paginated results
            return await query.ToPagedListAsync(objGrid.First, objGrid.Rows, false, (int)objGrid.ResponseType != 0);
        }
        private static void HandleDateRangeFilters(GridRequestModel objGrid)
        {
            var dateFilters = objGrid.Filters
                .Where(x => x.OpType.Equals("DATERANGE", StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            foreach (var filter in dateFilters)
            {
                if (!string.IsNullOrEmpty(filter.FieldValue))
                {
                    string[] dates = filter.FieldValue?.Split('-');
                    string firstDate = dates?[0]?.Trim() ?? "";
                    string secondDate = dates?.Length > 1 ? dates[1]?.Trim() : "";

                    if (!string.IsNullOrWhiteSpace(firstDate))
                    {
                        objGrid.Filters.Add(new SearchGrid
                        {
                            FieldName = filter.FieldName,
                            FieldValue = Convert.ToDateTime(firstDate).ToString("yyyy-MM-dd HH:mm"),
                            OpType = (!string.IsNullOrWhiteSpace(secondDate))?"GreaterThanOrEqual": "Equals",
                            IsTimeZone = filter.IsTimeZone
                        });
                    }

                    filter.FieldValue = !string.IsNullOrWhiteSpace(secondDate) ? $"{Convert.ToDateTime(secondDate).ToString("yyyy-MM-dd")} 23:59:59" : "";
                    filter.OpType = "LessThanOrEqual";
                }
            }
        }
        private static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, GridRequestModel objGrid, ParameterExpression parameterExpression)
        {
            foreach (var filter in objGrid.Filters.Where(x => !string.IsNullOrWhiteSpace(x.FieldValue)))
            {
                Expression<Func<T, bool>> predicate;
                if (filter.IsTimeZone)
                {
                    predicate = filter.OpType == OperatorComparer.InClause.ToString()
                    ? SelectListContainsPredicate<T>(filter.FieldName, filter.FieldValue.Replace('|', ','))
                    : (Expression<Func<T, bool>>)BuildNavigationExpression(parameterExpression, (OperatorComparer)Enum.Parse(typeof(OperatorComparer), filter.OpType), filter.FieldValue, objGrid.Timezone, filter.FieldName);
                }
                else
                {
                    predicate = filter.OpType == OperatorComparer.InClause.ToString()
                    ? SelectListContainsPredicate<T>(filter.FieldName, filter.FieldValue.Replace('|', ','))
                    : (Expression<Func<T, bool>>)BuildNavigationExpression(parameterExpression, (OperatorComparer)Enum.Parse(typeof(OperatorComparer), filter.OpType), filter.FieldValue, "", filter.FieldName);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private static IQueryable<T> ApplyIsDeletedCondition<T>(IQueryable<T> query, ParameterExpression parameterExpression)
        {
            var isDeletedProperty = parameterExpression.Type.GetProperty("IsDeleted", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (isDeletedProperty != null)
            {
                var isDeletedPredicate = (Expression<Func<T, bool>>)BuildCondition(parameterExpression, "IsDeleted", OperatorComparer.Equals, "false", "");
                query = query.Where(isDeletedPredicate);
            }

            return query;
        }

        private static IQueryable<T> ApplySorting<T>(IQueryable<T> query, GridRequestModel objGrid)
        {
            if (!string.IsNullOrWhiteSpace(objGrid.SortField))
            {
                query = query.OrderBy(objGrid.SortField, objGrid.SortOrder == -1);
            }

            return query;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            Type type = typeof(TEntity);
            PropertyInfo property = type.GetProperty(orderByProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            ParameterExpression parameter = Expression.Parameter(type, "p");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType],
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
        public static IQueryable<T> ThenByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        private static IQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, [source, lambda]);
            return (IOrderedQueryable<T>)result;
        }
        private static Expression BuildOrExpression(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
            {
                return expressionToAdd;
            }

            //Build 'OR' expression for each property
            return Expression.OrElse(existingExpression, expressionToAdd);
        }
        private static Expression BuildNavigationExpression(Expression parameter, OperatorComparer comparer, object value, string offset, params string[] properties)
        {
            Expression childParameter, predicate;
            Type childType = null;

            Expression resultExpression;
            if (properties.Length > 1)
            {
                //build path
                parameter = Expression.Property(parameter, properties[0]);
                bool isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                //if it´s a collection we later need to use the predicate in the methodexpressioncall
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                //skip current property and get navigation property expression recursivly
                string[] innerProperties = properties.Skip(1).ToArray();
                predicate = BuildNavigationExpression(childParameter, comparer, value, offset, innerProperties);
                if (isCollection)
                {
                    //build subquery
                    resultExpression = BuildSubQuery(parameter, childType, predicate);
                }
                else
                {
                    resultExpression = predicate;
                }
            }
            else
            {
                //build final predicate
                resultExpression = BuildCondition(parameter, properties[0], comparer, value, offset);
            }
            return resultExpression;
        }

        private static LambdaExpression BuildSubQuery(Expression parameter, Type childType, Expression predicate)
        {
            MethodInfo anyMethod = typeof(Enumerable).GetMethods().Single(m => m.Name == "Any" && m.GetParameters().Length == 2);
            anyMethod = anyMethod.MakeGenericMethod(childType);
            predicate = Expression.Call(anyMethod, parameter, predicate);
            return MakeLambda(parameter, predicate);
        }

        private static LambdaExpression BuildCondition(Expression parameter, string property, OperatorComparer comparer, object value, string offset)
        {
            if (property.Contains('|'))
            {
                Expression final = null;
                foreach (string pro in property.Split('|'))
                {
                    PropertyInfo childProperty = parameter.Type.GetProperty(pro, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    MemberExpression left = Expression.Property(parameter, childProperty);
                    Expression right;
                    if (left.Type == typeof(string))
                    {
                        var variable = new { value };
                        right = Expression.Property(Expression.Constant(variable), nameof(value));
                    }
                    else
                        right = Expression.Constant(value);
                    Expression predicate = BuildComparsion(left, comparer, right, offset);
                    final = BuildOrExpression(final, predicate);
                }
                return MakeLambda(parameter, final);
            }
            else
            {
                PropertyInfo childProperty = parameter.Type.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                MemberExpression left = Expression.Property(parameter, childProperty);
                Expression right;
                if (left.Type == typeof(string))
                {
                    var variable = new { value };
                    right = Expression.Property(Expression.Constant(variable), nameof(value));
                }
                else
                    right = Expression.Constant(value);

                Expression predicate = BuildComparsion(left, comparer, right, offset);
                return MakeLambda(parameter, predicate);
            }
        }

        public static Expression<Func<T, bool>> SelectListContainsPredicate<T>(string columnName, string searchValues, char separator = ',')
        {
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "x");
            ConstantExpression constant = Expression.Constant(true);
            PropertyInfo property = type.GetProperty(columnName);
            string[] values = searchValues.Split(separator);
            if (property == null || values.Length == 0)
            {
                return Expression.Lambda<Func<T, bool>>(constant, parameter);
            }
            MemberExpression member = Expression.Property(parameter, property);
            bool isInt = property.PropertyType.FullName.Contains("Int", StringComparison.CurrentCulture) || property.PropertyType.BaseType.Name.Contains("Enum", StringComparison.CurrentCulture);
            MethodInfo method = isInt ? typeof(List<int>).GetMethod("Contains") : typeof(List<string>).GetMethod("Contains");
            constant = Expression.Constant(isInt ? values.Select(int.Parse).ToList() : values.ToList());
            Expression memberAsInt = isInt ? Expression.Convert(member, typeof(int)) : Expression.Convert(member, typeof(string));
            Expression expression = Expression.Call(constant, method, memberAsInt);
            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
        private static Expression BuildComparsion(Expression left, OperatorComparer comparer, Expression right, string offset)
        {
            var stringComparers = new HashSet<OperatorComparer>
    {
        OperatorComparer.Contains,
        OperatorComparer.StartsWith,
        OperatorComparer.EndsWith
    };

            if (stringComparers.Contains(comparer) && left.Type != typeof(string))
            {
                comparer = OperatorComparer.Equals;
            }

            if (!stringComparers.Contains(comparer))
            {
                return BuildNonStringComparison(left, comparer, right, offset);
            }
            return BuildStringCondition(left, comparer, right);
        }

        private static BinaryExpression BuildNonStringComparison(Expression left, OperatorComparer comparer, Expression right, string offset)
        {
            if (left.Type == typeof(string))
            {
                return BuildBinaryExpression(comparer, left, right, left.Type);
            }

            if (left.Type == typeof(DateTime) || left.Type == typeof(Nullable<DateTime>))
            {
                return BuildDateTimeComparison(left, comparer, (ConstantExpression)right, offset);
            }

            if (left.Type.IsEnum || left.Type.IsNullableType())
            {
                return BuildEnumOrNullableComparison(left, comparer, right);
            }

            return BuildParsedComparison(left, comparer, right);
        }

        private static BinaryExpression BuildDateTimeComparison(Expression left, OperatorComparer comparer, ConstantExpression right, string offset)
        {
            var dateTimeValue = Convert.ToDateTime(right.Value?.ToString().Replace("\"", ""));

            if (!string.IsNullOrWhiteSpace(offset))
            {
                dateTimeValue = dateTimeValue.ToUtcDateTime(offset);
            }

            var dateTimeExpression = Expression.Constant(dateTimeValue);
            var targetType = left.Type == typeof(DateTime) ? typeof(DateTime) : typeof(DateTime?);

            return BuildBinaryExpression(comparer, left, dateTimeExpression, targetType);
        }

        private static BinaryExpression BuildEnumOrNullableComparison(Expression left, OperatorComparer comparer, Expression right)
        {
            if (left.Type.IsNullableType() && left.Type.GetGenericArguments()[0].IsEnum)
            {
                var underlyingType = Nullable.GetUnderlyingType(left.Type);
                return BuildBinaryExpression(comparer, Expression.Call(Expression.Convert(left, underlyingType), "ToString", null), right, underlyingType);
            }

            return BuildBinaryExpression(comparer, Expression.Call(Expression.Convert(left, typeof(int)), "ToString", null), right, typeof(string));
        }

        private static BinaryExpression BuildParsedComparison(Expression left, OperatorComparer comparer, Expression right)
        {
            var parseMethod = left.Type.GetMethod("Parse", [typeof(string)]);
            if (parseMethod != null)
            {
                var parsedExpression = Expression.Call(parseMethod, right);
                return BuildBinaryExpression(comparer, left, parsedExpression, left.Type);
            }

            return Expression.MakeBinary((ExpressionType)comparer, left, right);
        }

        private static BinaryExpression BuildBinaryExpression(OperatorComparer comparer, Expression left, Expression right, Type targetType)
        {
            var convertedRight = Expression.Convert(right, targetType);
            return Expression.MakeBinary((ExpressionType)comparer, left, convertedRight);
        }

        private static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }



        private static MethodCallExpression BuildStringCondition(Expression left, OperatorComparer comparer, Expression right)
        {
            MethodInfo compareMethod = typeof(string).GetMethod(Enum.GetName(typeof(OperatorComparer), comparer), [typeof(string)]);
            MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            left = Expression.Call(left, toLowerMethod);
            MethodInfo toString = typeof(object).GetMethod("ToString", Type.EmptyTypes);
            right = Expression.Call(right, toString);
            right = Expression.Call(right, toLowerMethod);
            return Expression.Call(left, compareMethod, right);
        }

        private static LambdaExpression MakeLambda(Expression parameter, Expression predicate)
        {
            ParameterVisitor resultParameterVisitor = new();
            resultParameterVisitor.Visit(parameter);
            Expression resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }
    }
}
