using Capstone.Editor.Common;
using System;

namespace Capstone.Editor.Data
{
    public class Objective : BindableBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; private set; }
        public object Data { get; set; }

        public int Count { get; set; }
        public int Total { get; set; }

        public event Action Completed;

        public Objective(string description, int total = 1, object data = null)
        {
            IsComplete = false;
            Description = description;
            Count = 0;
            Total = total;
            Data = data;
        }

        public void CompleteItem()
        {
            if (IsComplete) return;
            Count = Count + 1;
            if (Count >= Total)
            {
                IsComplete = true;
                if (Completed != null)
                    Completed();
            }
        }

        public void Reset()
        {
            IsComplete = false;
            Count = 0;
        }
    }
}
