using UnityEngine;
using System.Collections;

public class EnemyAttackBox : MonoBehaviour {
	
	
	public float Damage;
    public float damageMultiplier = 1.0f;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setDamageMultiplier(float m) {
        damageMultiplier = m;
    }

   
}
