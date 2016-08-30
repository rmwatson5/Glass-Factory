namespace SBR.GlassFactory
{
    public interface IRenderingModel<out TPageItem>
    {
        TPageItem PageItem { get; }
    }

    public interface IRenderingModel<out TPageItem, out TDataSource> : IRenderingModel<TPageItem>
    {
        TDataSource DataSource { get; }
    }

    public interface IRenderingModel<out TPageItem, out TDataSource, out TRenderingParamaters> :
        IRenderingModel<TPageItem, TDataSource>
    {
        TRenderingParamaters RenderingParamaters { get; }
    }
}
