using UnityEngine;

namespace Farm.Transition
{
    public class Teleport : MonoBehaviour
    {
        [SceneName] public string sceneToGo;
        public Vector3 positionToGo;

        #region Event Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                EventHandler.CallTransitionEvent(sceneToGo, positionToGo);
            }
        }

        #endregion
    }
}