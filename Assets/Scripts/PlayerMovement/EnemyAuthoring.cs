using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public class EnemyBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<NewEnemyTag>(entity);
            }
        }
    }
}