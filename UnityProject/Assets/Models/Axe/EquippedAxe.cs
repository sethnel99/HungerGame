using UnityEngine;
using System.Collections;

public class EquippedAxe : MonoBehaviour {
	
	bool isInAction = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && !isInAction){
			StartCoroutine(COAction());
			//audio.PlayOneShot(testSound);
		}
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
