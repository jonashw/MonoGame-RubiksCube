using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MonoGameRubiksCube
{
    public class KeyboardEvents
    {
        private HashSet<Keys> _lastUpdate = new HashSet<Keys>();
        private HashSet<Keys> _thisUpdate = new HashSet<Keys>();
        private readonly HashSet<Keys> _toBroadcast = new HashSet<Keys>();
        private readonly Dictionary<Keys, List<Action>> _observers = new Dictionary<Keys, List<Action>>();

        public void OnPress(Keys key, Action observer)
        {
            if (!_observers.ContainsKey(key))
            {
                _observers[key] = new List<Action>();
            }
            _observers[key].Add(observer);
        }

        public void Update(KeyboardState state)
        {
            foreach (var key in state.GetPressedKeys())
            {
                _thisUpdate.Add(key);
                _toBroadcast.Add(key);
            }
            _toBroadcast.ExceptWith(_lastUpdate);
            foreach (var newlyPressedKey in _toBroadcast.Where(_observers.ContainsKey))
            {
                if (!_observers.ContainsKey(newlyPressedKey))
                {
                    continue;
                }
                foreach (var observer in _observers[newlyPressedKey])
                {
                    observer();
                }
            }
            _toBroadcast.Clear();
            _lastUpdate.Clear();
            /* To save on Garbage Collection, we re-use all HashSets.
             * "This Update" will be known as "Last Update" on the next update,
             * hence the swap. */
            var temp = _lastUpdate;
            _lastUpdate = _thisUpdate;
            _thisUpdate = temp;
        }
    }
}