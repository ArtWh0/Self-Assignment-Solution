using System;
using System.Text;
using System.Web.Mvc;
using TechRent.WebUI.Models;

namespace TechRent.WebUI.Ht_Helping
{
    public static class Pages_H
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              Paging_Info Paging_Info,
                                              Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= Paging_Info.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == Paging_Info.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}