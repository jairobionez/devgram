namespace Devgram.Infra;

public class Notifiable
{
    private List<string> _notifications;

    public IEnumerable<object> GetNotificationsAsObject => this._notifications.Select(p => p);

    public IEnumerable<string> GetNotifications => this._notifications;

    public bool HasNotification => _notifications.Any();

    public Notifiable()
    {
        _notifications = new List<string>();
    }

    public void AddNotifications(IEnumerable<string> _notifications)
    {
        this._notifications.AddRange(_notifications);
    }

    public void AddNotification(string property, string message)
    {
        this._notifications.Add($"{property}: {message}");
    }

    public void AddNotification(string message)
    {
        this._notifications.Add($"{message}");
    }

    public void Dispose()
    {
        this._notifications = new List<string>();
        GC.SuppressFinalize(this);
    }
}