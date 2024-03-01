using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    public partial struct MoveCapabilityToTargetSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (transform, moveToTarget) in SystemAPI.Query<RefRW<LocalTransform>, MoveToTarget>())
            {
                if (transformLookup.TryGetComponent(moveToTarget.Target, out var targetPosition))
                {
                    var toTarget = targetPosition.Position - transform.ValueRO.Position;
                    transform.ValueRW.Rotation = quaternion.LookRotationSafe(toTarget, math.up());
                }

                transform.ValueRW.Position += transform.ValueRO.Forward() * moveToTarget.MoveSpeed * deltaTime;
            }
        }
    }
}