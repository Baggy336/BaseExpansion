using Assets.Domain.Interfaces;
using System;
using UnityEngine;

namespace Assets.Core.Events
{
    public class KeyActions<T> : IKeyAction
    {
        public Action<KeyCode, ISelectable> MethodToInvoke { get; set; }

        public KeyActions(Action<KeyCode, ISelectable> methodToInvoke)
        {
            MethodToInvoke = methodToInvoke;
        }

        public void Invoke(KeyCode key, ISelectable selectable)
        {
            MethodToInvoke.Invoke(key, selectable);
        }
    }
}
