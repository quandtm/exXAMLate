using System.Composition.Convention;
using System.Composition.Hosting;
using System.Reflection;
using Okra;

namespace ExXAMLate
{
    public class AppBootstrapper : OkraBootstrapper
    {
        protected override ContainerConfiguration GetContainerConfiguration()
        {
            var conventionBuilder = new ConventionBuilder();

            conventionBuilder.ForTypesMatching(type => type.FullName.EndsWith("View"))
                .Export(builder => builder.AsContractType<object>()
                                       .AsContractName("OkraPage")
                                       .AddMetadata("PageName", type => type.Name.Substring(0, type.Name.Length - 4)));

            conventionBuilder.ForTypesMatching(type => type.FullName.EndsWith("ViewModel"))
                .Export(builder => builder.AsContractType<object>()
                                       .AsContractName("OkraViewModel")
                                       .AddMetadata("PageName", type => type.Name.Substring(0, type.Name.Length - 9)));


           
            var ui = typeof(AppBootstrapper).GetTypeInfo().Assembly;
            return GetOkraContainerConfiguration().WithAssemblies(new[] { ui }, conventionBuilder);
        }
    }
}