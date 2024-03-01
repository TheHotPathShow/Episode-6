using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class DamageOnTriggerAuthoring : MonoBehaviour
    {
        public int DamageOnTrigger;
        public DamageType DamageType;
        
        public class DamageOnTriggerBaker : Baker<DamageOnTriggerAuthoring>
        {
            public override void Bake(DamageOnTriggerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamageOnTrigger
                {
                    HitPoints = authoring.DamageOnTrigger, 
                    DamageType = authoring.DamageType
                });
            }
        }
    }
}