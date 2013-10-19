using System.Collections.Generic;
using System.Collections.ObjectModel;
using Capstone.Core;

namespace Capstone.Objectives
{
    public class ObjectiveManager : IComponent
    {
        public Entity Owner { get; set; }
        public ObservableCollection<Objective> Objectives { get; private set; }
        private readonly Dictionary<string, Objective> _lookup;

        public ObjectiveManager()
        {
            Objectives = new ObservableCollection<Objective>();
            _lookup = new Dictionary<string, Objective>();
        }

        public Objective CreateObjective(string id, string description, bool startCompleted = false)
        {
            Objective obj;
            if (!_lookup.TryGetValue(id.ToLower(), out obj))
            {
                obj = new Objective(id, description) { Completed = startCompleted };
                Objectives.Add(obj);
                _lookup.Add(id.ToLower(), obj);
            }
            return obj;
        }

        public void CompleteObjective(string id)
        {
            Objective obj;
            if (_lookup.TryGetValue(id.ToLower(), out obj))
                obj.Completed = true;
        }

        public void ClearObjectives()
        {
            _lookup.Clear();
            Objectives.Clear();
        }

        public void Initialise()
        {
        }

        public void Destroy()
        {
        }
    }
}
