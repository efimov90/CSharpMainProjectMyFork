using System;
using System.Collections.Generic;
using System.Linq;
using UnitBrains;
using UnityEngine;

namespace Assets.Scripts.Model.Runtime.Effects
{
    public class EffectManager
    {
        public Dictionary<BaseUnitBrain, IEffect<BaseUnitBrain>> _appliedEffectsToUnits = new Dictionary<BaseUnitBrain, IEffect<BaseUnitBrain>>();

        public void AddEffect(BaseUnitBrain unitBrain)
        {
            var unitBrainType = unitBrain.GetType();

            if (_appliedEffectsToUnits.ContainsKey(unitBrain))
            {
                return;
            }

            var generic = typeof(Effect<>);

            var typeDefinition = generic.GetGenericTypeDefinition();

            Type[] typeArgs = { unitBrainType };

            Type constructed = generic.MakeGenericType(typeArgs);

            var appliableToUnitBrainEffect = typeof(EffectManager).Assembly
                .GetTypes()
                .Where(x =>
                    !x.IsAbstract
                    && x.IsClass
                    && constructed.IsAssignableFrom(x))
                .FirstOrDefault();

            if(appliableToUnitBrainEffect is null)
            {
                return;
            }

            IEffect<BaseUnitBrain> effect = (IEffect<BaseUnitBrain>)Activator.CreateInstance(appliableToUnitBrainEffect);

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
                effectToRemove.Value.RemoveEffect(effectToRemove.Key);
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
