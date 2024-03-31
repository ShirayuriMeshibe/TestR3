using R3;
using System.Collections.Generic;

namespace ShirayuriMeshibe
{
    /// https://github.com/Cysharp/R3?tab=readme-ov-file#subjectsreactiveproperty
    public sealed class ReactivePropertyHealth : ReactiveProperty<int>
    {
        readonly int min;
        int max;

        // callOnValueChangeInBaseConstructor to avoid OnValueChanging call before min, max set.
        public ReactivePropertyHealth(int initialValue, int min, int max)
            : base(initialValue, EqualityComparer<int>.Default, callOnValueChangeInBaseConstructor: false)
        {
            this.min = min;
            this.max = max;

            // modify currentValue manually
            OnValueChanging(ref GetValueRef());
        }

        protected override void OnValueChanging(ref int value)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
        }

        public int MaxValue
        {
            get => max;
            set
            {
                if (value < min)
                    max = min + 1;
                else
                    max = value;
            }
        }
    }
}
