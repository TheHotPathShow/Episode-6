using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace THPS.DamageSystem
{
    public partial struct DamageFlashEffectSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (color, damageFlashTimer) in SystemAPI.Query<RefRW<URPMaterialPropertyBaseColor>, RefRW<DamageFlashTimer>>())
            {
                damageFlashTimer.ValueRW.CurrentTime -= deltaTime;
                if (damageFlashTimer.ValueRO.CurrentTime <= 0f)
                {
                    color.ValueRW.Value = new float4(1, 1, 1, 1);
                    continue;
                }

                var percentage = damageFlashTimer.ValueRO.CurrentTime / damageFlashTimer.ValueRO.FlashTime;
                var value = 1 - percentage;

                color.ValueRW.Value = new float4(1, value, value, 1);
            }
        }
    }
}