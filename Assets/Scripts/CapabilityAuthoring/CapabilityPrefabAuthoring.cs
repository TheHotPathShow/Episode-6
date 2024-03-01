using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class CapabilityPrefabAuthoring : MonoBehaviour
    {
        public GameObject CapabilityPrefab;

        public class CapabilityPrefabBaker : Baker<CapabilityPrefabAuthoring>
        {
            public override void Bake(CapabilityPrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CapabilityPrefab
                {
                    Value = GetEntity(authoring.CapabilityPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}