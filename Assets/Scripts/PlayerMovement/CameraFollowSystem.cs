using Unity.Entities;
using Unity.Transforms;

namespace THPS.CombatSystem
{
    public partial struct CameraFollowSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (localToWorld, cameraTransform) in SystemAPI.Query<LocalToWorld, CameraTransformReference>())
            {
                cameraTransform.CameraTarget.position = localToWorld.Position;
            }
        }
    }
}