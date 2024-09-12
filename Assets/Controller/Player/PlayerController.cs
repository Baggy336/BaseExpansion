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

    [SerializeField]
    private CommandController CommandManager;

    private void Awake()
    {
        InputController.IssuedRightClick += ReactToRightClick;
        InputController.IssuedLeftClick += ReactToLeftClick;
        InputController.IssuedKeycodeCommand += CheckKeyInputOnSelections;
        InputController.IssuedDragSelection += ReactToDragSelectionBounds;
    }

    private void ReactToRightClick(Vector3 destination)
    {
        CommandManager.ProcessRightClickHit(destination);
    }

    private void ReactToLeftClick(Vector3 destination)
    {
        CommandManager.ProcessLeftClickHit(destination);
    }

    private void CheckKeyInputOnSelections(KeyCode pressedKey)
    {
        CommandManager.ProcessKeyStroke(pressedKey);
    }

    private void ReactToDragSelectionBounds(Rect bounds)
    {
        CommandManager.ProcessDragSelection(bounds);
    }

    private void OnDestroy()
    {
        InputController.IssuedRightClick -= ReactToRightClick;
        InputController.IssuedLeftClick -= ReactToLeftClick;
        InputController.IssuedKeycodeCommand -= CheckKeyInputOnSelections;
        InputController.IssuedDragSelection -= ReactToDragSelectionBounds;
    }
}
