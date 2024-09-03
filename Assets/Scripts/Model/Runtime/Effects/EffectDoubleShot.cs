using UnitBrains.Player;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public class EffectDoubleShot : Effect<SecondUnitBrain>
    {
        public override float Duration { get; set; } = 5f;

        public override void AddEffect(SecondUnitBrain secondUnitBrain)
        {
            secondUnitBrain.ApplyDoubleShot();
        }

        public override void RemoveEffect(SecondUnitBrain secondUnitBrain)
        {
            secondUnitBrain.RemoveDoubleShot();
        }
    }
}
