using Capstone.Editor.Common;
using System;
using Windows.UI.Notifications;

namespace Capstone.Editor.Data
{
    public class Objective : BindableBase
    {
        public string Description { get; set; }
        public bool IsComplete { get; private set; }

        public int Count { get; set; }
        public int Total { get; set; }

        public event Action Completed;

        public Objective(string description, int total = 1)
        {
            IsComplete = false;
            Description = description;
            Count = 0;
            Total = total;
        }

        public void CompleteItem()
        {
            if (IsComplete) return;
            Count = Count + 1;
            if (Count >= Total)
            {
                IsComplete = true;
                const ToastTemplateType template = ToastTemplateType.ToastText01;
                var xml = ToastNotificationManager.GetTemplateContent(template);
                xml.GetElementsByTagName("text")[0].AppendChild(
                    xml.CreateTextNode("Objective Complete: " + Description));
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(xml));
                if (Completed != null)
                    Completed();
            }
        }
    }
}
