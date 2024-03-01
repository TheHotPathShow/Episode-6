using System.Collections;
using UnityEngine;

namespace CapabilityAuthoring
{
    public class CleanupGameObject : MonoBehaviour
    {
        [SerializeField] private float _timeToLive = 2f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_timeToLive);
            Destroy(gameObject);
        }
    }
}