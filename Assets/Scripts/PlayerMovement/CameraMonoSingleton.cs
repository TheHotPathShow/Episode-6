using UnityEngine;

namespace THPS.CombatSystem
{
    public class CameraMonoSingleton : MonoBehaviour
    {
        public static CameraMonoSingleton Instance;
        
        public Transform CameraTargetTransform {get; private set;}
        public Transform MainCameraTransform {get; private set;}
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CameraTargetTransform = transform;
            MainCameraTransform = Camera.main.transform;
                
        }
    }
}