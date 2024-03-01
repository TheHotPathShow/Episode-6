using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class SpawnEntityOnDestroyAuthoring : MonoBehaviour
    {
        public GameObject SpawnEntityOnDestroy;

        public class SpawnEntityOnDestroyBaker : Baker<SpawnEntityOnDestroyAuthoring>
        {
            public override void Bake(SpawnEntityOnDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new SpawnEntityOnDestroy
                    {
                        Value = GetEntity(authoring.SpawnEntityOnDestroy, TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}