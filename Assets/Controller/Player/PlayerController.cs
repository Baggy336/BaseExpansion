using Assets.Controller.Selection;
using Assets.Domain.Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public PlayerBankRuntime PlayerBank;

    [SerializeField]
    public SelectionController SelectionController;
}
