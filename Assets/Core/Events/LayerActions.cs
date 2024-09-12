using System;
using UnityEngine;

namespace Assets.Core.Events
{
    public class LayerActions
    {
        public LayerMask InteractionLayer { get; set; }
        public Action<Vector3> MethodToInvoke { get; set; }

        public LayerActions(LayerMask interactionLayer, Action<Vector3> methodToInvoke)
        {
            InteractionLayer = interactionLayer;
            MethodToInvoke = methodToInvoke;
        }
    }
}
