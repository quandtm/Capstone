using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Media;

namespace Capstone.Core
{
    public class GameCore
    {
        private readonly Dictionary<Type, IScreen> _screens;
        private readonly Stopwatch _sw;

        public IScreen CurrentScreen { get; private set; }

        public GameCore()
        {
            _screens = new Dictionary<Type, IScreen>();
            _sw = new Stopwatch();
            CompositionTarget.Rendering += GameLoop;
        }

        private void GameLoop(object sender, object e)
        {
            double elapsed = 0;
            if (_sw.IsRunning)
            {
                _sw.Stop();
                elapsed = _sw.Elapsed.TotalSeconds;
            }

            if (CurrentScreen != null)
            {
                CurrentScreen.Update(elapsed);
                CurrentScreen.Draw(elapsed);
            }
            _sw.Restart();
        }

        public T GetScreen<T>() where T : IScreen, new()
        {
            var type = typeof(T);
            IScreen s;
            if (!_screens.TryGetValue(type, out s))
            {
                s = new T();
                _screens.Add(type, s);
                s.Initialise();
            }
            return (T)s;
        }

        public T ChangeTo<T>() where T : IScreen, new()
        {
            var s = GetScreen<T>();
            if (CurrentScreen != null)
                CurrentScreen.OnNavigatedFrom();
            CurrentScreen = s;
            CurrentScreen.OnNavigatedTo();
            return s;
        }

        public void ChangeToNull()
        {
            if (CurrentScreen == null)
                CurrentScreen.OnNavigatedFrom();
            CurrentScreen = null;
        }

        public void RemoveScreen<T>()
        {
            IScreen s;
            var type = typeof(T);
            if (_screens.TryGetValue(type, out s))
            {
                if (CurrentScreen == s)
                {
                    s.OnNavigatedFrom();
                    CurrentScreen = null;
                }
                s.Destroy();
                _screens.Remove(type);
            }
        }
    }
}
