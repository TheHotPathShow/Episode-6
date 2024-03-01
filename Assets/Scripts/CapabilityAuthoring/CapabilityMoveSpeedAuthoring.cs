using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class CapabilityMoveSpeedAuthoring : MonoBehaviour
    {
        public float CapabilityMoveSpeed;

        public class CapabilityMoveSpeedBaker : Baker<CapabilityMoveSpeedAuthoring>
        {
            public override void Bake(CapabilityMoveSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CapabilityMoveSpeed { Value = authoring.CapabilityMoveSpeed });
            }
        }
    }
}