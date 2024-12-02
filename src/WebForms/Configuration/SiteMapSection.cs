//MIT license

using System.Configuration;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace System.Web.Configuration;

/*         <!-- Configuration for siteMap:
           Attributes:
              defaultProvider="string"  Name of provider to use by default
              enabled="[true|false]"    Determine if the feature is enabled.

            <providers>              Providers (class must inherit from SiteMapProvider)

                <add                 Add a provider
                    name="string"    Required string by which the SiteMap class identifies this provider
                    type="string"    Required string which represents the type to instantiate: type must inherit from SiteMapProvider
                    securityTrimmingEnabled="[true|false]"   Determine if security trimming is enabled. (default is false)
                    provider-specific-configuration />

                <remove              Remove a provider
                    name="string" /> Name of provider to remove

                <clear/>             Remove all providers
    -->
    <siteMap defaultProvider="AspNetXmlSiteMapProvider" enabled="true">
        <providers>
            <add name="AspNetXmlSiteMapProvider"
                 description="SiteMap provider which reads in .sitemap XML files."
                 type="System.Web.XmlSiteMapProvider, System.Web, Version=%ASSEMBLY_VERSION%, Culture=neutral, PublicKeyToken=%MICROSOFT_PUBLICKEY%"
                 siteMapFile="web.sitemap" />
        </providers>
    </siteMap>
*/

internal sealed class SiteMapSection
{
    private SiteMapProviderCollection _siteMapProviders;
    private readonly SiteMapOptions _siteMapOptions;

#pragma warning disable CA1810 // Initialize reference type static fields inline
    static SiteMapSection()
#pragma warning restore CA1810 // Initialize reference type static fields inline
    {
        // Property initialization
       // _properties = [_propDefaultProvider, _propEnabled, _propProviders];
    }

    public SiteMapSection(IOptions<SiteMapOptions> options)
    {
        _siteMapOptions = options.Value;
    }

    public string DefaultProvider
    {
        get
        {
            return _siteMapOptions.DefaultProvider;
        }

    }

    public bool Enabled
    {
        get
        {
            Debug.Assert(_siteMapOptions.Enabled != null, "_siteMapOptions.Enabled != null");
            return _siteMapOptions.Enabled.Value;
        }

    }

    public ProviderSettingsCollection Providers
    {
        get
        {
            if (_siteMapOptions.Providers == null || _siteMapOptions.Providers.Count == 0)
            {
                //TODO: let's handle better way
                var ps = new ProviderSettings() { Name = "AspNetXmlSiteMapProvider", Type = typeof(XmlSiteMapProvider).AssemblyQualifiedName };
                ps.Parameters.Add("siteMapFile", "~/Web.sitemap");
                var col = new ProviderSettingsCollection{ ps };
                _siteMapOptions.Providers = col;
            }
            return _siteMapOptions.Providers;
        }
    }

    internal SiteMapProviderCollection ProvidersInternal
    {
        get
        {
            if (_siteMapProviders == null)
            {
                lock (this)
                {
                    if (_siteMapProviders == null)
                    {
                        SiteMapProviderCollection siteMapProviders = new SiteMapProviderCollection();
                        ProvidersHelper.InstantiateProviders(Providers, siteMapProviders, typeof(SiteMapProvider));

                        _siteMapProviders = siteMapProviders;
                    }
                }
            }

            return _siteMapProviders;
        }
    }

    internal void ValidateDefaultProvider()
    {
        if (!String.IsNullOrEmpty(DefaultProvider)) // make sure the specified provider has a provider entry in the collection
        {
#if Integrate_Sitemap_WebConfig
            if (Providers[DefaultProvider] == null) {
                throw new ConfigurationErrorsException(
                    SR.GetString(SR.Config_provider_must_exist, DefaultProvider),
                    ElementInformation.Properties[_propDefaultProvider.Name].Source,
                    ElementInformation.Properties[_propDefaultProvider.Name].LineNumber);
            }
#endif
        }
    }
} // class SiteMapSection
