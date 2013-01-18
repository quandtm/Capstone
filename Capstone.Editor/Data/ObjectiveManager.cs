using Capstone.Editor.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Capstone.Editor.Data
{
    public class ObjectiveManager : BindableBase
    {
        public ObservableCollection<Objective> Objectives { get; private set; }
        private readonly Dictionary<string, Objective> _lookup;
        public int TotalScore { get; private set; }

        public ObjectiveManager()
        {
            Objectives = new ObservableCollection<Objective>();
            _lookup = new Dictionary<string, Objective>();
        }

        public void AddObjective(string name, string description, int score, Action completeCallback = null, int totalToComplete = 1)
        {
            var obj = new Objective(description, totalToComplete);
            obj.Score = score;
            if (completeCallback != null)
                obj.Completed += completeCallback;
            _lookup.Add(name, obj);
            Objectives.Add(obj);
        }

        public void CompleteObjective(string name)
        {
            Objective obj;
            if (_lookup.TryGetValue(name, out obj))
            {
                obj.CompleteItem();
                TotalScore = TotalScore + obj.Score;
            }
        }
    }
}
