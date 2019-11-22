using System;
using System.Text;

namespace LogDashboard.Extensions
{
    public class Html
    {
        public static string Page(int page, int pageSize, int totalCount)
        {
            var totalPage = (int)Math.Ceiling(totalCount * 1.0 / pageSize);
            var build = new StringBuilder();
            build.Append("<nav><ul class='pagination justify-content-end'>");

            var start = totalPage == 7 ? 1 : Math.Max(page - 3, 1);

            build.Append("<li class='page-item'><a class='page-link' onclick=goPage('1')>首页</a></li>");

            var end = Math.Min(page + 3, totalPage);

            if (page - start < 3)
            {
                end += totalPage <= 7 ? totalPage - end : Math.Abs(4 - page);
            }
            else if (totalPage - end < 3 && start != 1)
            {
                start -= Math.Abs(page + 3 - end);

            }

            for (var i = start; i <= end; i++)
            {
                build.Append(i == page
                    ? $"<li class='page-item active'><a class='page-link' >{i}</a></li>"
                    : $"<li class='page-item'><a class='page-link' onclick=goPage('{i}')>{i}</a></li>");
            }

            build.Append($"<li class='page-item'><a class='page-link' onclick=goPage('{totalPage}')>尾页</a></li>");
            build.Append($"<li class='page-item'><a class='page-link' >总数{totalCount}条</a></li>");
            build.Append(@" </ul></ nav >");

            return build.ToString();
        }


    }
}
