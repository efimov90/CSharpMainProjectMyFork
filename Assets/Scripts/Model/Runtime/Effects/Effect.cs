using UnitBrains;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public abstract class Effect<TUnitBrain> : Effect
        where TUnitBrain : BaseUnitBrain
    {
        public abstract void AddEffect(TUnitBrain unitBrain);

        public abstract void RemoveEffect(TUnitBrain unitBrain);
    }

    public abstract class Effect
    {
        public abstract float Duration { get; set; }
    }
}
