namespace Sitecore.Support.XA.Foundation.Grid.Model
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore.Data.Items;
  using Sitecore.DependencyInjection;
  using Sitecore.StringExtensions;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Fields.FieldRenderers;
  using Sitecore.XA.Foundation.Grid.Parser;
  using Sitecore.XA.Foundation.SitecoreExtensions.Services;
  using System.Collections.Concurrent;
  using System.Collections.Generic;

  public class GridDefinition : Sitecore.XA.Foundation.Grid.Model.GridDefinition
  {
    #region Modified code
    private static ConcurrentDictionary<string, IGridFieldParser> parsers = new ConcurrentDictionary<string, IGridFieldParser>();
    #endregion

    public GridDefinition(Item item) : base(item) { }

    public new IGridFieldParser InstantiateGridFieldParser()
    {
      if (GridFieldParserType.IsNullOrEmpty())
      {
        return null;
      }
      if (parsers.ContainsKey(GridFieldParserType))
      {
        return parsers[GridFieldParserType];
      }
      IGridFieldParser gridFieldParser = ServiceLocator.ServiceProvider.GetService<IReflectionService>().Instantiate<IGridFieldParser>(GridFieldParserType, Item);
      #region Modified code
      parsers.TryAdd(GridFieldParserType, gridFieldParser);
      #endregion
      return gridFieldParser;
    }
  }
}