using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    Transform transform
    {
        get;
    }
    void TakeDamage(IAttacker attcker, int damage);
    void DisableThenDestroy();
    
}
