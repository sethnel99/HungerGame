using UnityEngine;
using System.Collections;

public class EquippedAxe : MonoBehaviour {
	
	bool isInAction = false;
	GameObject textHints;
	EquippedItem parentScript;
	// Use this for initialization
	void Start () {
		parentScript = transform.parent.GetComponent<EquippedItem>() as EquippedItem;
		textHints = GameObject.Find("TextHintGUI");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && !isInAction){
			
			float rand = Random.Range(0f, 100f);
			if(rand >50)
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
	    yield return new WaitForSeconds(0.8f); // extra delay before you can shoot again
	    isInAction = false;
	}
}
