using UnityEngine;
using System.Collections;

public class EquippedPlank : MonoBehaviour {
	
	
	public GameObject Plank;
	GameObject createdPlank;
	
	bool isInAction = false;
	// Use this for initialization
	void Start () {
		//float alpha = .5f;
		//gameObject.renderer.material.color.a = alpha;
		Color color = renderer.material.GetColor("_Color");
		color.r = 1f;
		color.g = .1f;
		color.b = .1f;
		color.a = 1f;
		renderer.material.SetColor("_Color", color);
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
		
		
		//gameObject.animation.Play("axeSwing2");
		Quaternion rotation = transform.rotation;
		rotation.x = 0;
		//rotation.y = 0;
		rotation.z = 0;
		//rotation.w = 0;
	    createdPlank = Instantiate(Plank, transform.position, rotation) as GameObject;
		
		//yield return 0; // wait 1 frame
	    // stop flash
	    //yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(swing2Sound);
	    yield return new WaitForSeconds(1.2f); // extra delay before you can shoot again
	    isInAction = false;
	}
	
}
