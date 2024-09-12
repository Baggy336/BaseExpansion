using Assets.Controller.Player.Events;

namespace Assets.Domain.Player
{
    public class PlayerDomain
    {
        public PlayerController Player { get; set; }

        public PlayerEvents PlayerEventHandler { get; set; }
    }
}
