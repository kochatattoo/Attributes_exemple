using Code.Hero.Data;
using Code.Infrastructure.AssetManagement;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.UI.MainMenuElements
{
    public class HeroClassContainerUI : MonoBehaviour
    {
        //Класс UI в котором находятся элементы отображения наших классов
        //Инстанируем объекты и загружаем по их данным из HeroClass иконку, имя
        //Информация о загруженных классах хранится в HeroProvider => IReadOnlyDictionary<int, HeroClass> HeroClassesReadonly

        //Сделать так же класс HeroClassUI, на котором есть кнопка - и при клике на объект мы его выбираем и информируем всех в потоке

        [SerializeField] private HeroClassUI _itemPrefab;
        [SerializeField] private Transform _container;

        private readonly Dictionary<int, HeroClassUI> _items = new();
        private CompositeDisposable _disposables = new();

        public void Construct(HeroData heroData, IAsset asset)
        {
            foreach (var kvp in heroData.Classes)
            {
                CreateUIItem(kvp.Key, kvp.Value, heroData, asset);
            }

            heroData.Classes
                .ObserveAdd()
                .Subscribe(add =>
                {
                    var ui = Instantiate(_itemPrefab, _container);
                    ui.SetData(add.Value, add.Key, asset, () => heroData.SelectedId.Value = add.Key);
                    _items[add.Key] = ui;
                })
                .AddTo(_disposables);

            heroData.Classes
                .ObserveRemove()
                .Subscribe(rem =>
                {
                    if (_items.Remove(rem.Key, out var ui))
                    Destroy(ui.gameObject);
                })
                .AddTo(_disposables);

            heroData.SelectedId
             .Subscribe(sel =>
             {
                 foreach (var kv in _items)
                     kv.Value.SetSelected(kv.Key == sel);
             })
             .AddTo(_disposables);
        }

        private void OnDestroy() => 
            _disposables.Dispose();

        private void CreateUIItem(int id, HeroClass heroClass, HeroData heroData, IAsset asset)
        {
            if (_items.ContainsKey(id)) return;

            var ui = Instantiate(_itemPrefab, _container);
            ui.SetData(heroClass, id, asset, () => heroData.SelectedId.Value = id);
            _items[id] = ui;

            // Сразу проверяем, не выбран ли этот ID прямо сейчас
            ui.SetSelected(id == heroData.SelectedId.Value);
        }
    }
}
