using RDPMonWebGUI.Attributes;
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
        /// <summary>
        /// The title for the Bootstrap Card.
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// The field to sort the list on.
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// The direction to sort the list in, defaults to descending.
        /// </summary>
        public string SortDirection { get; set; } = "desc";

        /// <summary>
        /// The current page viewed, defaults to 1.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// The amount of records on one page, configured in appsettings.json.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The start position of the numbering on the actual pagination controls. Can shift around when navigating.
        /// </summary>
        public int PaginatorStart { get; set; } = 1;

        /// <summary>
        /// The end position of the numbering on the actual pagination controls. Can shift around when navigating.
        /// </summary>
        public int PaginatorEnd { get; set; } = 11;

        /// <summary>
        /// The total numbers of pages.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// The Type of Entity/Model being viewed. Used to get the properties to display in a generic fashion.
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// The List of Entities/Models to list in the view. Will be of Type ModelType.
        /// </summary>
        public List<object> Records { get; set; }

        private List<object> _recordsPaged;
        /// <summary>
        /// Returns List of Type ModelType paged. Skipping to the requested Page -1 and Taking PageSize amount of records.
        /// </summary>
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
        /// <summary>
        /// All the properties of ModelType which are not decorated with NotMappedAttributes.
        /// </summary>
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
        /// <summary>
        /// The headers for the List of Records. Retrieved from the Properties and retrieved by the DisplayNameAttribute if set, otherwise the property.name.
        /// </summary>
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

        /// <summary>
        /// The List of RecordActionButtons to display behind each record in the List of Records.
        /// </summary>
        public List<RecordActionButton> RecordActionButtons { get; set; } = new List<RecordActionButton>();
        #endregion

        #region Methods
        /// <summary>
        /// Given a record of ModelType retrieves the value by the propertyName.
        /// If the propertyName is an IENumerable, join the values together comma seperated.
        /// </summary>
        /// <param name="record">The record of type ModelType to retrieve the value for.</param>
        /// <param name="propertyName">The name of the property to retrieve the value for.</param>
        /// <returns>The value retrieved for the record.</returns>
        public object GetValueForRecord(object record, string propertyName)
        {
            PropertyInfo property = Properties.FirstOrDefault(property => property.Name == propertyName);
            object value = property.GetValue(record);

            return value is IEnumerable<string> ? string.Join(", ", (IEnumerable<string>)value) : value;
        }

        /// <summary>
        /// Determines the sort direction for a given field. If the given field is equal to SortField and the SortDirection is "asc" return "desc",
        /// otherwise return "asc".
        /// </summary>
        /// <param name="field">The field to retrieve the sort direction for.</param>
        /// <returns>The sortdirection to use in the asp-route.</returns>
        public string GetSortDirectionForField(string field)
        {
            return field == SortField && SortDirection == "asc" ? "desc" : "asc";
        }

        /// <summary>
        /// Retrieves the sort icon for a given field.  the given field is equal to SortField and the SortDirection is "asc" return "sort-down",
        /// otherwise return "sort-up".
        /// </summary>
        /// <param name="field">The field to retrieve the sort icon for.</param>
        /// <returns>The sort icon's CSS class.</returns>
        public string GetSortIconForField(string field)
        {
            return field == SortField && SortDirection == "asc" ? "sort-down" : "sort-up";
        }

        /// <summary>
        /// Whether a field can be sorted or not.
        /// </summary>
        /// <param name="field">The field to check the presence of a DisableSortAttribute for.</param>
        /// <returns>Whether a field can be sorted or not</returns>
        public bool IsSortable(string field)
        {
            return ModelType.GetProperties().FirstOrDefault(prop => prop.Name == field).GetCustomAttributes<DisableSortAttribute>(true).FirstOrDefault() == null;
        }

        /// <summary>
        /// Given the List of records, set the sort direction and set the pagination properties.
        /// </summary>
        /// <typeparam name="T">The type of Record to apply sorting and paging to.</typeparam>
        /// <param name="enumerable">The List of Records to apply sotring and paging to.</param>
        /// <returns>A sorted List of Records.</returns>
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
