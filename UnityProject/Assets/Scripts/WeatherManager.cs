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
	int start;
	float end;
	int checkToStart;
	bool fog;
	bool rain;
	float i;
	float rate;
	
	public enum WeatherEffectsEnum{
		Rain
	}
	
	// Use this for initialization
	// Makes it so that the 3-dimensional position of the WeatherManager will always follow the player object 
	void Start () {
		gameObject.transform.parent = GameObject.FindWithTag("Player").transform;
		start = (int)(Random.value*7000 +1);
		RenderSettings.fogDensity = 0.0f;
		RenderSettings.fog = true;
		end = Random.value*200+200;
		i = 0.0f;
		rate = 1.0f/ 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		// For now, the fire button will begin the weather particle emission
		// this condition will be replaced when the game is closer to completion
		checkToStart = (int)(Random.value*7000 +1);
		if(checkToStart == start){
			weatherEffects[(int)WeatherEffectsEnum.Rain].particleEmitter.emit = true;
			Quaternion rotation = transform.rotation;
			rotation.x = 0;
			rotation.z = 0;
			fog = true;
			rain = true;
		
			transform.rotation = rotation;
		}
		
		if (fog){ 
   			if (i < 1.0) {
        		i +=   Time.deltaTime* rate;
        		RenderSettings.fogDensity = Mathf.Lerp (0.0f, 0.01f, i ); 
			}
		}
		
		if (!fog && rain){
			if (i < 1.0) {
        		i +=   Time.deltaTime* rate;
        		RenderSettings.fogDensity = Mathf.Lerp (0.01f, 0.0f, i ); 
			}
		}
		
		
		if (weatherEffects[(int)WeatherEffectsEnum.Rain].particleEmitter.emit){
			end = end - Time.deltaTime;
		}
		
		if(end <= 0.0f){
			weatherEffects[(int)WeatherEffectsEnum.Rain].particleEmitter.emit = false;
			fog = false;
			i = 0.0f;
			end = Random.value*200+200;
		}
	}
	
}
