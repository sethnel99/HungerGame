using System;
using UnityEngine;

public interface DamageDealer
{
    float damage {
        get;
        set;
    }
    
    void Hit();

}
