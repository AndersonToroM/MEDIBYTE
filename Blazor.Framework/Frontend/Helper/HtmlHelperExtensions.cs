using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc.Internals;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dominus.Frontend.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static DominusWidgetFactory<object> DControls(this IHtmlHelper html)
        {
            return new DominusWidgetFactory<object>((OptionsOwnerBuilder)null, (IHtmlHelperAdapter)new HtmlHelperAdapter(html));
        }

        public static DominusWidgetFactory<TModel> DControls<TModel>(this IHtmlHelper<TModel> html)
        {
            return new DominusWidgetFactory<TModel>((OptionsOwnerBuilder)null, (IHtmlHelperAdapter)new HtmlHelperAdapter((IHtmlHelper)html));
        }
    }


}



