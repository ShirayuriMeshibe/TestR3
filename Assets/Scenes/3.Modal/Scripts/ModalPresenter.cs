using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShirayuriMeshibe
{
    public class ModalPresenter : MonoBehaviour
    {
        [SerializeField] ModalModel _modalModel = null;
        [SerializeField] Toggle _toggleOpen = null;
        [SerializeField] GameObject _root = null;
        [SerializeField] Slider _slider = null;
        [SerializeField] TextMeshProUGUI _textSliderValue = null;
        [SerializeField] Button _buttonApply = null;
        [SerializeField] Button _buttonCancel = null;
        [SerializeField] Image _imageBlockingArea = null;
        [SerializeField] Image _imageDialog = null;

        ModalModel.Backup _backup;
        CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            _toggleOpen.onValueChanged.AddListener(isOn =>
            {
                // �_�C�A���O��\������Ƃ�
                if (isOn)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    var token = _cancellationTokenSource.Token;
                    _backup = _modalModel.CreateBackup();

                    // �{�^���̓�d������h��
                    _buttonApply.OnClickAsObservable().Take(1).Subscribe(_ =>
                    {
                        _toggleOpen.isOn = false;
                    }).AddTo(token);
                    _buttonCancel.OnClickAsObservable().Take(1).Subscribe(_ =>
                    {
                        _modalModel.ApplyBackup(ref _backup);
                        _toggleOpen.isOn = false;
                    }).AddTo(token);

                    _root.SetActive(true);
                }
                // �_�C�A���O���\���ɂ���Ƃ�
                else
                {
                    if (_cancellationTokenSource != null)
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource.Dispose();
                        _cancellationTokenSource = null;
                    }
                    _root.SetActive(false);
                }
            });

            // �_�C�A���O�ȊO���N���b�N�����Ƃ��ɕ���悤�ɂ���
            {
                var raycastResults = new List<RaycastResult>();
                _imageBlockingArea.OnPointerClickAsObservable().Subscribe(e =>
                {
                    raycastResults.Clear();
                    EventSystem.current.RaycastAll(e, raycastResults);
                    if (raycastResults.Count == 1 && raycastResults[0].gameObject == _imageBlockingArea.gameObject)
                    {
                        _modalModel.ApplyBackup(ref _backup);
                        _toggleOpen.isOn = false;
                    }
                }).AddTo(this);
            }

            _modalModel.SliderValue.Subscribe(value =>
            {
                _textSliderValue.text = string.Format($"{value:0.00}");
                _slider.value = value;
            }).AddTo(this);
            _slider.onValueChanged.AddListener(v => _modalModel.SliderValue.Value = v);

            // �����I��OFF�̒ʒm�����s���邽��
            _toggleOpen.SetIsOnWithoutNotify(true);
            _toggleOpen.isOn = false;
        }
    }
}
