using Code.Hero.Data;
using Code.UI.MainMenuElements;
using UniRx;
using UnityEngine;

namespace Code.UI.HeroWindow
{
    public class HeroWindowUI: MonoBehaviour
    {
        [SerializeField] private HeroParamentresUI _heroParamentresUI;
        [SerializeField] private HeroAttributesUI _heroAttributesUI;
        [SerializeField] private HeroTimeEffectsUI _heroTimeEffectsUI;

        private readonly CompositeDisposable _heroInternalDisposables = new();

        public void Construct(HeroData HeroData, CompositeDisposable windowLifetime)
        {
            _heroInternalDisposables.Clear();

            _heroParamentresUI.Construct(HeroData, _heroInternalDisposables);
            _heroAttributesUI.Construct(HeroData, _heroInternalDisposables);
            _heroTimeEffectsUI.Construct(HeroData, _heroInternalDisposables);

            _heroInternalDisposables.AddTo(windowLifetime);
        }
    }
}

