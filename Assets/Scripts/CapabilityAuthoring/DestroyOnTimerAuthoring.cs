using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class DestroyOnTimerAuthoring : MonoBehaviour
    {
        public float DestroyOnTimer;

        public class DestroyOnTimerBaker : Baker<DestroyOnTimerAuthoring>
        {
            public override void Bake(DestroyOnTimerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DestroyOnTimer { Value = authoring.DestroyOnTimer });
            }
        }
    }
}