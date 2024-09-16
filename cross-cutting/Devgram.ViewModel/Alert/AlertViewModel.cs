namespace Devgram.ViewModel.Alert;

public class AlertViewModel
{
    public string Message;
    public string Type;

    public AlertViewModel(string message, string type)
    {
        Message = message;
        Type = type;
    }
}