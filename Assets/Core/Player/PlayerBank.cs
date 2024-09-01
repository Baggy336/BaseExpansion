using Assets.Core.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.Player
{
    [CreateAssetMenu(fileName = "NewPlayerBank", menuName = "Player")]
    public class PlayerBank : ScriptableObject
    {
        [SerializeField]
        public List<ResourceBank> BankedResources;
    }
}
