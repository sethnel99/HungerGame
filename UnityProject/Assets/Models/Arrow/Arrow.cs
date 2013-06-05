using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour, DamageDealer {

    ParticleEmitter bloodSplatter;
    public float damage {
        get;
        set;
    }

	// Use this for initialization
	void Start () {
        bloodSplatter = gameObject.GetComponentInChildren<ParticleEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hit() {
        bloodSplatter.Emit();
    }

   
}
