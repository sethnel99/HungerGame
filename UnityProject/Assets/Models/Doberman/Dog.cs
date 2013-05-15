using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour {
	
	float timer = 0f;
	
	// Use this for initialization
	void Start () {
		timer = 0f;
		//StartCoroutine(COMainBehavior());
	}
	
	IEnumerator COMainBehavior() {
	    //yield return new WaitForSeconds(0.1f); // trigger delay
	    // create bullet and flash effect...
		gameObject.animation.Play("Stand_Idle");
	    //yield return 0; // wait 1 frame
	    // stop flash
	    yield return new WaitForSeconds(2f);
	    gameObject.animation.Play("Idle_Twitch_1");
	    yield return new WaitForSeconds(3f); // extra delay before you can shoot again
	    gameObject.animation.Play("Bark_1");
	}
	
	// Update is called once per frame
	void Update () {
		if(timer<8f)
		{
			timer += Time.deltaTime;
		}
		else
		{
			timer=0;
			StartCoroutine(COMainBehavior());
		}
	}
}
