using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class HealOnUseAuthoring : MonoBehaviour
    {
        public int HealOnUse;

        public class HealOnUseBaker : Baker<HealOnUseAuthoring>
        {
            public override void Bake(HealOnUseAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealOnUse { Value = authoring.HealOnUse });
            }
        }
    }
}