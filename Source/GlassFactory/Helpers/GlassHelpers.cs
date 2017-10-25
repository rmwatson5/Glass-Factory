using System;
using System.Linq.Expressions;
using System.Web;
using Glass.Mapper.Sc;

namespace SBR.GlassFactory.Helpers
{
	public static class GlassHelpers
	{
        public static IHtmlString RenderImage<TModel>(this TModel model, Expression<Func<TModel, object>> field, object parameters = null, bool isEditable = false)
        {
            var glassHtml = (IGlassHtml)new GlassHtml(new SitecoreContext());
            return new HtmlString(glassHtml.RenderImage(model, field, parameters, isEditable));
        }

        public static IHtmlString RenderEditable<TModel>(this TModel model, Expression<Func<TModel, object>> field,
            object paramaters = null)
        {
            var glassHtml = (IGlassHtml)new GlassHtml(new SitecoreContext());
            return new HtmlString(glassHtml.Editable(model, field, paramaters));
        }

        public static bool IsProxy(this object item)
        {
            if (item == null)
            {
                return false;
            }

            return item.GetType().Name.ToLower().Contains("proxy");
        }

    }
}