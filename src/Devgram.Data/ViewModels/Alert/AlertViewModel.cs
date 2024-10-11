using System.Text.Json.Serialization;

namespace Devgram.Data.ViewModels;

public class AlertViewModel
{
    public string Message;
    public string Type;

    [JsonConstructor]
    public AlertViewModel()
    {
        
    }
    
   
    public AlertViewModel(string message, string type)
    {
        Message = message;
        Type = type;
    }
}