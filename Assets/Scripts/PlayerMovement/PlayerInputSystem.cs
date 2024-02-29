using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public partial struct PlayerInputSystem : ISystem
    {
        public class Singleton : IComponentData
        {
            public TurboInputActions Value;
        }
        
        public void OnCreate(ref SystemState state)
        {
            var inputActions = new TurboInputActions();
            inputActions.Enable();

            state.EntityManager.AddComponentObject(state.SystemHandle, new Singleton
            {
                Value = inputActions
            });
        }

        public void OnUpdate(ref SystemState state)
        {
            var inputActionMap = SystemAPI.ManagedAPI.GetComponent<Singleton>(state.SystemHandle).Value.DefaultMap;
            var moveInput = inputActionMap.PlayerMove.ReadValue<Vector2>();
            var sprintInput = inputActionMap.PlayerSprint.IsPressed();
            
            foreach (var (playerMoveInput, playerSprintMultiplier, entity) in SystemAPI.Query<RefRW<PlayerMoveInput>, 
                             EnabledRefRW<PlayerSprintMultiplier>>()
                         .WithPresent<PlayerSprintMultiplier>().WithEntityAccess())
            {
                playerMoveInput.ValueRW.Value = moveInput;
                
                // Set enabled state of a component
                playerSprintMultiplier.ValueRW = sprintInput;
            }
        }
    }
}