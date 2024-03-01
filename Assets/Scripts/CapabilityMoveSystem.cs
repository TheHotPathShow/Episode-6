using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    public partial struct CapabilityMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (transform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, CapabilityMoveSpeed>())
            {
                transform.ValueRW.Position += transform.ValueRO.Forward() * moveSpeed.Value * deltaTime;
            }
        }
    }
}