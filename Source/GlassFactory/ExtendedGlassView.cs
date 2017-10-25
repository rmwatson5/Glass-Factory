using Glass.Mapper.Sc.Web.Mvc;

namespace SBR.GlassFactory
{
    public abstract class ExtendedGlassView<TPageItem> : GlassView<object>, IExtendedGlassView<TPageItem>
        where TPageItem : class
    {
        private readonly RenderingModel<TPageItem> renderingModel;

        protected ExtendedGlassView()
        {
            renderingModel = new RenderingModel<TPageItem>();
        }

        public TPageItem PageItem => this.renderingModel.PageItem;
    }

    public abstract class ExtendedGlassView<TPageItem, TDataSource> : ExtendedGlassView<TPageItem>,
        IExtendedGlassView<TPageItem, TDataSource>
        where TPageItem : class
        where TDataSource : class
    {
        private readonly RenderingModel<TPageItem, TDataSource> renderingModel;

        protected ExtendedGlassView()
        {
            renderingModel = new RenderingModel<TPageItem, TDataSource>();
        }

        public TDataSource DataSource => this.renderingModel.DataSource;
    }

    public abstract class ExtendedGlassView<TPageItem, TDataSource, TRenderingParamaters> : ExtendedGlassView<TPageItem, TDataSource>,
        IExtendedGlassView<TPageItem, TDataSource, TRenderingParamaters>
        where TPageItem : class
        where TDataSource : class
        where TRenderingParamaters : class, new()
    {
        private readonly RenderingModel<TPageItem, TDataSource, TRenderingParamaters> renderingModel;

        protected ExtendedGlassView()
        {
            renderingModel = new RenderingModel<TPageItem, TDataSource, TRenderingParamaters>();
        }

        public TRenderingParamaters RenderingParamaters => this.renderingModel.RenderingParamaters;
    }
}
