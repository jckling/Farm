using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Farm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName] public string startSceneName = string.Empty;

        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        #region Event Functions

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }

        private void Start()
        {
            StartCoroutine(LoadSceneSetActive(startSceneName));
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        }

        #endregion

        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
            EventHandler.CallAfterSceneLoadedEvent();
        }

        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            yield return LoadSceneSetActive(sceneName);
            EventHandler.CallMoveToPosition(targetPosition);
            EventHandler.CallAfterSceneLoadedEvent();
            yield return Fade(0);
        }

        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
            {
                StartCoroutine(Transition(sceneToGo, positionToGo));
            }
        }


        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}