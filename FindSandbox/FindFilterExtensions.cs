using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FindSandbox
{
    public static class FindFilterExtensions
    {
        //public static DelegateFilterBuilder MatchArrayContainedCaseInsensitive<T>(this IEnumerable<T> value, Expression fieldSelector, FieldFilterValue valueToMatch)
        //{
        //    return new DelegateFilterBuilder((string field) => new TermFilter(field, valueToMatch))
        //    {
        //        FieldNameMethod = ((Expression expression, IClientConventions conventions) => $"{conventions.FieldNameConvention.GetFieldName(expression)}.{conventions.FieldNameConvention.GetFieldName(fieldSelector)}")
        //    };
        //}


        public static DelegateFilterBuilder MatchArrayContainedCaseInsensitive<T>(this IEnumerable<T> value, Expression<Func<T, string>> fieldSelector, string valueToMatch)
        {
            return new DelegateFilterBuilder((string field) => new TermFilter(field, valueToMatch.ToLowerInvariant()))
            {
                FieldNameMethod = ((Expression expression, IClientConventions conventions) => $"{conventions.FieldNameConvention.GetFieldName(expression)}.{conventions.FieldNameConvention.GetFieldNameForLowercase(fieldSelector)}")
            };
        }
    }
}
