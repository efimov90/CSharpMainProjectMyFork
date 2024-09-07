using System.Reflection;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public class EffectDoubleRange : Effect<ThirdUnitBrain>
    {
        public override float Duration { get; set; } = 5f;

        public override void AddEffect(ThirdUnitBrain unitBrain)
        {
            unitBrain.ApplyDoubleRange();
        }

        public override void RemoveEffect(ThirdUnitBrain unitBrain)
        {
            unitBrain.RemoveDoubleRange();
        }
    }
}
