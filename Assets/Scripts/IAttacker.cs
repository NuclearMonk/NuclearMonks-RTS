using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    void NewAttackableInDetectionRange(IAttackable attackable);
    void RemoveAttackableInDetectionRange(IAttackable attackable);
    float CheckRange(IAttackable attackable);
    void Attack(IAttackable target);
}
