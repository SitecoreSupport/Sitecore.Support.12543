namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipelines.Import.PageProcessing
{
using Sitecore.Data.Items;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.PageProcessing;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Model;
using Sitecore.XA.Foundation.Grid.Parser;
using Sitecore.XA.Foundation.IoC;
using System.Collections.Generic;
  public class GetGridDefinitionClasses : ImportPageProcessingBase
  {
    public override void Process(ImportPageProcessingArgs args)
    {
      Item item = args.PageContext.ImportContext.Database.GetItem(args.PageContext.ImportContext.DeviceId);
      #region Modified to support grid definition.
      IEnumerable<GridClass> allClasses = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(args.PageContext.Item, item)).InstantiateGridFieldParser().GetAllClasses();
      #endregion
      args.GridDefinitionClasses.AddRange(allClasses);
    }
  }
}