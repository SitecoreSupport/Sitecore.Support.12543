using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Grid;
using Sitecore.XA.Foundation.Grid.Parser;
using Sitecore.XA.Foundation.MarkupDecorator.Pipelines.DecorateRendering;

namespace Sitecore.Support.XA.Foundation.Grid.Pipelines.DecorateRendering
{
  public class GetGridCssClasses : RenderingDecorator
  {
    public override void Process(RenderingDecoratorArgs args)
    {
      var gridDefinitionItem = ServiceLocator.ServiceProvider.GetService<IGridContext>().GetGridDefinitionItem(args.Rendering.Item, Context.Device);
      if (gridDefinitionItem != null)
      {
        var gridDefinition = new Sitecore.Support.XA.Foundation.Grid.Model.GridDefinition(gridDefinitionItem);
        IGridFieldParser parser = gridDefinition.InstantiateGridFieldParser();

        var rawFieldValue = args.Rendering.Parameters[Sitecore.XA.Foundation.Grid.Constants.GridParametersFieldName];
        var classes = parser.ParseFromFieldValue(rawFieldValue).ToList();
        if (classes.Any())
        {
          args.AddAttribute(Sitecore.XA.Foundation.MarkupDecorator.Constants.AttributeNames.Class, classes);
        }
      }
    }
  }
}