using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.XA.Feature.PageStructure.Repositories;
using Sitecore.XA.Foundation.DynamicPlaceholders.Repositories;

namespace Sitecore.Support
{
  public class RegisterDependencies : IServicesConfigurator
  {
    public void Configure(IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient<IDynamicPlaceholdersRepository, Sitecore.Support.XA.Foundation.DynamicPlaceholders.Repositories.DynamicPlaceholdersRepository>();
      serviceCollection.AddTransient<ISplitterRepository, Sitecore.Support.XA.Feature.PageStructure.Repositories.SplitterRepository>();
    }
  }
}
