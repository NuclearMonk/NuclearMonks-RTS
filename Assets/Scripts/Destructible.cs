using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IAttackable
{

    public void DisableThenDestroy()
    {
        Destroy(gameObject, 0.2f);
        gameObject.SetActive(false);

    }

    public void TakeDamage(IAttacker attacker, int damage)
    {
        Debug.Log(gameObject.name + " Was Attacked");
        attacker.RemoveAttackableInDetectionRange(this);
        DisableThenDestroy();
    }
}
