using UnityEngine;
using System.Collections;

public class EnemyVitals : MonoBehaviour {
    
    public float health = 100f;
    float damageHitCD = 0.45f;
    float damageHitCDTimer = 0.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (damageHitCDTimer > 0) {
            damageHitCDTimer -= Time.deltaTime;
        }
	}
    
    public void TakeDamage(float damage) {
        if (damageHitCDTimer > 0) {
            return;
        } else {
            damageHitCDTimer = damageHitCD;
        }
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        //GameObject.Destroy(gameObject);
        gameObject.SendMessage("StartDeathAnim");
    }

}
