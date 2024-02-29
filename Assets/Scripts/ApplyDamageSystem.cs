using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace THPS.CombatSystem
{
    public partial struct ApplyDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            
            foreach (var (currentHitPoints, maxHitPoints, damageBuffer, entityTeam, damageReceivingEntity) in SystemAPI
                         .Query<RefRW<CurrentHitPoints>, MaxHitPoints, DynamicBuffer<DamageBufferElement>, EntityTeam>()
                         .WithEntityAccess()
                         .WithOptions(EntityQueryOptions.FilterWriteGroup))
            {
                var damageHitPoints = 0;
                var healingHitPoints = 0;
                
                foreach (var damageElement in damageBuffer)
                {
                    switch (damageElement.DamageType)
                    {
                        case DamageType.Physical:
                            
                            // Ignore friendly fire
                            if (entityTeam.Value == damageElement.DamageTeam) continue;
                            var physicalHitPoints = damageElement.HitPoints;
                            if (SystemAPI.HasComponent<PhysicalDamageMultiplier>(damageReceivingEntity))
                            {
                                var physicalDamageMultiplier = SystemAPI.GetComponent<PhysicalDamageMultiplier>(damageReceivingEntity).Value;
                                physicalHitPoints = (int)(physicalHitPoints * physicalDamageMultiplier);
                            }
                            damageHitPoints += physicalHitPoints;
                            break;
                        
                        case DamageType.Magic:
                            
                            // Ignore friendly fire
                            if (entityTeam.Value == damageElement.DamageTeam) continue;
                            var magicHitPoints = damageElement.HitPoints;
                            if (SystemAPI.HasComponent<MagicDamageMultiplier>(damageReceivingEntity))
                            {
                                var magicDamageMultiplier = SystemAPI.GetComponent<MagicDamageMultiplier>(damageReceivingEntity).Value;
                                magicHitPoints = (int)(magicHitPoints * magicDamageMultiplier);
                            }
                            damageHitPoints += magicHitPoints;
                            break;
                        
                        case DamageType.Poison:
                            
                            // Ignore friendly fire
                            if (entityTeam.Value == damageElement.DamageTeam) continue;
                            var poisonHitPoints = damageElement.HitPoints;
                            if (SystemAPI.HasComponent<PoisonDamageMultiplier>(damageReceivingEntity))
                            {
                                var poisonDamageMultiplier = SystemAPI.GetComponent<PoisonDamageMultiplier>(damageReceivingEntity).Value;
                                poisonHitPoints = (int)(poisonHitPoints * poisonDamageMultiplier);
                            }
                            damageHitPoints += poisonHitPoints;
                            break;
                        
                        case DamageType.Healing:
                            
                            // Only apply healing if coming from the same team
                            if (entityTeam.Value != damageElement.DamageTeam) continue;
                            healingHitPoints += damageElement.HitPoints;
                            break;
                        
                        default:
                            Debug.LogWarning($"Warning: attempting to apply undefined damage type: {damageElement.DamageType}");
                            break;
                    }
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