﻿using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;

namespace WebOptimizer.TagHelpersDynamic
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;link&gt; elements that supports fallback href paths.
    /// </summary>
    /// <remarks>
    /// The tag helper won't process for cases with just the 'href' attribute.
    /// </remarks>
    [HtmlTargetElement(TagName, Attributes = HrefIncludeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = HrefExcludeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = FallbackHrefAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = FallbackHrefIncludeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = FallbackHrefExcludeAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = FallbackTestClassAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = FallbackTestPropertyAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = FallbackTestValueAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = AppendVersionAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement(TagName, Attributes = BundleKeyName)]
    [HtmlTargetElement(TagName, Attributes = BundleDestinationKeyName)]
    public class LinkTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.LinkTagHelper
    {
        private const string TagName = "link";

        private const string HrefIncludeAttributeName = "asp-href-include";
        private const string HrefExcludeAttributeName = "asp-href-exclude";
        private const string FallbackHrefAttributeName = "asp-fallback-href";
        private const string FallbackHrefIncludeAttributeName = "asp-fallback-href-include";
        private const string FallbackHrefExcludeAttributeName = "asp-fallback-href-exclude";
        private const string FallbackTestClassAttributeName = "asp-fallback-test-class";
        private const string FallbackTestPropertyAttributeName = "asp-fallback-test-property";
        private const string FallbackTestValueAttributeName = "asp-fallback-test-value";
        private const string AppendVersionAttributeName = "asp-append-version";

        private const string BundleDestinationKeyName = "asp-bundle-dest-key";
        private const string BundleKeyName = "asp-bundle-key";

        /// <summary>
        ///
        /// </summary>
        [HtmlAttributeName(BundleKeyName)]
        public string BundleKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        [HtmlAttributeName(BundleDestinationKeyName)]
        public string BundleDestinationKey { get; set; }

        /// <summary>
        /// Creates a new <see cref="LinkTagHelper"/>.
        /// </summary>
        /// <param name="hostingEnvironment">The <see cref="IWebHostEnvironment"/>.</param>
        /// <param name="cache">The <see cref="IMemoryCache"/>.</param>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/>.</param>
        /// <param name="javaScriptEncoder">The <see cref="JavaScriptEncoder"/>.</param>
        /// <param name="urlHelperFactory">The <see cref="IUrlHelperFactory"/>.</param>
        public LinkTagHelper(
            IWebHostEnvironment hostingEnvironment,
            TagHelperMemoryCacheProvider cache,
            IFileVersionProvider fileVersionProvider,
            HtmlEncoder htmlEncoder,
            JavaScriptEncoder javaScriptEncoder,
            IUrlHelperFactory urlHelperFactory, IServiceProvider serviceProvider)
            : base(hostingEnvironment, cache, fileVersionProvider, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }

        private IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (Helpers
                    .HandleBundle(
                        Helpers.CreateCssAsset,
                        _serviceProvider,
                        HostingEnvironment,
                        output,
                        ViewContext.HttpContext,
                        "href", Href, BundleKey, BundleDestinationKey) == false)
            {
                base.Process(context, output);
            }
        }
    }
}
