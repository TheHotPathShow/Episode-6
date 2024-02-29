using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace THPS.CombatSystem
{
    public partial struct ApplyUnscaledDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            
            foreach (var (currentHitPoints, maxHitPoints, damageBuffer, team, damageReceivingEntity) in SystemAPI
                         .Query<RefRW<CurrentHitPoints>, MaxHitPoints, DynamicBuffer<DamageBufferElement>, 
                             EntityTeam>()
                         .WithEntityAccess()
                         .WithAll<IgnoreDamageMultiplicationTag>())
            {
                var damageHitPoints = 0;
                var healingHitPoints = 0;
                
                foreach (var damageElement in damageBuffer)
                {
                    if (damageElement.DamageType == DamageType.None) continue;
                    if (damageElement.DamageType == DamageType.Healing)
                    {
                        if (team.Value != damageElement.DamageTeam) continue;
                        healingHitPoints += damageElement.HitPoints;
                        continue;
                    }


                    if (team.Value == damageElement.DamageTeam) continue;
                    damageHitPoints += damageElement.HitPoints;
                }
                
                damageBuffer.Clear();
                
                var totalHitPoints = 0 - damageHitPoints + healingHitPoints;
                if (totalHitPoints == 0) continue;
                
                currentHitPoints.ValueRW.Value += totalHitPoints;
                currentHitPoints.ValueRW.Value = math.min(currentHitPoints.ValueRO.Value, maxHitPoints.Value);
                
                if (currentHitPoints.ValueRO.Value <= 0)
                {
                    ecb.AddComponent<DestroyEntityTag>(damageReceivingEntity);
                }
                
                if (SystemAPI.ManagedAPI.HasComponent<HealthBarUI>(damageReceivingEntity))
                {
                    SystemAPI.SetComponentEnabled<UpdateHealthBarUI>(damageReceivingEntity, true);
                }
                
                if (totalHitPoints > 0) continue;
                
                if (SystemAPI.HasComponent<DamageFlashTimer>(damageReceivingEntity))
                {
                    var timer = SystemAPI.GetComponent<DamageFlashTimer>(damageReceivingEntity);
                    timer.CurrentTime = timer.FlashTime;
                    SystemAPI.SetComponent(damageReceivingEntity, timer);
                    SystemAPI.SetComponentEnabled<DamageFlashTimer>(damageReceivingEntity, true);
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}