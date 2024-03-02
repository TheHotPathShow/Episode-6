using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem.CapabilityAuthoring
{
    public class CapabilityAuthoring : MonoBehaviour
    {
        public class CapabilityBaker : Baker<CapabilityAuthoring>
        {
            public override void Bake(CapabilityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddBuffer<HitBufferElement>(entity);
                AddComponent(entity, new EntityTeam { Value = TeamType.None });
                AddComponent<CastingEntity>(entity);
            }
        }
    }
}