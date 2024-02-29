using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class CameraTransformReference : IComponentData
    {
        public Transform CameraTarget;
        public Transform Camera;
    }
}