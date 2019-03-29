namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipelines.Import.RenderingProcessing
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Layouts;
  using Sitecore.Text;
  using Sitecore.Web;
  using Sitecore.XA.Feature.CreativeExchange;
  using Sitecore.XA.Feature.CreativeExchange.Pipelines.Import.RenderingProcessing;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Model;
  using Sitecore.XA.Foundation.Grid.Parser;
  using Sitecore.XA.Foundation.IoC;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;

  public class GridStylesImporter : ImportRenderingProcessingBase
  {
    public override void Process(ImportRenderingProcessingArgs args)
    {
      if (!CreativeExchangeSettings.SkipGridSystemClassesImprort)
      {
        IEnumerable<string> classNames = args.Classes.Except(args.IgnoredClasses).Intersect(from v in args.GridDefinitionClasses
                                                                                            select v.Name);
        IEnumerable<GridClass> gridClasses = from c in args.GridDefinitionClasses
                                             where classNames.Contains(c.Name)
                                             select c;
        UpdateGridParameters(args, gridClasses);
      }
    }

    protected virtual void UpdateGridParameters(ImportRenderingProcessingArgs args, IEnumerable<GridClass> gridClasses)
    {
      List<GridClass> source = gridClasses.ToList();
      Item renderingSourceItem = args.RenderingSourceItem;
      LayoutField layoutField = new LayoutField(renderingSourceItem);
      LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
      RenderingDefinition renderingByUniqueId = layoutDefinition.GetDevice(args.ImportContext.DeviceId.ToString()).GetRenderingByUniqueId(args.RenderingUniqueId.ToString());
      if (renderingByUniqueId != null && args.Attributes.ContainsKey("data-gridfield"))
      {
        string name = args.Attributes["data-gridfield"].First();
        NameValueCollection nameValueCollection = WebUtil.ParseUrlParameters(renderingByUniqueId.Parameters);
        if (source.Any() || !string.IsNullOrWhiteSpace(nameValueCollection[name]))
        {
          Item item = args.Page.Database.GetItem(args.ImportContext.DeviceId);
          Item gridDefinitionItem = ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(args.Page, item);
          #region Modified to support grid definition.
          nameValueCollection[name] = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem).InstantiateGridFieldParser().ToFieldValue(from c in source
                                                                                                                       select c.Id);
          #endregion
          renderingByUniqueId.Parameters = new UrlString(nameValueCollection).GetUrl();
          using (new EditContext(renderingSourceItem))
          {
            layoutField.Value = layoutDefinition.ToXml();
          }
        }
      }
    }
  }

}