using Assets.Domain.Globals.Enums;
using UnityEngine;

namespace Assets.Domain.Interfaces
{
    public interface ITypeInputAction
    {
        MouseButton ClickType { get; set; }
        public void Invoke(GameObject hitObject, Component component);
    }
}
