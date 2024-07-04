using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using JJH;

namespace JJH
{
    public class SceneManager : Singleton<SceneManager>
    {
        [SerializeField] Image fade;
        [SerializeField] Slider loadingBar;
        [SerializeField] float fadeTime;

        [SerializeField]
        private GameObject playerObject;
        public GameObject PlayerObject { get { return playerObject; } set { playerObject = value; } }

        private BaseScene curScene;

        [Tooltip("플레이어의 카메라")]
        private Camera playerCamera;

        public BaseScene GetCurScene()
        {
            if (curScene == null)
            {
                curScene = FindObjectOfType<BaseScene>();
            }
            return curScene;
        }

        public T GetCurScene<T>() where T : BaseScene
        {
            if (curScene == null)
            {
                curScene = FindObjectOfType<BaseScene>();
            }
            return curScene as T;
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadingRoutine(sceneName));
        }

        IEnumerator LoadingRoutine(string sceneName)
        {
            playerCamera = Camera.main;
            SetUpFadeUI(); // 캔버스를 world space로 변경 
            fade.gameObject.SetActive(true);
            yield return FadeOut();

            Time.timeScale = 0f;
            loadingBar.gameObject.SetActive(true);

            AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
            while (oper.isDone == false)
            {
                loadingBar.value = oper.progress;
                yield return null;
            }


            BaseScene curScene = GetCurScene();

            yield return null; // 이 부분 시간 차 어떻게 둘지 생각해보기. 
            yield return curScene?.LoadingRoutine();


            loadingBar.gameObject.SetActive(false);
            Time.timeScale = 1f;

            yield return FadeIn();
            fade.gameObject.SetActive(false);

            RestoreFadeUI(); // 다시 원래 상태로 복원 
        }

        IEnumerator FadeOut()
        {
            float rate = 0;
            Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
            Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

            while (rate <= 1)
            {
                rate += Time.deltaTime / fadeTime;
                fade.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
                yield return null;
            }
        }

        IEnumerator FadeIn()
        {
            float rate = 0;
            Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
            Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

            while (rate <= 1)
            {
                rate += Time.deltaTime / fadeTime;
                fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
                yield return null;
            }
        }



        private void SetUpFadeUI()
        {
            if (playerCamera != null && fade != null)
            {
                // 캔버스가 VR 카메라 앞에 위치하도록 설정
                Canvas canvas = fade.GetComponentInParent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = playerCamera;
                canvas.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 0.1f; // 카메라 앞 0.5m 위치
                canvas.transform.rotation = playerCamera.transform.rotation;
                canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); // 필요에 따라 스케일 조정
            }
        }

        private void RestoreFadeUI()
        {
            if (fade != null)
            {
                Canvas canvas = fade.GetComponentInParent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }

    }
}

