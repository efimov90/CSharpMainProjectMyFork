using UnitBrains;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public abstract class Effect<TUnitBrain> : IEffect<TUnitBrain> where TUnitBrain : BaseUnitBrain
    {
        public abstract float Duration { get; set; }

        public abstract void AddEffect(TUnitBrain unitBrain);

        public abstract void RemoveEffect(TUnitBrain unitBrain);
    }
}
