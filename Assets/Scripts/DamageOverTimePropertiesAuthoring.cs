using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class DamageOverTimePropertiesAuthoring : MonoBehaviour
    {
        public DamageType DamageType;
        public TeamType DamageTeam;
        public int HitPointsPerHit;
        public float HitInterval;
        public float Timer;

        public class DamageOverTimePropertiesBaker : Baker<DamageOverTimePropertiesAuthoring>
        {
            public override void Bake(DamageOverTimePropertiesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamageOverTimeProperties
                {
                    DamageType = authoring.DamageType,
                    DamageTeam = authoring.DamageTeam,
                    HitPointsPerHit = authoring.HitPointsPerHit,
                    HitInterval = authoring.HitInterval,
                    Timer = authoring.Timer,
                    NextHitTime = float.MaxValue
                });
            }
        }
    }
}