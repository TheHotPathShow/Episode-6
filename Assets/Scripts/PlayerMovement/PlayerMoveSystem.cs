using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    public partial struct PlayerMoveSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CameraTransformReference>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var cameraForward = SystemAPI.ManagedAPI.GetSingleton<CameraTransformReference>().Camera.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            var cameraRight = SystemAPI.ManagedAPI.GetSingleton<CameraTransformReference>().Camera.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            var playerMoveJob = new PlayerMoveJob { CameraForward = cameraForward, CameraRight = cameraRight, DeltaTime = deltaTime };
            state.Dependency = playerMoveJob.ScheduleParallel(state.Dependency);
        }
    }
    
    [BurstCompile]
    [WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    public partial struct PlayerMoveJob : IJobEntity
    {
        public float3 CameraForward;
        public float3 CameraRight;
        public float DeltaTime;

        private void Execute(ref LocalTransform transform, in PlayerMoveInput moveInput, in PlayerMoveSpeed moveSpeed, 
            EnabledRefRO<PlayerSprintMultiplier> playerSprintEnabled, in PlayerSprintMultiplier playerSprint)
        {
            var finalSpeed = moveSpeed.Value;
            if (playerSprintEnabled.ValueRO)
            {
                var multiplier = playerSprint.Value;
                finalSpeed *= multiplier;
            }
            var moveVector = (CameraForward * moveInput.Value.y + CameraRight * moveInput.Value.x) *
                             finalSpeed * DeltaTime;
            transform = transform.Translate(moveVector);
            if (math.lengthsq(moveVector) <= float.Epsilon) return;
            transform.Rotation = quaternion.LookRotationSafe(moveVector, math.up());
        }
    }
}