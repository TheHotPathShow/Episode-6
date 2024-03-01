using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float PlayerMoveSpeed;
        public float PlayerSprintMultiplier;
        
        public class PlayerMoveSpeedBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerMoveSpeed { Value = authoring.PlayerMoveSpeed });
                AddComponent<PlayerMoveInput>(entity);
                AddComponent<NewPlayerTag>(entity);
                AddComponent(entity, new PlayerSprintMultiplier { Value = authoring.PlayerSprintMultiplier });
                SetComponentEnabled<PlayerSprintMultiplier>(entity, false);
                AddComponent<PlayerCapabilityAction>(entity);
                SetComponentEnabled<PlayerCapabilityAction>(entity, false);
            }
        }
    }
}