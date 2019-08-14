using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Layouts;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;

namespace Sitecore.Support.XA.Foundation.Grid.Commands
{
  public class ShowGridPropertiesDialog : Sitecore.XA.Foundation.Grid.Commands.ShowGridPropertiesDialog
  {
    protected override void FillGridParameters(NameValueCollection contextParameters, DeviceDefinition device, NameValueCollection parameters, string fieldName, string value)
    {
      var item = ServiceLocator.ServiceProvider.GetService<IContentRepository>().GetItem(contextParameters["itemId"]);
      var gridDefinitionItem = ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(item, Client.ContentDatabase.GetItem(device.ID));
      var gridSettings = value.Split(new[]
      {
        '|'
      }, StringSplitOptions.RemoveEmptyEntries).Select(ID.Parse).Where(id => !id.IsNull).ToList();

      if (gridSettings.Any())
      {
        parameters[fieldName] = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem).InstantiateGridFieldParser().ToFieldValue(gridSettings);
      }
      else
      {
        parameters[fieldName] = string.Empty;
      }
    }
  }
}