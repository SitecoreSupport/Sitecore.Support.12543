namespace Sitecore.Support.XA.Foundation.DynamicPlaceholders.Repositories
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Parser;
  using System.Collections.Generic;
  using System.Linq;

  public class DynamicPlaceholdersRepository : Sitecore.XA.Foundation.DynamicPlaceholders.Repositories.DynamicPlaceholdersRepository
  {
    protected override IList<string> GetColumnsStyles()
    {
      Item gridDefinitionItem = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(Context.Item, Context.Device);
      if (gridDefinitionItem == null)
      {
        return null;
      }
      var gridDefinition = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem);
      IGridFieldParser parser = gridDefinition.InstantiateGridFieldParser();
      return Enumerable.Range(0, GetPlaceholdersCount()).Select(delegate (int col)
      {
        string value = Rendering.Parameters[ColumnWidthParameterService.GetColumnWidthParameter(col)];
        if (!string.IsNullOrWhiteSpace(value))
        {
          List<string> list = parser.ParseFromFieldValue(value).ToList();
          if (list.Any())
          {
            return string.Join(" ", list);
          }
        }
        return string.Empty;
      }).ToList();
    }
  }
}