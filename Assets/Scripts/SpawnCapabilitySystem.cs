using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct SpawnCapabilitySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GamePlayingTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            foreach (var (capabilityPrefab, transform, entityTeam) in SystemAPI.Query<CapabilityPrefab, LocalTransform, EntityTeam>()
                         .WithAll<PlayerCapabilityAction>())
            {
                // If implementing a cooldown, check the cooldown timer here first before spawning
                var newCapability = ecb.Instantiate(capabilityPrefab.Value);
                ecb.SetComponent(newCapability, transform);
                ecb.SetComponent(newCapability, entityTeam);
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}