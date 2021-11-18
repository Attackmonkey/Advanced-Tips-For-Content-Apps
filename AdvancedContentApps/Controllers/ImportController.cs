using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace AdvancedContentApps.Controllers
{
    [PluginController("24Days")]
    [IsBackOffice]
    public class ImportController : UmbracoAuthorizedJsonController
    {
        private readonly IContentService _contentService;

        public ImportController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        public IActionResult ImportArticles([FromQuery] Guid blogKey)
        {
            if (!Request.HasFormContentType && !Request.Form.Files.Any())
            {
                return BadRequest("Error: bad file uploaded.");
            }

            var uploadedFile = Request.Form.Files[0];

            using (var reader = new StreamReader(uploadedFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //get the header rows
                csv.Read();
                csv.ReadHeader();

                //read the CSV data
                while (csv.Read())
                {
                    var title = csv.GetField<string>(0);
                    var excerpt = csv.GetField<string>(1);

                    var newPage = _contentService.Create(title, blogKey, "blogPost");

                    newPage.SetValue("pageTitle", title);
                    newPage.SetValue("excerpt", excerpt);

                    _contentService.Save(newPage);
                }
            }

            return Ok("Success: the file you uploaded has been uploaded to the system and processed.");
        }
    }
}
