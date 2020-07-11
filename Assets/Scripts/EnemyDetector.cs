using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    IAttacker _attacker;

    private void Awake()
    {
        _attacker = GetComponentInParent<IAttacker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IAttackable attackable = other.GetComponent<IAttackable>();
        if (attackable != null)
        {
            _attacker.NewAttackableInDetectionRange(attackable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IAttackable attackable = other.GetComponent<IAttackable>();
        if (attackable != null)
        {
            _attacker.RemoveAttackableInDetectionRange(attackable);
        }
    }
}
