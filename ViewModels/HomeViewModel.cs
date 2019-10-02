using RDPMonWebGUI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace RDPMonWebGUI.ViewModels
{
    public class HomeViewModel
    {
        #region Properties
        public string Title { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; } = "desc";

        public int Page { get; set; } = 1;

        public int PageSize { get; set; }

        public int PaginatorStart { get; set; } = 1;

        public int PaginatorEnd { get; set; } = 11;

        public int PageCount { get; set; }

        public Type ModelType { get; set; }

        public List<object> Records { get; set; }

        private List<object> _recordsPaged;
        public List<object> RecordsPaged
        {
            get
            {
                if (_recordsPaged == null && Records != null)
                {
                    _recordsPaged = Records.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
                }

                return _recordsPaged;
            }
        }

        private List<PropertyInfo> _properties;
        public List<PropertyInfo> Properties
        {
            get
            {
                if (_properties == null && ModelType != null)
                {
                    _properties = ModelType.GetProperties().Where(prop => prop.GetCustomAttribute<NotMappedAttribute>(true) == null).ToList();
                }

                return _properties;
            }
        }

        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        public Dictionary<string, string> Headers
        {
            get
            {
                if (_headers.Count == 0 && Properties != null)
                {
                    foreach (PropertyInfo info in Properties)
                    {
                        DisplayNameAttribute nameAttribute = info.GetCustomAttributes<DisplayNameAttribute>(true).FirstOrDefault();

                        _headers.Add(info.Name, nameAttribute != null ? nameAttribute.DisplayName : info.Name);
                    }
                }

                return _headers;
            }
        }

        public List<RecordActionButton> RecordActionButtons { get; set; } = new List<RecordActionButton>();
        #endregion

        #region Methods
        public object GetValueForRecord(object record, string propertyName)
        {
            PropertyInfo property = Properties.FirstOrDefault(property => property.Name == propertyName);
            object value = property.GetValue(record);

            return value is IEnumerable<string> ? string.Join(", ", (IEnumerable<string>)value) : value;
        }

        public string GetSortDirectionForField(string field)
        {
            return field == SortField && SortDirection == "asc" ? "desc" : "asc";
        }

        public string GetSortIconForField(string field)
        {
            return field == SortField && SortDirection == "asc" ? "sort-down" : "sort-up";
        }

        public List<object> SetOrderAndPaging<T>(IEnumerable<T> enumerable)
        {
            switch (SortDirection)
            {
                case "asc":
                    enumerable = enumerable.OrderBy(SortField);
                    break;

                case "desc":
                    enumerable = enumerable.OrderByDescending(SortField);
                    break;
            }

            List<object> records = enumerable.Cast<object>().ToList();

            PageCount = Convert.ToInt32(Math.Ceiling(records.Count / Convert.ToDouble(PageSize)));

            if (Page - 5 > 0)
            {
                PaginatorStart = Page - 5;
                PaginatorEnd = Page + 5;
            }

            if (PageCount < PaginatorEnd)
            {
                PaginatorStart = PageCount - 10 > 0 ? PageCount - 10 : 1;
                PaginatorEnd = PageCount;
            }

            return records;
        }
        #endregion
    }
}
