using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class GameOverOnDestroyAuthoring : MonoBehaviour
    {
        public class GameOverOnDestroyBaker : Baker<GameOverOnDestroyAuthoring>
        {
            public override void Bake(GameOverOnDestroyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<GameOverOnDestroy>(entity);
            }
        }
    }
}