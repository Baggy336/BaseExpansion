using Assets.Domain.Interfaces;
using UnityEngine;

namespace Assets.Domain.Building.Commands
{
    public class ConstructionCommand : ICommand
    {
        private readonly IConstruction _construction;

        private readonly KeyCode _key;

        public ConstructionCommand(IConstruction construction, KeyCode key)
        {
            _construction = construction;
            _key = key;
        }

        public void Execute()
        {
            _construction.CheckConstructionHotkey(_key);
        }
    }
}
