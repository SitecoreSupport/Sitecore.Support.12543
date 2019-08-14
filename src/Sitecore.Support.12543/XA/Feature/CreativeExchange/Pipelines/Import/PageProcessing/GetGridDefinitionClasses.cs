using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.PageProcessing;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Model;
using Sitecore.XA.Foundation.Grid.Parser;

namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipelines.Import.PageProcessing
{
  public class GetGridDefinitionClasses : Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.PageProcessing.GetGridDefinitionClasses
  {
    public override void Process(ImportPageProcessingArgs args)
    {
      var device = args.PageContext.ImportContext.Database.GetItem(args.PageContext.ImportContext.DeviceId);
      var gridDefinitionItem = ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(args.PageContext.Item, device);
      IGridFieldParser fieldParser = new GridDefinition(gridDefinitionItem).InstantiateGridFieldParser();
      args.GridDefinitionClasses.AddRange(fieldParser.GetAllClasses());
      args.IgnoredGridClasses.AddRange(fieldParser.GetIgnoredGridClasses());
    }
  }
}