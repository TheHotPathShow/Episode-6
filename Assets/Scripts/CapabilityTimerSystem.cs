using Unity.Entities;

namespace THPS.CombatSystem
{
    public partial struct CapabilityTimerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GamePlayingTag>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach (var (capabilityTimer, capabilityAction) in SystemAPI.Query<RefRW<SpawnCapabilityTimer>, EnabledRefRW<PlayerCapabilityAction>>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
            {
                capabilityTimer.ValueRW.Timer -= deltaTime;
                if (capabilityTimer.ValueRO.Timer > 0f)
                {
                    capabilityAction.ValueRW = false;
                    continue;
                };
                capabilityAction.ValueRW = true;
                capabilityTimer.ValueRW.Timer = capabilityTimer.ValueRO.Interval;
            }
        }
    }
}