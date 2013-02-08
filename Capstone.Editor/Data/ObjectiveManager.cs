using Capstone.Editor.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Capstone.Editor.Data
{
    public class ObjectiveManager : BindableBase
    {
        public ObservableCollection<Objective> Objectives { get; private set; }
        public ObservableCollection<Objective> CompletedObjectives { get; private set; }
        private readonly Dictionary<string, Objective> _lookup;

        public ObjectiveManager()
        {
            Objectives = new ObservableCollection<Objective>();
            CompletedObjectives = new ObservableCollection<Objective>();
            _lookup = new Dictionary<string, Objective>();
        }

        public void AddObjective(string name, string description, Action completeCallback = null, int totalToComplete = 1, object data = null)
        {
            var obj = new Objective(description, totalToComplete, data);
            obj.Name = name;
            if (completeCallback != null)
                obj.Completed += completeCallback;
            _lookup.Add(name, obj);
        }

        public void DisplayObjective(string name)
        {
            Objective o;
            if (_lookup.TryGetValue(name, out o) && !o.IsComplete && !Objectives.Contains(o))
                Objectives.Add(o);
        }

        public void CompleteObjective(string name)
        {
            Objective obj;
            if (_lookup.TryGetValue(name, out obj))
            {
                obj.CompleteItem();
                if (obj.IsComplete)
                {
                    Objectives.Remove(obj);
                    CompletedObjectives.Add(obj);
                }
            }
        }

        public void ClearObjectives()
        {
            _lookup.Clear();
            Objectives.Clear();
            CompletedObjectives.Clear();
        }

        public void ResetObjectives()
        {
            Objectives.Clear();
            CompletedObjectives.Clear();
        }

        public Objective Get(string name)
        {
            Objective result;
            if (_lookup.TryGetValue(name, out result))
                return result;
            else
                return null;
        }
    }
}
