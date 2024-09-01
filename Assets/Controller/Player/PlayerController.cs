using Assets.Controller.Player;
using Assets.Controller.Selection;
using Assets.Domain.Interfaces;
using Assets.Domain.Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public PlayerBankRuntime PlayerBank;

    [SerializeField]
    public SelectionController SelectionController;

    [SerializeField]
    public InputController InputController;

    private void Awake()
    {
        InputController.IssuedMovementCommand += MoveSelectedUnits;
    }

    private void MoveSelectedUnits(Vector3 destination)
    {
        foreach(ISelectable selectable in SelectionController.SelectedObjects)
        {
            if(selectable is IMoveable moveable)
            {
                moveable.MoveToLocation(destination);
            }
        }
    }

    private void OnDestroy()
    {
        InputController.IssuedMovementCommand -= MoveSelectedUnits;
    }
}
