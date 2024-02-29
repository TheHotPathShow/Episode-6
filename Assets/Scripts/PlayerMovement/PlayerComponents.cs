using Unity.Entities;
using Unity.Mathematics;

namespace THPS.CombatSystem
{
    public struct NewPlayerTag : IComponentData {}
    public struct PlayerMoveInput : IComponentData
    {
        public float2 Value;
    }
    
    public struct PlayerMoveSpeed : IComponentData
    {
        public float Value;
    }

    public struct PlayerSprintMultiplier : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}