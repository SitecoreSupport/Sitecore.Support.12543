namespace Sitecore.Support.XA.Foundation.Grid.Pipelines.DecorateRendering
{
  using Sitecore;
  using Sitecore.Data.Items;
  using Sitecore.XA.Foundation.Grid;
  using Sitecore.XA.Foundation.Grid.Model;
  using Sitecore.XA.Foundation.Grid.Parser;
  using Sitecore.XA.Foundation.IoC;
  using Sitecore.XA.Foundation.MarkupDecorator.Pipelines.DecorateRendering;
  using System.Collections.Generic;
  using System.Linq;

  public class GetGridCssClasses : RenderingDecorator
  {
    public override void Process(RenderingDecoratorArgs args)
    {
      Item gridDefinitionItem = ServiceLocator.Current.Resolve<IGridContext>().GetGridDefinitionItem(args.Rendering.Item, Context.Device);
      if (gridDefinitionItem != null)
      {
        #region Modified to support grid definition.
        IGridFieldParser gridFieldParser = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem).InstantiateGridFieldParser();
        #endregion
        string value = args.Rendering.Parameters["GridParameters"];
        List<string> list = gridFieldParser.ParseFromFieldValue(value).ToList();
        if (list.Any())
        {
          args.AddAttribute("class", list);
        }
      }
    }
  }
}