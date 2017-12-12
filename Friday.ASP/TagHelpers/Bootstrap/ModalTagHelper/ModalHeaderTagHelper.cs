using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Friday.ASP.TagHelpers.Bootstrap.ModalTagHelper
{
    [HtmlTargetElement("modal-header", ParentTag = "modal")]
    public class ModalHeaderTagHelper : TagHelper
    {

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var headerContent = new DefaultTagHelperContent();
            headerContent.AppendHtml(childContent);
            var modalContext = (ModalContext)context.Items[typeof(ModalTagHelper)];
            modalContext.Header = headerContent;
            output.SuppressOutput();
        }
    }
}