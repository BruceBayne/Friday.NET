using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Friday.ASP.TagHelpers.Bootstrap.ModalTagHelper
{
    [RestrictChildren("modal-body", "modal-footer","modal-header")]
    public class ModalTagHelper : TagHelper
    {
        public string Title { get; set; }

        [HtmlAttributeName("i18n-title")]
        public string I18NTitle { get; set; }

        public string Id { get; set; }

        public bool CloseCross { get; set; } = true;

        private const string ModalStartTemplate =
            @"<div class='modal-dialog' role='document'><div class='modal-content'>";
        private const string BodyStartTemplate = @"<div class='modal-body'>";
        private const string HeaderStartTemplate = @"<div class='modal-header'>";
        private const string CloseCrossTemplate = @"<button type = 'button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var modalContext = new ModalContext();
            context.Items.Add(typeof(ModalTagHelper), modalContext);

            await output.GetChildContentAsync();




            output.TagName = "div";
            output.Attributes.SetAttribute("role", "dialog");
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("aria-labelledby", $"{context.UniqueId}Label");
            output.Attributes.SetAttribute("tabindex", "-1");
            var classNames = "modal fade";
            if (output.Attributes.ContainsName("class"))
            {
                classNames = $"{output.Attributes["class"].Value} {classNames}";
            }
            output.Attributes.SetAttribute("class", classNames);

            output.Content.AppendHtml(ModalStartTemplate);
            output.Content.AppendHtml(HeaderStartTemplate);
            if (CloseCross) output.Content.AppendHtml(CloseCrossTemplate);

            if (modalContext.Header == null)
            {
                var headerTemplate = "";
                if (I18NTitle != null) headerTemplate += $@"<h4 class='modal-title' id='{context.UniqueId}Label' data-i18n='{I18NTitle}'>{Title}</h4>";
                else headerTemplate += $@"<h4 class='modal-title' id='{context.UniqueId}Label'>{Title}</h4>";
                output.Content.AppendHtml(headerTemplate);
            }
            else
            {
                output.Content.AppendHtml(modalContext.Header);
            }
            output.Content.AppendHtml("</div>");

            output.Content.AppendHtml(BodyStartTemplate);
            if (modalContext.Body != null)
            {
                output.Content.AppendHtml(modalContext.Body);
            }
            output.Content.AppendHtml("</div>");
            if (modalContext.Footer != null)
            {
                output.Content.AppendHtml("<div class='modal-footer'>");
                output.Content.AppendHtml(modalContext.Footer);
                output.Content.AppendHtml("</div>");
            }

            output.Content.AppendHtml("</div></div>");
        }
    }
}
