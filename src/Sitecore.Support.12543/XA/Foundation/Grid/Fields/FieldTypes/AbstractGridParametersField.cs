using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Fields.FieldRenderers;
using Sitecore.XA.Foundation.Grid.Model;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Services;

namespace Sitecore.Support.XA.Foundation.Grid.Fields.FieldTypes
{
  public class AbstractGridParametersField : Sitecore.XA.Foundation.Grid.Fields.FieldTypes.AbstractGridParametersField
  {
    protected override IGridParametersFieldRenderer GridSettingsFieldRenderer()
    {
      ItemUri itemUri = GetItemUri();
      var db = Data.Database.GetDatabase(itemUri.DatabaseName);
      Item item = db.GetItem(itemUri.ItemID);
      var deviceId = ServiceLocator.ServiceProvider.GetService<ISuspendedPipelineService>().GetCurrentDeviceId();
      var device = db.GetItem(deviceId);

      var gridDefinitionItem = ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(item, device);
      if (gridDefinitionItem != null)
      {
        if (gridDefinitionItem.InheritsFrom(Templates.GridDefinition.ID))
        {
          return new GridDefinition(gridDefinitionItem).InstantiateFieldRenderer();
        }
        Log.Error($"Linked grid definition item with itemId:{gridDefinitionItem.ID} has incorrect template", this);
        return null;
      }
      Log.Error($"Could not resolve grid definition item for itemId:{item?.ID}, device:{device?.ID}", this);
      return null;
    }
  }
}