using Code.Hero.Data;
using Code.Hero.Items;
using Code.UI.Elements;
using Code.UI.HeroWindow;
using UniRx;
using UnityEngine;

namespace Code.UI.Windows
{
    public class DemoWindow : WindowBase
    {
        [SerializeField] private HeroWindowUI _heroWindowUI;

        [Header("Demo Equipment Items")]
        [SerializeField] private EquipmentItemUI ArmorUI;
        [SerializeField] private EquipmentItem ArmorSO;
        [SerializeField] private EquipmentItemUI WeaponUI;
        [SerializeField] private EquipmentItem WeaponSO;
        [SerializeField] private ConsumableItemUI BottleUI;
        [SerializeField] private ConsumableItem BottleSO;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Construct(IHeroDataService heroDataService)
        {
            heroDataService.CurrentHero
                .Where(hero => hero != null)
                .Subscribe(hero =>
                {
                    _heroWindowUI.Construct(hero, _disposables);
                    ArmorUI.Construct(ArmorSO, hero);
                    WeaponUI.Construct(WeaponSO, hero);
                    BottleUI.Construct(BottleSO, hero);
                })
                .AddTo(_disposables);
        }

        protected override void Cleanup()
        {
            _disposables.Dispose();
        }
    }
}
