using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class SpawnCapabilityTimerAuthoring : MonoBehaviour
    {
        public float Interval;

        public class SpawnCapabilityTimerBaker : Baker<SpawnCapabilityTimerAuthoring>
        {
            public override void Bake(SpawnCapabilityTimerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpawnCapabilityTimer
                {
                    Timer = 0f, 
                    Interval = authoring.Interval
                });
                AddComponent<PlayerCapabilityAction>(entity);
                SetComponentEnabled<PlayerCapabilityAction>(entity, false);
            }
        }
    }
}