using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace THPS.CombatSystem
{
    public partial struct InitializePlayerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (physicsMass, entity) in SystemAPI.Query<RefRW<PhysicsMass>>()
                         .WithAny<NewPlayerTag, NewEnemyTag>().WithEntityAccess())
            {
                physicsMass.ValueRW.InverseInertia[0] = 0;
                physicsMass.ValueRW.InverseInertia[1] = 0;
                physicsMass.ValueRW.InverseInertia[2] = 0;
                
                ecb.RemoveComponent<NewPlayerTag>(entity);
            }
        }
    }
}