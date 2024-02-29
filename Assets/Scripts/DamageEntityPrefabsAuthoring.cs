using Unity.Entities;
using UnityEngine;

namespace THPS.DamageSystem
{
    public class DamageEntityPrefabsAuthoring : MonoBehaviour
    {
        public GameObject DamageOverTime;

        public class DamageEntityPrefabsBaker : Baker<DamageEntityPrefabsAuthoring>
        {
            public override void Bake(DamageEntityPrefabsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new DamageEntityPrefabs
                    {
                        DamageOverTime = GetEntity(authoring.DamageOverTime, TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}