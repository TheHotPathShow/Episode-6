using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class DestroyAfterHitsAuthoring : MonoBehaviour
    {
        public int DestroyAfterHits;

        public class DestroyAfterHitsBaker : Baker<DestroyAfterHitsAuthoring>
        {
            public override void Bake(DestroyAfterHitsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DestroyAfterHits { Value = authoring.DestroyAfterHits });
            }
        }
    }
}