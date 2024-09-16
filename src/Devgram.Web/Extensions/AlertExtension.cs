using Devgram.ViewModel.Alert;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Devgram.Web.Extensions;

public static class AlertExtension
{
    private const string AlertKey = "Devgram.Alert";

    public static void AddAlertSuccess(this Controller controller, string message)
    {
        var alerts = GetAlerts(controller);

        alerts.Add(new AlertViewModel(message, "alert-success"));

        controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
    }

    public static void AddAlertInfo(this Controller controller, string message)
    {
        var alerts = GetAlerts(controller);

        alerts.Add(new AlertViewModel(message, "alert-info"));

        controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
    }

    public static void AddAlertWarning(this Controller controller, string message)
    {
        var alerts = GetAlerts(controller);

        alerts.Add(new AlertViewModel(message, "alert-warning"));

        controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
    }

    public static void AddAlertDanger(this Controller controller, string message)
    {
        var alerts = GetAlerts(controller);

        alerts.Add(new AlertViewModel(message, "alert-danger"));

        controller.TempData[AlertKey] = JsonConvert.SerializeObject(alerts);
    }

    private static ICollection<AlertViewModel> GetAlerts(Controller controller)
    {
        if (controller.TempData[AlertKey] == null)
            controller.TempData[AlertKey] = JsonConvert.SerializeObject(new HashSet<AlertViewModel>());

        ICollection<AlertViewModel> alerts = JsonConvert.DeserializeObject<ICollection<AlertViewModel>>(controller.TempData[AlertKey].ToString());

        if (alerts == null)
        {
            alerts = new HashSet<AlertViewModel>();
        }
        return alerts;
    }
}