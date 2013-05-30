using UnityEngine;
using System.Collections;

public class EquippedAxe : MonoBehaviour {
	
	bool disabledByGUI = false;
	bool isInAction = false;
	bool swinging = false;
	GameObject textHints;
	EquippedItem parentScript;
	ParticleEmitter bloodSplatter;
	
	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<EquippedItem>() as EquippedItem;
		bloodSplatter = gameObject.GetComponentInChildren<ParticleEmitter>();
		textHints = GameObject.Find("TextHintGUI");
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI){
			
			float rand = Random.Range(0f, 100f);
			if(rand >0)
			{
				textHints.SendMessage("ShowHint",
						"The axe isn't used for this demo, but feel free to swing it around.\n" +
						"Having 10 fingers is overrated anyway.");
			}
			
			
			StartCoroutine(COAction());
			//audio.PlayOneShot(testSound);
		}
//		if(Input.GetButtonDown("Undo") && !isInAction)
//		{
//			parentScript.EquipItem((int)EquippedItem.Equippable.Axe);
//		}
	}
	
	public AudioClip swing2Sound;
	IEnumerator COAction() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f); // trigger delay
	    // create bullet and flash effect...
		gameObject.animation.Play("axeSwing1");
	    //yield return 0; // wait 1 frame
	    // stop flash
	    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(swing2Sound);
		yield return new WaitForSeconds(0.35f);
		swinging = true; // activate damaging tag
		yield return new WaitForSeconds(0.25f);
		swinging = false;
	    yield return new WaitForSeconds(0.2f); // extra delay before you can shoot again
	    isInAction = false;
	}
	
	void Hit()
	{
		bloodSplatter.Emit();
	}
	
	public bool IsSwinging()
	{
		return swinging;
	}

    public void DisableByGUI(bool d) {
        disabledByGUI = d;
    }
}
