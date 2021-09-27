using CourseLibrary.API.Services.MappingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace CourseLibrary.API.Helpers
{
    public static class IQueryableExtention
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, 
            string orderBy, 
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;

            // the orderBy string is separated by ",", so we split
            var orderByAfterSplit = orderBy.Split(',');

            // apply each orderby clouse in reverse order - otherwhise, the
            // IQueryble will be ordered in the wrong order
            foreach (var orderByClouse in orderByAfterSplit.Reverse())
            {
                // trim the orderBy clause, as might contain leading
                // or trailing spaces. Can't trim the var in foreach, so use another var
                var trimmedOrderByClause = orderByClouse.Trim();

                // if the sort option ends with " desc", we order descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // remove " asc" or " desc" from the orderByClouse, so we
                // get the property name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing.");
                }

                // get the PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                // Run through the property names so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    // revert sort order if necessary
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    orderByString = orderByString + (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                        + destinationProperty + (orderDescending ? " descending" : " ascending");
                }
            }
            return source.OrderBy(orderByString);
        }
    }
}
