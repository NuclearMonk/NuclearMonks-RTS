using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    List<IAttackable> targets
    {
        get;
        set;
    }
    Transform transform
    {
        get;
    }
    bool _hasTarget
    {
        get;
        set;
    }
    void NewAttackableInDetectionRange(IAttackable attackable);
    void RemoveAttackableInDetectionRange(IAttackable attackable);
    float CheckRange(IAttackable attackable);
    void Attack(IAttackable target);
}
