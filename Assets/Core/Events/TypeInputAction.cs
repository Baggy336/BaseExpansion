using Assets.Domain.Globals.Enums;
using Assets.Domain.Interfaces;
using System;
using UnityEngine;

namespace Assets.Core.Events
{
    public class TypeInputAction<T> : ITypeInputAction
    {
        private MouseButton left;
        private Action<GameObject, ISelectable, ISelectable> handleSelectable;

        public MouseButton ClickType { get; set; }

        public Action<GameObject, Component> MethodToInvoke { get; set; }

        public TypeInputAction(MouseButton clickType, Action<GameObject, Component> methodToInvoke)
        {
            ClickType = clickType;
            MethodToInvoke = methodToInvoke;
        }

        public void Invoke(GameObject hitObject, Component component)
        {
            MethodToInvoke.Invoke(hitObject, component);
        }
    }
}
