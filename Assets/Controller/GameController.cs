using Assets.Controller.Player.Events;
using Assets.Core.Building;
using Assets.Domain.Building.Economy;
using Assets.Domain.Interfaces;
using Assets.Domain.Player;
using Assets.Domain.Unit.PlayerUnits;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Controller
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField]
        public List<PlayerController> PlayersToRegister;

        private List<PlayerDomain> Players = new List<PlayerDomain>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            foreach(PlayerController player in PlayersToRegister)
            {
                RegisterPlayer(player);
            }
        }

        public void RegisterPlayer(PlayerController player)
        {
            if(Players.Where(x => x.Player == player).Any() == false)
            {
                PlayerEvents playerEventHandler = new PlayerEvents();
                playerEventHandler.OnSelectableCreated += RegisterSelectable;
                playerEventHandler.ResourceCollectorNeedsDepot += FindNearestResourceDepot;
                playerEventHandler.OnBuildingPlacementStarted += BeginBuildingPlacementMode;
                PlayerDomain playerDomain = new PlayerDomain()
                {
                    Player = player,
                    PlayerEventHandler = playerEventHandler
                };
                Players.Add(playerDomain);
            }
        }

        private void RegisterSelectable(ISelectable selectable, PlayerController player)
        {
            player.SelectionController.AddSelectableToList(selectable);
            selectable.SetPlayerReference(player);
        }

        private ResourceDepot FindNearestResourceDepot(IResourceCollector worker, Vector3 workerPosition, PlayerController player)
        {
            return player.SelectionController.FindNearestResourceDepot(workerPosition);
        }

        private void BeginBuildingPlacementMode(Worker worker, PlayerController player, BuildingConstructionCost building)
        {
            player.BuildingPlacementManager.StartBuildingPlacement(worker, building);
        }

        public PlayerEvents GetPlayerEventSystem(PlayerController player)
        {
            return Players.Where(x => x.Player == player).Select(x => x.PlayerEventHandler).First();
        }
    }
}
