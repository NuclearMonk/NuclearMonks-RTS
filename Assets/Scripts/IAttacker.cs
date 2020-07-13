using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    List<IAttackable> targets {get; set;}
    Transform transform {get;}
    bool hasTarget {get; set;}
    float range {get;}
    int team {get;}
    int damage {get;}
    float rampuptime { get; }
    float cooldownTime { get; }
    bool isoncooldown { get; set; }
    void NewAttackableInDetectionRange(IAttackable attackable);
    void RemoveAttackableInDetectionRange(IAttackable attackable);
    void Attack(IAttackable target);
    IEnumerator AttackCoroutine(IAttackable target);
}
