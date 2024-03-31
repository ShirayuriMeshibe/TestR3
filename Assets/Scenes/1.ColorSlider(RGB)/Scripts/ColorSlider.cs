using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace ShirayuriMeshibe
{
    public sealed class ColorSlider : MonoBehaviour
    {
        public readonly ReactiveProperty<Color> Color = new ReactiveProperty<Color>(UnityEngine.Color.black);

        [SerializeField] private Slider _red;
        [SerializeField] private Slider _green;
        [SerializeField] private Slider _blue;

        private void Start()
        {
            Color.Subscribe(c =>
            {
                _red.value = c.r;
                _green.value = c.g;
                _blue.value = c.b;
            }).AddTo(this);

            // スライダーがどれか1つでも変化したら反映
            Observable.Merge(
                    _red.OnValueChangedAsObservable(),
                    _green.OnValueChangedAsObservable(),
                    _blue.OnValueChangedAsObservable())
                .Subscribe(_ =>
                {
                    Color.Value = new Color(_red.value, _green.value, _blue.value);
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Take(1)
                .Subscribe(_ => Color.Value = UnityEngine.Color.white)
                .AddTo(this);
        }
    }
}
