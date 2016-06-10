using System.Web;
using Tz.Core.Tools;

namespace Tz.DataObjects
{
    public class JqGridRequest
    {
        public JqGridRequest(HttpContextBase context)
        {
            var page = context.Request["page"].ToInt(1);//页数
            var rows = context.Request["rows"].ToInt(10);//单页行数
            var sort = context.Request["sort"];//排序字段
            var order = context.Request["order"];//倒序还是升序
            var search = context.Request["search"];//搜索请求参数名称
            var totalrows = context.Request["totalrows"].ToInt();

            this.PageIndex = page;
            this.PageSize = rows;
            this.SortName = sort;
            this.Order = order;
            this.Search = search;
            this.TotalRows = totalrows;
        }

        public int TotalRows { get; set; }

        public string Search { get; set; }

        public string Order { get; set; }

        public string SortName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}