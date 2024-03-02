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
            
            foreach (var (destroyAfterHits, hitBuffer, entity) in SystemAPI.Query<RefRW<DestroyAfterHits>, 
                         DynamicBuffer<HitBufferElement>>().WithEntityAccess())
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
            var teamLookup = SystemAPI.GetComponentLookup<EntityTeam>(true);

            foreach (var (hitBuffer, spawnGameObjectOnHit, entity) in SystemAPI
                         .Query<DynamicBuffer<HitBufferElement>, SpawnGameObjectOnHit>()
                         .WithEntityAccess())
            {
                foreach (var hit in hitBuffer)
                {
                    if (hit.IsHandled) continue;
                    if(teamLookup[entity].Value == teamLookup[hit.HitEntity].Value) continue;
                    var spawnPosition = hit.Position;
                    var spawnRotation = Quaternion.Euler(hit.Normal);

                    Object.Instantiate(spawnGameObjectOnHit.Value, spawnPosition, spawnRotation);
                }
            }
        }
    }

    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct HealOnUseSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            foreach (var (healOnUse, castingEntity, entity) in SystemAPI.Query<HealOnUse, CastingEntity>().WithEntityAccess())
            {
                if (!SystemAPI.HasBuffer<DamageBufferElement>(castingEntity.Value)) continue;
                ecb.AppendToBuffer(castingEntity.Value, new DamageBufferElement
                {
                    HitPoints = healOnUse.Value,
                    DamageTeam = SystemAPI.GetComponent<EntityTeam>(castingEntity.Value).Value,
                    DamageType = DamageType.Healing
                });
                
                ecb.RemoveComponent<HealOnUse>(entity);
            }

            ecb.Playback(state.EntityManager);
        }
    }

    [UpdateInGroup(typeof(CapabilitySystemGroup), OrderLast = true)]
    public partial struct HandleHitBufferSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var hitBufferLookup = SystemAPI.GetBufferLookup<HitBufferElement>();
            var triggerEntities = SystemAPI.QueryBuilder().WithAll<HitBufferElement>().Build().ToEntityArray(state.WorldUpdateAllocator);
            
            foreach (var triggerEntity in triggerEntities)
            {
                var hitBuffer = hitBufferLookup[triggerEntity];
                for (var i = 0; i < hitBuffer.Length; i++)
                {
                    hitBuffer.ElementAt(i).IsHandled = true;
                }
            }
        }
    }
}