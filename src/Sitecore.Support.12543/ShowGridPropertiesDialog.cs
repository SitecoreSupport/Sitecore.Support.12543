namespace Sitecore.Support.XA.Foundation.Grid.Commands
{
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Layouts;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.IoC;
  using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  public class ShowGridPropertiesDialog : Sitecore.XA.Foundation.Grid.Commands.ShowGridPropertiesDialog
  {
    protected override void FillGridParameters(NameValueCollection contextParameters, DeviceDefinition device, NameValueCollection parameters, string fieldName, string value)
    {
      Item item = ServiceLocator.Current.Resolve<IContentRepository>().GetItem(contextParameters["itemId"]);
      Item gridDefinitionItem = ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(item, Client.ContentDatabase.GetItem(device.ID));
      List<ID> list = (from id in value.Split(new char[1]
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