namespace Sitecore.Support.XA.Foundation.DynamicPlaceholders.Pipelines.IoC
{
using Sitecore.XA.Foundation.IOC.Pipelines.IOC;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.XA.Foundation.DynamicPlaceholders.Repositories;
using Sitecore.XA.Foundation.DynamicPlaceholders.Services;
  public class RegisterDynamicPlaceholdersServices : IocProcessor
  {
    public override void Process(IocArgs args)
    {
      #region Modified Code Sitecore.Support.12543
      args.ServiceCollection.AddTransient<IDynamicPlaceholdersRepository, Repositories.DynamicPlaceholdersRepository>();
      #endregion
      args.ServiceCollection.AddTransient<IWildcardReplacer, WildcardReplacer>();
      args.ServiceCollection.AddTransient<IColumnWidthParameterService, ColumnWidthParameterService>();
    }
  }
}