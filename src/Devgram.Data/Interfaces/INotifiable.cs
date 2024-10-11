using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devgram.Data.Interfaces
{
    public interface INotifiable
    {
        IEnumerable<object> GetNotificationsAsObject { get; }
        IEnumerable<string> GetNotifications { get; }
        bool HasNotification { get; }

        void AddNotifications(IEnumerable<string> _notifications);
        void AddNotification(string property, string message);
        void AddNotification(string message);
    }
}
