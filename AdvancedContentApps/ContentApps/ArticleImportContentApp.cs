using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace AdvancedContentApps.ContentApps
{
    public class ArticleImportAppComponent : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // Add our app to the site
            builder.ContentApps().Append<ArticleImportContentApp>();
        }
    }

    public class ArticleImportContentApp : IContentAppFactory
    {
        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            // Only show app on content items
            if (!(source is IContent))
                return null;

            var content = ((IContent)source);

            // Only show app on blog list pages
            if (!content.ContentType.Alias.Equals("blog"))
                return null;

            // Only show when editing
            if (content.Id == 0)
                return null;

            return new ContentApp
            {
                Alias = "articleImport",
                Name = "Article Import",
                Icon = "icon-cloud-upload",
                View = "/App_Plugins/24Days/app.html",
                Weight = 1
            };
        }
    }
}
