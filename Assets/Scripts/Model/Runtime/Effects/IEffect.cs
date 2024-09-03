using UnitBrains;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public interface IEffect<in TUnitBrain>
        where TUnitBrain : BaseUnitBrain
    {
        float Duration { get; set; }

        void AddEffect(TUnitBrain unitBrain);
        void RemoveEffect(TUnitBrain unitBrain);
    }
}