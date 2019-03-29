namespace Sitecore.Support.XA.Foundation.Grid.Fields.FieldTypes
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Fields.FieldRenderers;
  using Sitecore.XA.Foundation.Grid.Model;
  using Sitecore.XA.Foundation.IoC;
  using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
  public class AbstractGridParametersField : Sitecore.XA.Foundation.Grid.Fields.FieldTypes.AbstractGridParametersField
  {
    protected override IGridParametersFieldRenderer GridSettingsFieldRenderer()
    {
      ItemUri itemUri = GetItemUri();
      Database database = Sitecore.Data.Database.GetDatabase(itemUri.DatabaseName);
      Item item = database.GetItem(itemUri.ItemID);
      ID currentDeviceId = GetCurrentDeviceId();
      Item item2 = database.GetItem(currentDeviceId);
      Item gridDefinitionItem = ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(item, item2);
      if (gridDefinitionItem != null)
      {
        if (gridDefinitionItem.InheritsFrom(Templates.GridDefinition.ID))
        {
          #region Modified to support grid definition
          return new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem).InstantiateFieldRenderer();
          #endregion
        }
        Log.Error($"Linked grid definition item with itemId:{gridDefinitionItem.ID} has incorrect template", this);
        return null;
      }
      Log.Error($"Could not resolve grid definition item for itemId:{item?.ID}, device:{item2?.ID}", this);
      return null;
    }
  }
}