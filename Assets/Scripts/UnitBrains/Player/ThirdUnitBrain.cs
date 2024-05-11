using System.Collections.Generic;
using UnitBrains.Player;
using UnityEngine;

public class ThirdUnitBrain : DefaultPlayerUnitBrain
{
    private const float _changingTime = 60f;

    public override string TargetUnitName => "Ironclad Behemoth";
    private bool _isRunMode = true;
    private bool _isAttackMode = false;
    private bool _isChangingMode = false;
    private float _timeFromToChange = 0f;

    public override void Update(float deltaTime, float time)
    {
        base.Update(deltaTime, time);

        if (_isChangingMode)
        {
            if (_timeFromToChange == 0f)
            {
                _timeFromToChange = _changingTime;
            }

            _timeFromToChange -= deltaTime;

            if (_timeFromToChange <= 0f)
            {
                _isChangingMode = false;
                _timeFromToChange = 0f;

                if (_isAttackMode)
                {
                    _isRunMode = true;
                    _isAttackMode = false;
                }
                else if (_isRunMode)
                {
                    _isRunMode = false;
                    _isAttackMode = true;
                }
            }
        }
    }

    public override Vector2Int GetNextStep()
    {
        var nextStep = base.GetNextStep();
        
        if (_isRunMode && nextStep == unit.Pos && !_isChangingMode)
        {
            _isChangingMode = true;
        }

        if (_isRunMode && !_isChangingMode)
        {
            return nextStep;
        }

        return unit.Pos;
    }

    protected override List<Vector2Int> SelectTargets()
    {
        var targets =  base.SelectTargets();

        if (targets.Count == 0 && _isAttackMode && !_isChangingMode)
        {
            _isChangingMode = true;
        }

        if (targets.Count > 0 && _isAttackMode && !_isChangingMode)
        {
            return targets;
        }

        return new List<Vector2Int>();
    }
}
