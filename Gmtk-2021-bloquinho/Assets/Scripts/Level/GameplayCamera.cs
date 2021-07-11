using Cinemachine;
using UnityEngine;

namespace Level
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _cinemachine = null;
        private CinemachineVirtualCamera Cinemachine => _cinemachine;

        public void SetCamera(Transform focusedAgent)
        {
            Cinemachine.Follow = focusedAgent;
            Cinemachine.LookAt = focusedAgent;
        }
    }
}
