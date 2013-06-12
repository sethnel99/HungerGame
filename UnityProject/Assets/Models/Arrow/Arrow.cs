using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour, DamageDealer {

    public float arrowTimeout = 45.0f;

    ParticleEmitter bloodSplatter;
    public float damage {
        get;
        set;
    }

    InteractionManager interactionManager;

	// Use this for initialization
	void Start () {
        bloodSplatter = gameObject.GetComponentInChildren<ParticleEmitter>();
        interactionManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionManager>();
	}
	
	// Update is called once per frame
	void Update () {
        arrowTimeout -= Time.deltaTime;
        if (arrowTimeout < 0.0f) {
            interactionManager.removePotentialInteractor(gameObject);
            Destroy(gameObject);
        }
	}

    public void Hit() {
        bloodSplatter.Emit();
        Debug.Log("hit");
    }

   
}
