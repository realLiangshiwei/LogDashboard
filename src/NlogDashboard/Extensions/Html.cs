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

            var start = page - 3;

            if (start > 1)
            {
                build.Append("<li class='page-item'><a class='page-link'  onclick=goPage('1')>首页</a></li>");
            }
            else
            {
                start = 1;
            }

            var end = totalPage - page;

            var endPage = "";

            if (end > 3)
            {
                end = page + 3;
                endPage = $"<li class='page-item'><a class='page-link'  onclick=goPage('{totalPage}')>尾页</a></li>";
            }
            else if (end <= 3)
            {
                end = totalPage;
            }

            if (totalPage >= 7)
            {

                if (page - start < 3)
                {
                    end += Math.Abs(start - 3);
                }
                else
                {
                    if (totalPage - end < 3)
                    {
                        start -= Math.Abs(page + 3 - end);
                    }
                }

            }

            for (var i = start; i <= end; i++)
            {
                build.Append(i == page
                    ? $"<li class='page-item active'><a class='page-link' >{i}</a></li>"
                    : $"<li class='page-item'><a class='page-link' onclick=goPage('{i}')>{i}</a></li>");
            }

            build.Append(endPage);
            build.Append(@" </ul></ nav >");

            return build.ToString();
        }
    }
}
