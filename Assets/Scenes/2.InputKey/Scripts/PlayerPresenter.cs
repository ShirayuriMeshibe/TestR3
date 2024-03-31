using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ShirayuriMeshibe
{
    public class PlayerPresenter : MonoBehaviour
    {
        [SerializeField] PlayerModel _playerModel = null;
        [SerializeField] HealthSlider _healthSlider = null;
        [SerializeField] float _animationSpeed = 1f;

        void Start()
        {
            _playerModel.HealthMax.Subscribe(v => _healthSlider.SetHealthMax(v)).AddTo(this);

            //// �l���ύX�����Ƃ��ɌÂ��l�ƐV�����l���擾����
            //_playerModel.Health.Zip(_playerModel.Health.Skip(1), (v1, v2) => new { OldValue =v1, NewValue = v2 })
            //    .Subscribe(v =>
            //    {
            //        var diff = Mathf.Abs(v.NewValue - v.OldValue);
            //        var normalized = (float)diff / _playerModel.HealthMax.Value;
            //        _healthSlider.AnimateValue(v.OldValue, v.NewValue, normalized * _animationSpeed);
            //    })
            //    .AddTo(this);

            // �����_���Ō��肵�������_���Ȓl��HP���X�V����
            this.LateUpdateAsObservable()
                .Take(1)
                .Subscribe(_ =>
                {
                    _healthSlider.SetHealthMax(_playerModel.HealthMax.Value);
                    _healthSlider.SetHealthimmediately(_playerModel.Health.Value);

                    // ���[�J���ϐ����L���v�`�����ĕύX�O��̒l�ɑΉ�����
                    var previousHealthValue = _playerModel.Health.Value;
                    _playerModel.Health
                        .Subscribe(v =>
                        {
                            var diff = Mathf.Abs(previousHealthValue - v);
                            if (0 < diff)
                            {
                                var normalized = (float)diff / _playerModel.Health.MaxValue;
                                _healthSlider.AnimateValue(previousHealthValue, v, normalized * _animationSpeed);
                            }
                            previousHealthValue = v;
                        })
                        .AddTo(this);
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => _playerModel.Health.Value -= Random.Range(10, 20))
                .AddTo(this);
        }
    }
}
