using Devgram.ViewModel.Alert;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;

namespace Devgram.Web.TagHelpers;

public class AlertTagHelper : TagHelper
{
    private const string AlertKey = "Devgram.Alert";

    [ViewContext] public ViewContext ViewContext { get; set; }

    protected ITempDataDictionary TempData => ViewContext.TempData;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        var alertId = Guid.NewGuid();

        if (TempData[AlertKey] == null)
            TempData[AlertKey] = JsonConvert.SerializeObject(new HashSet<AlertViewModel>());

        var alerts = JsonConvert.DeserializeObject<ICollection<AlertViewModel>>(TempData[AlertKey].ToString());

        var html = string.Empty;

        foreach (var alert in alerts)
        {
            html += $@"<div id={alertId} class=""alert {alert.Type}"">
                          <span class=""alert-message"">{alert.Message}</span>
                        </div>
                    <script>
                        setTimeout(function() {{
                            var alertElement = document.getElementById('{alertId}');
                            if (alertElement) {{
                                alertElement.style.display = 'none';
                            }}
                        }}, 5000);
                    </script>";
        }

        output.Content.SetHtmlContent(html);
    }
}