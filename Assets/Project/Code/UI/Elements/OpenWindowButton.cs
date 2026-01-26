using Code.UI.Services;
using Code.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button Button;
        [SerializeField] private WindowID WindowId;

        private IWindowService _windowService;

        public void Construct(IWindowService windowService) =>
            _windowService = windowService;

        private void Awake() =>
            Button.AddListener(Open);

        private void OnDisable() =>
            Button.RemoveListener(Open);

        private void Open()
        {
            _windowService.Open(WindowId);
        }
    }
}
