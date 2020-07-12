using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IAttackable
{
    public List<IAttacker> _attackers { get; set; } = new List<IAttacker>();
    public int _team;
    public int team => _team;

    public int health { get; set; } = 10;

    public void DisableThenDestroy()
    {
        DestructionRemoval();
        Destroy(gameObject, 0.2f);
        gameObject.SetActive(false);

    }

    public void TakeDamage(IAttacker attacker, int damage)
    {
        Debug.Log(gameObject.name + " Was Attacked");
        health -= damage;
        Debug.Log(health, this);
        if (health <= 0)
        {
            DisableThenDestroy();
        }

    }
    public void DestructionRemoval()
    {
        foreach(IAttacker attacker in _attackers)
        {
            attacker.targets.Remove(this);
        }
    }
}
