using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.XA.Feature.PageStructure.Models;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Model;
using Sitecore.XA.Foundation.Grid.Parser;

namespace Sitecore.Support.XA.Feature.PageStructure.Repositories
{
  public class SplitterRepository : Sitecore.XA.Feature.PageStructure.Repositories.SplitterRepository
  {
    private const string EnabledPlaceholders = "EnabledPlaceholders"; //because Sitecore.XA.Feature.PageStructure.Constants is internal
    protected override List<string> GetColumnsStyles()
    {
      Item gridDefinitionItem = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(Context.Item, Context.Device);

      if (gridDefinitionItem == null)
      {
        return null;
      }

      GridDefinition gridDefinition = new GridDefinition(gridDefinitionItem);
      IGridFieldParser parser = gridDefinition.InstantiateGridFieldParser();


      var placeholders = new EnabledPlaceholders(Rendering.Parameters[EnabledPlaceholders]);

      return placeholders.PlaceholdersIndexes.Select(col =>
      {
        col = col - 1;
        string columnProperties = Rendering.Parameters[ColumnWidthParameterService.GetColumnWidthParameter(col)];
        if (!string.IsNullOrWhiteSpace(columnProperties))
        {
          var classes = parser.ParseFromFieldValue(columnProperties).ToList();
          if (classes.Any())
          {
            return string.Join(" ", classes);
          }
        }

        return string.Empty;

      }).ToList();
    }
  }
}