using CourseLibrary.API.Services.MappingService;
using System.Collections.Generic;

namespace CourseLibrary.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSourse, TDestination>();
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}