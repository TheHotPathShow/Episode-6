using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace THPS.CombatSystem
{
    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct DamageOnTriggerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var damageBufferLookup = SystemAPI.GetBufferLookup<DamageBufferElement>();
            
            foreach (var (hitBuffer, damageOnTrigger, entityTeam) in SystemAPI.Query<DynamicBuffer<HitBufferElement>, DamageOnTrigger, EntityTeam>())
            {
                foreach (var hit in hitBuffer)
                {
                    if (hit.IsHandled) continue;
                    var damageBuffer = damageBufferLookup[hit.HitEntity];
                    damageBuffer.Add(new DamageBufferElement
                    {
                        HitPoints = damageOnTrigger.HitPoints,
                        DamageType = damageOnTrigger.DamageType,
                        DamageTeam = entityTeam.Value
                    });
                }
            }
        }
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
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
    
    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct DestroyOnTimerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            
            foreach (var (timer, entity) in SystemAPI.Query<RefRW<DestroyOnTimer>>().WithEntityAccess())
            {
                timer.ValueRW.Value -= deltaTime;
                if (timer.ValueRO.Value > 0f) continue;
                ecb.AddComponent<DestroyEntityTag>(entity);
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
    
    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct DestroyAfterHitsSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            var teamLookup = SystemAPI.GetComponentLookup<EntityTeam>(true);
            
            foreach (var (destroyAfterHits, hitBuffer, entity) in SystemAPI.Query<RefRW<DestroyAfterHits>, DynamicBuffer<HitBufferElement>>().WithEntityAccess())
            {
                foreach (var hit in hitBuffer)
                {
                    if (hit.IsHandled) continue;
                    if(teamLookup[entity].Value == teamLookup[hit.HitEntity].Value) continue;
                    destroyAfterHits.ValueRW.Value--;
                }

                if (destroyAfterHits.ValueRO.Value > 0) continue;
                ecb.AddComponent<DestroyEntityTag>(entity);
            }
            ecb.Playback(state.EntityManager);
        }
    }

    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct SpawnGameObjectOnHitSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            var teamLookup = SystemAPI.GetComponentLookup<EntityTeam>(true);

            foreach (var (hitBuffer, spawnGameObjectOnHit, transform, entity) in SystemAPI
                         .Query<DynamicBuffer<HitBufferElement>, SpawnGameObjectOnHit, LocalTransform>()
                         .WithEntityAccess())
            {
                foreach (var hit in hitBuffer)
                {
                    if (hit.IsHandled) continue;
                    if(teamLookup[entity].Value == teamLookup[hit.HitEntity].Value) continue;
                    var spawnPosition = transform.Position;
                    var spawnRotation = transform.Rotation;

                    Object.Instantiate(spawnGameObjectOnHit.Value, spawnPosition, spawnRotation);
                }
            }
        }
    }

    [UpdateInGroup(typeof(CapabilitySystemGroup), OrderLast = true)]
    public partial struct HandleHitBufferSystem : ISystem
    {
        private EntityQuery _hitBufferQuery;

        public void OnCreate(ref SystemState state)
        {
            _hitBufferQuery = state.GetEntityQuery(ComponentType.ReadWrite<HitBufferElement>());
        }

        public void OnUpdate(ref SystemState state)
        {
            var triggerEntities = _hitBufferQuery.ToEntityArray(state.WorldUpdateAllocator);
            var hitBufferLookup = SystemAPI.GetBufferLookup<HitBufferElement>();

            foreach (var triggerEntity in triggerEntities)
            {
                var hitBuffer = hitBufferLookup[triggerEntity];
                for (var i = 0; i < hitBuffer.Length; i++)
                {
                    var hit = hitBuffer[i];
                    hit.IsHandled = true;
                    hitBuffer[i] = hit;
                }
            }
        }
    }
}