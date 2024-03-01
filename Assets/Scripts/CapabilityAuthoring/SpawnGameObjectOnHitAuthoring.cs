using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class SpawnGameObjectOnHitAuthoring : MonoBehaviour
    {
        public GameObject SpawnGameObjectOnHit;

        public class SpawnGameObjectOnHitBaker : Baker<SpawnGameObjectOnHitAuthoring>
        {
            public override void Bake(SpawnGameObjectOnHitAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new SpawnGameObjectOnHit { Value = authoring.SpawnGameObjectOnHit });
            }
        }
    }
}