using Code.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
    public abstract class WindowBase: MonoBehaviour
    {
        [SerializeField] private Button CloseButton;

        private void Awake() =>
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        protected virtual void OnAwake() =>
            CloseButton.AddListener(() => Destroy(gameObject));

        protected virtual void Initialize() { }

        protected virtual void SubscribeUpdates() { }

        protected virtual void Cleanup() { }

    }
}
