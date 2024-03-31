using R3;
using UnityEngine;

namespace ShirayuriMeshibe
{
    public class PlayerModel : MonoBehaviour
    {
        public const int HealthMaxValue = 100;

        public readonly ReactivePropertyHealth Health = new ReactivePropertyHealth(HealthMaxValue, 0, HealthMaxValue);
        public readonly ReactiveProperty<int> HealthMax = new ReactiveProperty<int>(HealthMaxValue);
        public readonly ReactiveProperty<Color> Color = new ReactiveProperty<Color>(UnityEngine.Color.black);

        // Start is called before the first frame update
        void Start()
        {
            HealthMax.Value = Random.Range(HealthMaxValue, HealthMaxValue+101);
            Health.MaxValue = HealthMax.Value;
            Health.Value = HealthMax.Value;
        }
    }
}
