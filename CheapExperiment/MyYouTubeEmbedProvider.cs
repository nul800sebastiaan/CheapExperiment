using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Media.EmbedProviders;
using Umbraco.Cms.Core.Serialization;

namespace CheapExperiment;

public class MyYouTubeEmbedProvider : OEmbedProviderBase
{
    public MyYouTubeEmbedProvider(IJsonSerializer jsonSerializer) : base(jsonSerializer)
    {
    }

    public override string ApiEndpoint => "https://www.youtube.com/oembed";

    public override string[] UrlSchemeRegex => new[] { @"youtu.be/.*", @"youtube.com/watch.*", @"youtube.com/shorts/.*" };
    
    public override Dictionary<string, string> RequestParams => new()
    {
        // ApiUrl/?format=json
        { "format", "json" },
    };
    
    public override string? GetMarkup(string url, int maxWidth = 0, int maxHeight = 0)
    {
        var requestUrl = base.GetEmbedProviderUrl(url, maxWidth, maxHeight);
        var oembed = base.GetJsonResponse<OEmbedResponse>(requestUrl);

        return oembed?.GetHtml();
    }
}

public class RegisterEmbedProvidersComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
        => builder.EmbedProviders().Remove<YouTube>().Append<MyYouTubeEmbedProvider>();
}