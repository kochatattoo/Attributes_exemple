using Code.Hero.Data;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.MainMenuElements
{
    public class DistributePoints : MonoBehaviour
    {
        [SerializeField] private string _attrName;

        [Header("UI Elements")]
        [SerializeField] private Button _increaseBtn;
        [SerializeField] private Button _decreaseBtn;
        [SerializeField] private TextMeshProUGUI _attributePointsText;
       // [SerializeField] private TextMeshProUGUI _attributeNameLabel;

        private int _baseValue; // Базовое значение из конфига
        private IntReactiveProperty _addedPoints = new(0); // Очки, вложенные игроком сейчас
        private CompositeDisposable _disposables = new();

        private Color _baseColor = Color.white;
        private int _points = 0;

        public string AttrName => _attrName;
        public int AddedPoints => _addedPoints.Value;


        public void Bind(HeroClass selectedHero, IntReactiveProperty globalBonusPoints, CompositeDisposable lifetime)
        {
            _disposables.Clear(); // Чистим старые подписки при смене героя
            _addedPoints.Value = 0;

            // 1. Получаем базу
            _baseValue = selectedHero.Attributes.GetValue(_attrName);

           // _attributeNameLabel.text = _attrName;

            // 2. Реактивное обновление текста суммы (База + Вложенные)
            _addedPoints.Subscribe(added => {
                _attributePointsText.text = (_baseValue + added).ToString();
            }).AddTo(_disposables);

            // 3. Логика кнопок
            _increaseBtn.onClick.AsObservable()
                .Where(_ => globalBonusPoints.Value > 0)
                .Subscribe(_ => {
                    globalBonusPoints.Value--;
                    _addedPoints.Value++;
                }).AddTo(_disposables);

            _decreaseBtn.onClick.AsObservable()
                .Where(_ => _addedPoints.Value > 0)
                .Subscribe(_ => {
                    _addedPoints.Value--;
                    globalBonusPoints.Value++;
                }).AddTo(_disposables);

            // Добавляем наши внутренние подписки в общий жизненный цикл окна
            _disposables.AddTo(lifetime);
        }
    }
}
