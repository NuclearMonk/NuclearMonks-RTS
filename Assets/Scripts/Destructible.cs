using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IAttackable
{
    public List<IAttacker> _attackers { get; set; } = new List<IAttacker>();


    public void DisableThenDestroy()
    {
        DestructionRemoval();
        Destroy(gameObject, 0.2f);
        gameObject.SetActive(false);

    }

    public void TakeDamage(IAttacker attacker, int damage)
    {
        Debug.Log(gameObject.name + " Was Attacked");
        attacker.RemoveAttackableInDetectionRange(this);
        DisableThenDestroy();
    }
    public void DestructionRemoval()
    {
        foreach(IAttacker attacker in _attackers)
        {
            attacker.targets.Remove(this);
        }
    }
}
