using System.ComponentModel;

namespace Capstone.Objectives
{
    public class Objective : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Completed { get; set; }
        public string Identifier { get; private set; }
        public string Description { get; set; }

        public Objective(string id, string description)
        {
            Completed = false;
            Identifier = id;
            Description = description;
        }
    }
}
