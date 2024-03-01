using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class MoveToTargetAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        public float DetectionRadius;

        public class MoveToTargetBaker : Baker<MoveToTargetAuthoring>
        {
            public override void Bake(MoveToTargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MoveToTarget
                {
                    MoveSpeed = authoring.MoveSpeed,
                    DetectionRadius = authoring.DetectionRadius
                });
            }
        }
    }
}