using System.Reflection;

namespace CourseLibrary.API.Services
{
    public class PropertyCheckerService : IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the fields are separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // check if the requested fields exist on source
            foreach (var field in fieldsAfterSplit)
            {
                // Trim each field, as it might contain leadind or trailing spaces.
                // Can't trim the var in foreach, so use another var
                var propertyName = field.Trim();

                // Use reflection to check if the propery can be found on T.
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // It can't ne found, return false
                if (propertyInfo == null)
                {
                    return false;
                }
            }

            // All checks out, return ture
            return true;
        }
    }
}
