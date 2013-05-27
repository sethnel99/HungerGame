using UnityEngine;
using System.Collections;

public class TestInventory : MonoBehaviour {
	
	public static int charge = 0;
	public AudioClip collectSound;
	public AudioClip winSound;
	
	// HUD
	public Texture2D[] hudCharge;
	public GUITexture chargeHudGUI;
	
	// Generator
	public Texture2D[] meterCharge;
	public Renderer meter;
	
	// Use this for initialization
	void Start () {
		charge = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CellPickup(){
		HUDon();
		AudioSource.PlayClipAtPoint(collectSound, transform.position);
		charge++;
		chargeHudGUI.texture = hudCharge[charge];
		meter.material.mainTexture = meterCharge[charge];
	}
	
	void HUDon(){
		if(!chargeHudGUI.enabled){
			chargeHudGUI.enabled = true;
		}
	}
	
	void Gather () {
		AudioSource.PlayClipAtPoint(collectSound, transform.position);
	}
	
	public void Win() {
		AudioSource.PlayClipAtPoint(winSound, transform.position);
	}
}
