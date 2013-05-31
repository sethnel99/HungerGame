using UnityEngine;
using System.Collections;

public class EquippedKnife : MonoBehaviour {

		
	bool disabledByGUI = false;
	bool isInAction = false;
	bool swinging = false;
	GameObject textHints;
	EquippedItem parentScript;
	ParticleEmitter bloodSplatter;
	
	void Start () {
		parentScript = transform.parent.GetComponent<EquippedItem>() as EquippedItem;
		bloodSplatter = gameObject.GetComponentInChildren<ParticleEmitter>();
		textHints = GameObject.Find("TextHintGUI");
	}
	
	void Update () {

		if(Input.GetButtonDown("Fire1") && !isInAction && !disabledByGUI){
					
			float rand = Random.Range(0f, 100f);
			if(rand >50)
			{
				StartCoroutine(COAction1());
			}
			else
				StartCoroutine(COAction2());

		}
	}
	
	public AudioClip swing1Sound;
	public AudioClip stab1Sound;
	//NEED TO REDO THESE TIMINGS
	IEnumerator COAction1() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f);
		gameObject.animation.Play("Knife_Swing_1");
	    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(swing1Sound);
		yield return new WaitForSeconds(0.35f);
		swinging = true; // activate damaging tag
		yield return new WaitForSeconds(0.25f);
		swinging = false;
	    yield return new WaitForSeconds(0.2f); // extra delay before you can shoot again
	    isInAction = false;
	}
	IEnumerator COAction2() {
	    isInAction = true;
	    //yield return new WaitForSeconds(0.1f);
		gameObject.animation.Play("Knife_Stab_1");
	    yield return new WaitForSeconds(0.35f); // wait 0.05seconds
	    audio.PlayOneShot(stab1Sound);
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
