using System;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Layouts;
using Sitecore.Shell.Framework.Commands;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Model;

namespace Sitecore.Support.XA.Foundation.Grid.Commands
{
  public class SetGridValues : Sitecore.XA.Foundation.Grid.Commands.SetGridValues
  {
    protected override void FillGridParameters(CommandContext context, DeviceDefinition device, NameValueCollection parameters,
      string fieldName)
    {
      var gridDefinitionItem = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(context.Items.First(), Client.ContentDatabase.GetItem(device.ID));
      var gridSettings = context.Parameters["gridSettings"].Split(new[]
      {
        '|'
      }, StringSplitOptions.RemoveEmptyEntries).Select(ID.Parse).Where(id => !id.IsNull).ToList();

      if (gridSettings.Any())
      {
        parameters[fieldName] = new GridDefinition(gridDefinitionItem).InstantiateGridFieldParser().ToFieldValue(gridSettings);
      }
      else
      {
        parameters[fieldName] = string.Empty;
      }
    }
  }
}