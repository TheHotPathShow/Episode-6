using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace THPS.DamageSystem
{
    public partial struct HealthBarUISystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            // Initialize health bars for new entities that need them
            foreach (var (maxHitPoints, transform, healthBarOffset, entity) in SystemAPI.Query<MaxHitPoints, 
                         LocalTransform, HealthBarOffset>().WithNone<HealthBarUI>().WithEntityAccess())
            {
                var healthBarPrefab = SystemAPI.ManagedAPI.GetSingleton<DamageGameObjectPrefabs>().HealthBarUIPrefab;
                var spawnPosition = transform.Position + healthBarOffset.Value;
                var newHealthBar = Object.Instantiate(healthBarPrefab, spawnPosition, quaternion.identity);

                var healthBarSlider = newHealthBar.GetComponentInChildren<Slider>();
                healthBarSlider.minValue = 0;
                healthBarSlider.maxValue = maxHitPoints.Value;
                healthBarSlider.value = maxHitPoints.Value;

                ecb.AddComponent(entity, new HealthBarUI { Value = newHealthBar });
            }

            // Cleanup health bars for destroyed entities
            foreach (var (healthBarUI, entity) in SystemAPI.Query<HealthBarUI>().WithNone<HealthBarOffset>()
                         .WithEntityAccess())
            {
                Object.Destroy(healthBarUI.Value);
                ecb.RemoveComponent<HealthBarUI>(entity);
            }

            ecb.Playback(state.EntityManager);

            // Update the transform position of all health bars to be above each entity
            foreach (var (healthBarUI, transform, healthBarOffset) in SystemAPI.Query<HealthBarUI, LocalTransform, 
                         HealthBarOffset>())
            {
                healthBarUI.Value.transform.position = transform.Position + healthBarOffset.Value;
                healthBarUI.Value.transform.LookAt(Camera.main.transform);
            }
            
            // Update the values of the health bar for entities that need it updated
            foreach (var (healthBarUI, maxHitPoints, currentHitPoints, entity) in SystemAPI.Query<HealthBarUI, MaxHitPoints, 
                         CurrentHitPoints>().WithAll<UpdateHealthBarUI>().WithEntityAccess())
            {
                var healthBarSlider = healthBarUI.Value.GetComponentInChildren<Slider>();
                healthBarSlider.minValue = 0;
                healthBarSlider.maxValue = maxHitPoints.Value;
                healthBarSlider.value = currentHitPoints.Value;

                state.EntityManager.SetComponentEnabled<UpdateHealthBarUI>(entity, false);
            }
        }
    }
}