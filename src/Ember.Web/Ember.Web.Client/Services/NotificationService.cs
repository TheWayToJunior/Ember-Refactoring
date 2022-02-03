using System;

namespace Ember.Web.Client
{
    public interface INotificationService
    {
        event EventHandler<INotification> Notify;

        void Send(object sender, INotification notification);
    }

    public class NotificationService : INotificationService
    {
        public event EventHandler<INotification> Notify;

        public void Send(object sender, INotification notification)
        {
            Notify?.Invoke(sender, notification);
        }
    }

    public interface INotification
    {
        public string Message { get; set; }

        public DateTime Date { get; set; }
    }

    public class MessageNotification : INotification
    {
        public MessageNotification(string message) 
        { 
            Message = message; 
            Date = DateTime.Now;
        }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
