using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem.CapabilityAuthoring
{
    public class CapabilityAuthoring : MonoBehaviour
    {
        public TeamType Team;
        public class CapabilityBaker : Baker<CapabilityAuthoring>
        {
            public override void Bake(CapabilityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddBuffer<HitBufferElement>(entity);
                AddComponent(entity, new EntityTeam { Value = authoring.Team });
                AddComponent<CastingEntity>(entity);
            }
        }
    }
}