using System;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Glass.Mapper.Sc.Configuration.Attributes;
using SBR.GlassFactory.Exceptions;
using SBR.GlassFactory.Helpers;
using Sitecore.Mvc.Presentation;

namespace SBR.GlassFactory
{
    public class RenderingModel<TPageItem> : IRenderingModel<TPageItem>
        where TPageItem : class
    {
        private TPageItem pageItem;
        public TPageItem PageItem => pageItem ?? (pageItem = this.GlassFactory.GetGlassItem<TPageItem>(Sitecore.Context.Item));
        protected readonly Rendering Rendering;
        protected readonly IGlassFactory GlassFactory;

        public RenderingModel()
        {
            this.GlassFactory = new GlassFactory();
            this.Rendering = RenderingContext.Current.Rendering;
        }
    }

    public class RenderingModel<TPageItem, TDataSource> : RenderingModel<TPageItem>, IRenderingModel<TPageItem, TDataSource>
        where TPageItem : class
        where TDataSource : class
    {
        protected readonly bool DataSourceRequired;

        private TDataSource dataSource;
        public TDataSource DataSource
        {
            get
            {
                if (this.dataSource != null)
                {
                    return this.dataSource;
                }

                var dataSourceIdValue = this.Rendering.DataSource;
                Guid dataSourceId;
                if (Guid.TryParse(dataSourceIdValue, out dataSourceId))
                {
                    this.dataSource = this.GlassFactory.GetGlassItem<TDataSource>(dataSourceId);
                }

                if (this.dataSource == null && this.DataSourceRequired)
                {
                    throw new NullDataSourceException(typeof(TDataSource));
                }

                return this.dataSource;
            }
            set { this.dataSource = value; }
        }

        public RenderingModel(bool dataSourceRequired = false)
        {
            this.DataSourceRequired = dataSourceRequired;
        }
    }

    public class RenderingModel<TPageItem, TDataSource, TRenderingParamaters> : RenderingModel<TPageItem, TDataSource>, IRenderingModel<TPageItem, TDataSource, TRenderingParamaters>
        where TPageItem : class
        where TDataSource : class
        where TRenderingParamaters : class
    {
        private TRenderingParamaters renderingParamaters;
        public TRenderingParamaters RenderingParamaters
        {
            get
            {
                if (this.renderingParamaters != null)
                {
                    return this.renderingParamaters;
                }

                this.renderingParamaters = this.GetRenderingParamaters();
                return this.renderingParamaters;
            }
        }

        public RenderingModel(bool dataSourceRequired = false)
            : base(dataSourceRequired)
        {
        }

        private TRenderingParamaters GetRenderingParamaters()
        {
            var sitecoreRenderingParamaters =
                Sitecore.Web.WebUtil.ParseUrlParameters(this.Rendering["Parameters"]);

            var glassSpawn = new GlassSpawn();
            var assignableType = glassSpawn.GetAssignableType(typeof(TRenderingParamaters));
            var renderingParamatersItem = assignableType.CreateInstance<TRenderingParamaters>();
            var properties =
                renderingParamatersItem.GetType()
                    .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public |
                                   BindingFlags.Static)
                    .ToList();

            foreach (var property in properties)
            {
                // Get the attribute so we can get the proper field name
                var attribute = property.GetCustomAttribute(typeof(SitecoreFieldAttribute)) as SitecoreFieldAttribute;
                if (attribute == null)
                {
                    continue;
                }

                var value = sitecoreRenderingParamaters[attribute.FieldName];
                if (value == null)
                {
                    continue;
                }

                var castedValue = TypeHelper.ChangeType(value, property.PropertyType);
                if (castedValue == null)
                {
                    continue;
                }

                // Set the property of the reindering paramaters
                property.SetValue(renderingParamatersItem, castedValue, null);
            }

            return renderingParamatersItem;
        }
    }
}
