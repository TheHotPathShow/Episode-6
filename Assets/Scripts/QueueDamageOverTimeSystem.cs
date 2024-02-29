using Unity.Entities;
using UnityEngine.InputSystem;

namespace THPS.DamageSystem
{
    public partial struct QueueDamageOverTimeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DamageEntityPrefabs>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (Keyboard.current[Key.Digit4].wasPressedThisFrame)
            {
                var damagePrefab = SystemAPI.GetSingleton<DamageEntityPrefabs>().DamageOverTime;
                state.EntityManager.Instantiate(damagePrefab);
            }

            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            
            foreach (var (damageOverTime, entity) in SystemAPI.Query<RefRW<DamageOverTimeProperties>>().WithEntityAccess())
            {
                damageOverTime.ValueRW.Timer -= deltaTime;
                if (damageOverTime.ValueRO.Timer <= 0f)
                {
                    ecb.AddComponent<DestroyEntityTag>(entity);
                    continue;
                }

                if (damageOverTime.ValueRO.Timer <= damageOverTime.ValueRO.NextHitTime)
                {
                    foreach (var damageBuffer in SystemAPI.Query<DynamicBuffer<DamageBufferElement>>())
                    {
                        damageBuffer.Add(new DamageBufferElement
                        {
                            DamageType = damageOverTime.ValueRO.DamageType,
                            DamageTeam = damageOverTime.ValueRO.DamageTeam,
                            HitPoints = damageOverTime.ValueRO.HitPointsPerHit
                        });
                    }

                    damageOverTime.ValueRW.NextHitTime = damageOverTime.ValueRO.Timer - damageOverTime.ValueRO.HitInterval;
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}