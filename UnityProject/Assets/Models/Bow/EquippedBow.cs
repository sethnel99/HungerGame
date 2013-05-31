using UnityEngine;
using System.Collections;

public class EquippedBow : MonoBehaviour {
		
	bool disabledByGUI = false;
	bool isInAction = false;
	bool swinging = false;
	GameObject textHints;
	EquippedItem parentScript;
	
	public GameObject Arrow;
	
	void Start () {
		parentScript = transform.parent.GetComponent<EquippedItem>() as EquippedItem;
		textHints = GameObject.Find("TextHintGUI");
	}
	
	void Update () {

		if(Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI){
			StartCoroutine(COAction());
		}
	}
	
	public AudioClip shoot1Sound;
	//NEED TO REDO THESE TIMINGS
	IEnumerator COAction() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f);
		gameObject.animation.Play("Bow_Shoot");
	    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(shoot1Sound);
		GameObject arrow = (GameObject)Instantiate(Arrow, transform.position,transform.rotation);
		arrow.transform.Rotate(0f, 90f, 0f);
		arrow.rigidbody.AddForce(.00011f*transform.parent.transform.forward);
		yield return new WaitForSeconds(0.35f);
		swinging = true; // activate damaging tag
		yield return new WaitForSeconds(0.25f);
		swinging = false;
	    yield return new WaitForSeconds(0.2f); // extra delay before you can shoot again
	    isInAction = false;
	}
	
	void Hit()
	{
	}
	
	public bool IsSwinging()
	{
		return swinging;
	}

    public void DisableByGUI(bool d) {
        disabledByGUI = d;
    }
}