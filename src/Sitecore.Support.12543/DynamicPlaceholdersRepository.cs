namespace Sitecore.Support.XA.Foundation.DynamicPlaceholders.Repositories
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Model;
  using Sitecore.XA.Foundation.Grid.Parser;
  using System.Collections.Generic;
  using System.Linq;

  public class DynamicPlaceholdersRepository : Sitecore.XA.Foundation.DynamicPlaceholders.Repositories.DynamicPlaceholdersRepository
  {
    protected override List<string> GetColumnsStyles()
    {
      Item gridDefinitionItem = Sitecore.XA.Foundation.IoC.ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(Context.Item, Context.Device);

      if (gridDefinitionItem == null)
      {
        return null;
      }
      #region Modified Code Sitecore.Support.12543
      Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition gridDefinition = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem);
      #endregion
      IGridFieldParser parser = gridDefinition.InstantiateGridFieldParser();

      return Enumerable.Range(0, GetPlaceholdersCount()).Select(col =>
      {
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