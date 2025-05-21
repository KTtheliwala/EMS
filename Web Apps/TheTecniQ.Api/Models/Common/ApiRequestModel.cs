using TheTecniQ.Core.Domain.Grid;
using System.Collections.Generic;

namespace TheTecniQ.API.Models.Common
{
    public class BaseModel
    {
        public int Id { get; set; }
    }

    public class BaseListModel
    {
        public EnumResponseType ResponseType { get; set; } = EnumResponseType.JSON;
        public ListModel List { get; set; } = new();
        public List<ColumnModel> Columns { get; set; } = null;
    }

    public class ListModel
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SortField { get; set; } = "";
        public bool IsDescending { get; set; } = false;
    }

    public class ColumnModel
    {
        public string FieldName { get; set; }
        public string FieldTitle { get; set; }
    }
}
