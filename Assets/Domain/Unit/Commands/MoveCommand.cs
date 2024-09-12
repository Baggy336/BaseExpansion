using Assets.Domain.Interfaces;
using UnityEngine;

namespace Assets.Domain.Unit.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly Vector3 _position;

        private readonly IMoveable _moveable;

        public MoveCommand(Vector3 position, IMoveable moveable)
        {
            _position = position;
            _moveable = moveable;
        }

        public void Execute()
        {
            _moveable.MoveToLocation(_position);
        }
    }
}
