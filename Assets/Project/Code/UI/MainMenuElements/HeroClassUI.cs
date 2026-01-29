using Code.Hero.Data;
using Code.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


namespace Code.UI.MainMenuElements
{
    public class HeroClassUI: MonoBehaviour
    {
        private readonly CompositeDisposable _disposables;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _icon;
        [SerializeField] private int _heroID;

        private HeroClass _hero;
        private Action _onClick;

        public async void SetData(HeroClass hero, int id, IAsset asset, Action onClick)
        {
            Debug.Log("HeroClassUI SetData");

            _hero = hero;
            _onClick = onClick;
            _name.text = hero.Name;
            _icon.sprite = await asset.LoadAsync<Sprite>(_hero.ImagePath);

            _button.OnClickAsObservable()
                   .Subscribe(_ => _onClick())
                   .AddTo(this);
        }

        public void SetSelected(bool isSel)
        {
            // например, меняем цвет фона
            GetComponent<Image>().color = isSel
                ? Color.yellow
                : Color.white;
        }

        private void SetLabelColor(string hex, TextMeshProUGUI field)
        {
            if (!hex.StartsWith("#")) hex = "#" + hex;
            if (ColorUtility.TryParseHtmlString(hex, out var c))
                field.color = c;
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}
