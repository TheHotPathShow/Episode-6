using Unity.Burst;
using Unity.Entities;

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
}