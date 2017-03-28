using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.MetCalWeb.Helper
{
    public static class ImageExtension
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string height, string width, string cssClass)
        {
            TagBuilder builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("height", height);
            builder.MergeAttribute("width", width);
            builder.MergeAttribute("class", cssClass);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}