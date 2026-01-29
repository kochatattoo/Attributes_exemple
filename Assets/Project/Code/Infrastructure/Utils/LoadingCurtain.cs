using System.Collections;
using UnityEngine;

namespace Code.Infrastructure.Utils
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup Curtain;

        //private void Awake()
        //{
        //    DontDestroyOnLoad(this);
        //}

        public void Show()
        {
            gameObject.SetActive(true);
            Curtain.alpha = 1;
        }

        public void Hide() =>
            StartCoroutine(FadeIn());

        private IEnumerator FadeIn()
        {
            while (Curtain.alpha > 0)
            {
                Curtain.alpha -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            gameObject.SetActive(false);
        }
    }
}
