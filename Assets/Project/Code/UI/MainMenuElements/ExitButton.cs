using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Code.UI.MainMenuElements
{
    public class ExitButton: MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        void Start()
        {
            _closeButton.OnClickAsObservable()
                .Subscribe(_ => QuitGame())
                .AddTo(this); // Авто-отписка при уничтожении объекта
        }

        private void QuitGame()
        {
            Debug.Log("Завершение работы приложения...");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    #region Реализую позже
    //public class ApplicationExitView : MonoBehaviour
    //{
    //    public Button CloseButton; // Прилинкуй в инспекторе
    //}

    //public class ApplicationPresenter : IInitializable
    //{
    //    [Inject] private ApplicationExitView _view;

    //    public void Initialize()
    //    {
    //        _view.CloseButton.OnClickAsObservable()
    //            .ThrottleFirst(System.TimeSpan.FromSeconds(1)) // Защита от спам-клика
    //            .Subscribe(_ =>
    //            {
    //                // Здесь можно добавить сохранение перед выходом
    //                UnityEngine.Application.Quit();
    //            })
    //            .AddTo(_view);
    //    }
    //}
        #endregion
}
