using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Layouts;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.RenderingProcessing;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Parser;

namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipelines.Import.RenderingProcessing
{
  public class GridStylesImporter : Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.RenderingProcessing.GridStylesImporter
  {
    protected override void UpdateGridParameters(ImportRenderingProcessingArgs args, IEnumerable<GridClass> gridClasses)
    {
      List<GridClass> classes = gridClasses.ToList();
      Item item = args.RenderingSourceItem;

      LayoutField layoutField = new LayoutField(item);
      string presentationXml = layoutField.Value;

      LayoutDefinition definition = LayoutDefinition.Parse(presentationXml);
      var renderingDefinition = definition.GetDevice(args.ImportContext.DeviceId.ToString()).GetRenderingByUniqueId(args.RenderingUniqueId.ToString());
      if (renderingDefinition == null)
      {
        return;
      }

      if (!args.Attributes.ContainsKey(Sitecore.XA.Feature.CreativeExchange.Constants.GridFieldDataAttribute))
      {
        return;
      }

      string gridParamsField = args.Attributes[Sitecore.XA.Feature.CreativeExchange.Constants.GridFieldDataAttribute].First();
      var parameters = WebUtil.ParseUrlParameters(renderingDefinition.Parameters ?? String.Empty);
      if (!classes.Any() && string.IsNullOrWhiteSpace(parameters[gridParamsField]))
      {
        return;
      }

      var device = args.Page.Database.GetItem(args.ImportContext.DeviceId);
      var gridDefinitionItem = ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(args.Page, device);
      parameters[gridParamsField] = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem).InstantiateGridFieldParser().ToFieldValue(classes.Select(c => c.Id));

      renderingDefinition.Parameters = new UrlString(parameters).GetUrl();

      using (new EditContext(item))
      {
        layoutField.Value = definition.ToXml();
      }
    }
  }
}