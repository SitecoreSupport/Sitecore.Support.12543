using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.DynamicPlaceholders.Repositories;

namespace Support
{
  public class RegisterDependencies : IServicesConfigurator
  {
    public void Configure(IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient<IDynamicPlaceholdersRepository, Sitecore.Support.XA.Foundation.DynamicPlaceholders.Repositories.DynamicPlaceholdersRepository>();
    }
  }
}
