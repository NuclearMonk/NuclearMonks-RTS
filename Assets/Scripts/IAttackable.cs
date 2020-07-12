using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    Transform transform
    {
        get;
    }
    List<IAttacker> _attackers
    {
        get;
        set;
    }
    int team
    {
        get;
    }
    int health
    {
        get;
        set;
    }
    void TakeDamage(IAttacker attcker, int damage);
    void DisableThenDestroy();
    void DestructionRemoval();
}
