using System;
using System.Collections.Generic;
using System.Linq;
using UnitBrains;
using UnitBrains.Player;
using UnityEngine;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public class EffectManager
    {
        public Dictionary<BaseUnitBrain, Effect> _appliedEffectsToUnits = new Dictionary<BaseUnitBrain, Effect>();

        public void AddEffect(BaseUnitBrain unitBrain)
        {
            if (_appliedEffectsToUnits.ContainsKey(unitBrain))
            {
                return;
            }
           
            if(unitBrain is SecondUnitBrain secondUnitBrain)
            {
                AddEffect(secondUnitBrain);
            }

            if (unitBrain is ThirdUnitBrain thirdUnitBrain)
            {
                AddEffect(thirdUnitBrain);
            }
        }

        private void AddEffect<TUnitBrain>(TUnitBrain unitBrain)
            where TUnitBrain : BaseUnitBrain
        {
            var generic = typeof(Effect<>);

            var typeDefinition = generic.GetGenericTypeDefinition();

            Type[] typeArgs = { typeof(TUnitBrain) };

            Type constructed = generic.MakeGenericType(typeArgs);

            var appliableToUnitBrainEffect = typeof(EffectManager).Assembly
                .GetTypes()
                .Where(x =>
                    !x.IsAbstract
                    && x.IsClass
                    && constructed.IsAssignableFrom(x))
                .FirstOrDefault();

            if (appliableToUnitBrainEffect is null)
            {
                return;
            }

            Effect<TUnitBrain> effect = (Effect<TUnitBrain>)Activator.CreateInstance(appliableToUnitBrainEffect);

            if (effect is null)
            {
                return;
            }

            effect.AddEffect(unitBrain);

            _appliedEffectsToUnits.Add(unitBrain, effect);
        }

        public void UpdateEffectsDuration()
        {
            var time = Time.deltaTime;

            foreach (var effect in _appliedEffectsToUnits.Values)
            {
                effect.Duration -= time;
            }

            var effectsToRemove = _appliedEffectsToUnits.Where(kv => kv.Value.Duration <= 0).ToList();

            if(!effectsToRemove.Any())
            {
                return;
            }

            foreach (var effectToRemove in effectsToRemove)
            {
                RemoveEffect(effectToRemove.Key);
                _appliedEffectsToUnits.Remove(effectToRemove.Key);
            }

            effectsToRemove.Clear();
        }

        internal bool HasEffect(BaseUnitBrain brain)
        {
            return _appliedEffectsToUnits.ContainsKey(brain);
        }

        internal void RemoveEffect(BaseUnitBrain brain)
        {
            if(!HasEffect(brain))
            {
                return;
            }
            _appliedEffectsToUnits.Remove(brain);
        }
    }
}
