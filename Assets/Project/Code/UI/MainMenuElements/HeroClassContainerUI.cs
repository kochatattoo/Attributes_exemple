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

        private readonly Dictionary<int, HeroClassUI> _items = new();

        [SerializeField] private HeroClassUI _itemPrefab;
        [SerializeField] private Transform _container;

        private CompositeDisposable _disposables = new();
        private int _lastSelectedId = -1;

        public void Construct(HeroDataFabric heroData, IAsset asset)
        {
            _disposables.Clear();

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
                .DistinctUntilChanged()
                .Subscribe(sel =>
                {
                    // Снимаем выделение с предыдущего элемента
                    if (_items.TryGetValue(_lastSelectedId, out var oldUi))
                        oldUi.SetSelected(false);

                    // Устанавливаем выделение новому
                    if (_items.TryGetValue(sel, out var newUi))
                        newUi.SetSelected(true);

                    // Запоминаем текущий как старый для следующего раза
                    _lastSelectedId = sel;

                })
                .AddTo(_disposables);
        }

        private void OnDestroy() => 
            _disposables.Dispose();

        private void CreateUIItem(int id, HeroClass heroClass, HeroDataFabric heroData, IAsset asset)
        {
            if (_items.ContainsKey(id)) return;

            var ui = Instantiate(_itemPrefab, _container);

            ui.SetData(heroClass, id, asset, () => heroData.SelectClass(id));

            _items[id] = ui;

            // Сразу проверяем, не выбран ли этот ID прямо сейчас
            ui.SetSelected(id == heroData.SelectedId.Value);
        }
    }
}
