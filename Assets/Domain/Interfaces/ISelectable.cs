namespace Assets.Domain.Interfaces
{
    public interface ISelectable
    {
        public PlayerController OwnerPlayer { get; set; }
        public void SetPlayerReference(PlayerController player);
    }
}
