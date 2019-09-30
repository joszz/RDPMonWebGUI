using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace RDPMonWebGUI.ViewModels
{
    public class HomeViewModel<T> where T : class
    {
        public HomeViewModel()
        {
            foreach (PropertyInfo info in Properties)
            {
                DisplayNameAttribute nameAttribute = info.GetCustomAttributes<DisplayNameAttribute>(true).FirstOrDefault();

                Headers.Add(nameAttribute != null ? nameAttribute.DisplayName : info.Name);
            }
        }

        public string Title { get; set; }

        public List<T> Records { get; set; }

        private List<PropertyInfo> _properties;
        public List<PropertyInfo> Properties
        {
            get
            {
                if (_properties == null)
                {
                    Type type = typeof(T);
                    _properties = type.GetProperties().Where(prop => prop.GetCustomAttribute<NotMappedAttribute>(true) == null).ToList();
                }

                return _properties;
            }
        }


        public List<string> Headers { get; set; } = new List<string>();

        public object GetValueForRecord(T record, string propertyName)
        {
            PropertyInfo property = Properties.FirstOrDefault(property => property.Name == propertyName);
            object value = property.GetValue(record);

            return value is IEnumerable<string> ? string.Join(", ", (IEnumerable<string>)value) : value;
        }
    }
}
