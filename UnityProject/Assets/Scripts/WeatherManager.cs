using UnityEngine;
using System.Collections;

/*** 
 * The function of this code is to move the ingame object that controls the weather
 * along with the player. Rather than having the sky in every part of the environment
 * emit precipitaion particles, the rain and snow particles are emitted in front of
 * the player's field of view
***/


public class WeatherManager : MonoBehaviour {
	
	public GameObject[] weatherEffects;
	
	public enum WeatherEffectsEnum{
		Rain,
		Snow
	}
	
	// Use this for initialization
	// Makes it so that the 3-dimensional position of the WeatherManager will always follow the player object 
	void Start () {
		gameObject.transform.parent = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		// For now, the fire button will begin the weather particle emission
		// this condition will be replaced when the game is closer to completion
		if(Input.GetButtonDown("Fire1"))
		{
			weatherEffects[(int)WeatherEffectsEnum.Rain].particleEmitter.emit = true;
			Quaternion rotation = transform.rotation;
			rotation.x = 0;
			rotation.z = 0;
			transform.rotation = rotation;
		}
	}
}
