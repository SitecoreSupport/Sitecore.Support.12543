namespace Sitecore.Support.XA.Foundation.Grid.Model
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore.Data.Items;
  using Sitecore.DependencyInjection;
  using Sitecore.StringExtensions;
  using Sitecore.XA.Foundation.Grid.Parser;
  using Sitecore.XA.Foundation.SitecoreExtensions.Services;
  using System.Collections.Concurrent;

  public class GridDefinition : Sitecore.XA.Foundation.Grid.Model.GridDefinition
  {
    private static readonly ConcurrentDictionary<string, IGridFieldParser> parsers = new ConcurrentDictionary<string, IGridFieldParser>();

    public GridDefinition(Item item) : base(item) { }

    public new IGridFieldParser InstantiateGridFieldParser()
    {
      if (GridFieldParserType.IsNullOrEmpty())
      {
        return null;
      }

      IGridFieldParser gridFieldParser;
      if (parsers.TryGetValue(GridFieldParserType, out gridFieldParser))
      {
        return gridFieldParser;
      }
      gridFieldParser = ServiceLocator.ServiceProvider.GetService<IReflectionService>().Instantiate<IGridFieldParser>(GridFieldParserType, Item);
      parsers.TryAdd(GridFieldParserType, gridFieldParser);
      return gridFieldParser;
    }
  }
}