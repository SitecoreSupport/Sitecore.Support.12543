namespace Sitecore.Support.XA.Foundation.Grid.Commands
{
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Layouts;
  using Sitecore.Shell.Applications.WebEdit.Commands;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Model;
  using Sitecore.XA.Foundation.IoC;
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using System.Text.RegularExpressions;
  public class SetGridValues : Sitecore.XA.Foundation.Grid.Commands.SetGridValues
  {
    protected override void FillGridParameters(CommandContext context, DeviceDefinition device, NameValueCollection parameters, string fieldName)
    {
      Item gridDefinitionItem = ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(context.Items.First(), Client.ContentDatabase.GetItem(device.ID));
      List<ID> list = (from id in context.Parameters["gridSettings"].Split(new char[1]
      {
            '|'
      }, StringSplitOptions.RemoveEmptyEntries).Select(ID.Parse)
                       where !id.IsNull
                       select id).ToList();
      if (list.Any())
      {
        #region Modified to support grid definition.
        parameters[fieldName] = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem).InstantiateGridFieldParser().ToFieldValue(list);
        #endregion
      }
      else
      {
        parameters[fieldName] = string.Empty;
      }
    }
  }
}