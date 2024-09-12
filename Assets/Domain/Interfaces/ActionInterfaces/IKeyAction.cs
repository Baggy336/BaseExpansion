using UnityEngine;

namespace Assets.Domain.Interfaces
{
    public interface IKeyAction
    {
        public void Invoke(KeyCode key, ISelectable selectable);
    }
}
