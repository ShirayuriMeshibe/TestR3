using Aya.Tween;
using UnityEngine;
using UnityEngine.UI;

namespace ShirayuriMeshibe
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeField] Slider _slider = null;

        public void SetHealthimmediately(int value)
        {
            _slider.value = value;
        }

        public void SetHealthMax(int value)
        {
            _slider.maxValue = value;
        }

        public void AnimateValue(int from, int to, float duration)
        {
            // LMotion��duration��Infinity���n�����ƁA���̌�̓��삪���������Ȃ�
            if (float.IsInfinity(duration) || float.IsNaN(duration))
                return;

            to = Mathf.Max(0, to);

            // MEMO: LitMotion���g���Ɠ���s����ɂȂ�B
            //LMotion.Create(from, to, duration)
            //    .WithEase(Ease.InQuad)
            //    .Bind(v => _slider.value = v)
            //    .AddTo(this);

            UTween.Create<TweenValue>()
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .SetEaseType(EaseType.EaseInQuad)
                .SetFloatCallback(v => _slider.value = v)
                .Play();
        }
    }
}
