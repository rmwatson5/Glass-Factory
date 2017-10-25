namespace SBR.GlassFactory
{
    public interface IExtendedGlassView<out TPageItem>
        where TPageItem : class
    {
        TPageItem PageItem { get; }
    }

    public interface IExtendedGlassView<out TPageItem, out TDataSource> : IExtendedGlassView<TPageItem>
        where TPageItem : class
    {
        TDataSource DataSource { get; }
    }

    public interface IExtendedGlassView<out TPageItem, out TDataSource, out TRenderingParamaters> : IExtendedGlassView<TPageItem, TDataSource>
    where TPageItem : class
    {
        TRenderingParamaters RenderingParamaters { get; }
    }
}
