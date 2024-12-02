// MIT License.

using System.Configuration;
using Microsoft.Extensions.Options;

namespace System.Web;

public class SiteMapOptions
{
    public string DefaultProvider { get; set; }
    public bool? Enabled { get; set; }
    public ProviderSettingsCollection Providers { get; set; }
}

public class ConfigureSiteMapOptions : IConfigureOptions<SiteMapOptions>
{
    public void Configure(SiteMapOptions options)
    {
        options.DefaultProvider ??= "AspNetXmlSiteMapProvider";
        options.Enabled ??= true;
    }
}
