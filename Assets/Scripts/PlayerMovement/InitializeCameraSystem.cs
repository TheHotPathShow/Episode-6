using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace THPS.CombatSystem
{
    public partial struct InitializeCameraSystem : ISystem
    {
        private EntityQuery _uninitializedCameraQuery;
        
        public void OnCreate(ref SystemState state)
        {
            _uninitializedCameraQuery = SystemAPI.QueryBuilder().WithAll<PlayerMoveInput>()
                .WithNone<CameraTransformReference>().Build();
            
            state.RequireForUpdate(_uninitializedCameraQuery);
        }

        public void OnUpdate(ref SystemState state)
        {
            if (CameraMonoSingleton.Instance == null)
            {
                Debug.LogError("Error: no CameraMonoSingleton found");
                return;
            }

            var uninitializedCameras = _uninitializedCameraQuery.ToEntityArray(Allocator.Temp);
            foreach (var uninitializedCamera in uninitializedCameras)
            {
                state.EntityManager.AddComponentObject(uninitializedCamera,
                    new CameraTransformReference
                    {
                        CameraTarget = CameraMonoSingleton.Instance.CameraTargetTransform,
                        Camera = CameraMonoSingleton.Instance.MainCameraTransform
                    });
            }
        }
    }
}