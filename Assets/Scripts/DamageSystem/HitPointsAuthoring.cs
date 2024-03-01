using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class HitPointsAuthoring : MonoBehaviour
    {
        public int MaxHitPoints;
        public TeamType Team;
        
        public bool MultiplyPhysicalDamage;
        public float PhysicalDamageMultiplier;

        public bool MultiplyMagicDamage;
        public float MagicDamageMultiplier;

        public bool MultiplyPoisonDamage;
        public float PoisonDamageMultiplier;

        public Vector3 HealthBarUIOffset;

        public float FlashTime;

        public bool ShouldIgnoreDamageMultiplication;
        
        
        private class HitPointsBaker : Baker<HitPointsAuthoring>
        {
            public override void Bake(HitPointsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MaxHitPoints { Value = authoring.MaxHitPoints });
                AddComponent(entity, new CurrentHitPoints { Value = authoring.MaxHitPoints });
                AddComponent(entity, new EntityTeam { Value = authoring.Team });
                AddBuffer<DamageBufferElement>(entity);

                if (authoring.MultiplyPhysicalDamage)
                {
                    AddComponent(entity, new PhysicalDamageMultiplier { Value = authoring.PhysicalDamageMultiplier });
                }

                if (authoring.MultiplyMagicDamage)
                {
                    AddComponent(entity, new MagicDamageMultiplier { Value = authoring.MagicDamageMultiplier });
                }

                if (authoring.MultiplyPoisonDamage)
                {
                    AddComponent(entity, new PoisonDamageMultiplier { Value = authoring.PoisonDamageMultiplier });
                }

                AddComponent(entity, new HealthBarOffset { Value = authoring.HealthBarUIOffset });
                AddComponent<UpdateHealthBarUI>(entity);
                SetComponentEnabled<UpdateHealthBarUI>(entity, false);
                
                AddComponent(entity, new DamageFlashTimer { FlashTime = authoring.FlashTime });
                SetComponentEnabled<DamageFlashTimer>(entity, false);

                AddComponent(entity, new URPMaterialPropertyBaseColor { Value = new float4(1, 1, 1, 1) });
                
                if (authoring.ShouldIgnoreDamageMultiplication)
                {
                    AddComponent<IgnoreDamageMultiplicationTag>(entity);
                }
            }
        }
    }
}