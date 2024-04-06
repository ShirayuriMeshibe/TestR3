using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShirayuriMeshibe
{
    public class ModalModel : MonoBehaviour
    {
        public readonly struct Backup
        {
            public readonly float SliderValue;

            public Backup(float sliderValue)
            {
                SliderValue = sliderValue;
            }
        }

        public readonly ReactiveProperty<float> SliderValue = new ReactiveProperty<float>();

        public Backup CreateBackup()
        {
            return new Backup(SliderValue.Value);
        }

        public void ApplyBackup(ref Backup backup)
        {
            SliderValue.Value = backup.SliderValue;
        }
    }
}
