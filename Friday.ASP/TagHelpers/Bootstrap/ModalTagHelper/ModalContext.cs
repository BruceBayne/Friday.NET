using Microsoft.AspNetCore.Html;

namespace Friday.ASP.TagHelpers.Bootstrap.ModalTagHelper
{
    public class ModalContext
    {
        public IHtmlContent Body { get; set; }
        public IHtmlContent Footer { get; set; }
        public IHtmlContent Header { get; set; }
    }
}