using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class SpawnGameObjectOnDestroyAuthoring : MonoBehaviour
    {
        public GameObject SpawnGameObjectOnDestroy;

        public class SpawnGameObjectOnDestroyBaker : Baker<SpawnGameObjectOnDestroyAuthoring>
        {
            public override void Bake(SpawnGameObjectOnDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new SpawnGameObjectOnDestroy { Value = authoring.SpawnGameObjectOnDestroy });
            }
        }
    }
}