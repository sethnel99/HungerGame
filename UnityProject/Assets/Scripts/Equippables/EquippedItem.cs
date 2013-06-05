using UnityEngine;
using System.Collections;

public abstract class EquippedItem : MonoBehaviour, DamageDealer {

    protected bool disabledByGUI = false;
    protected bool isInAction = false;
    protected GameObject textHints;
    protected EquippedItemManager parentScript;
    protected ParticleEmitter bloodSplatter;
    public float damage {get;set;}


	// Use this for initialization
	protected virtual void Start () {
        parentScript = transform.parent.GetComponent<EquippedItemManager>() as EquippedItemManager;
        bloodSplatter = gameObject.GetComponentInChildren<ParticleEmitter>();
        textHints = GameObject.Find("TextHintGUI");
        
	}
	
	// Update is called once per frame
    protected abstract void Update();

    public virtual void Hit() {
        bloodSplatter.Emit();
    }


    public void DisableByGUI(bool d) {
        disabledByGUI = d;
    }

}
