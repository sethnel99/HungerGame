using UnityEngine;
using System.Collections;

public class StatusHUD : MonoBehaviour {

    public Rect statusHUDRect;
    Rect statusHUDRectNormalized;

    public Texture2D HUDOverlay;
	
	public Texture2D head;
	public Texture2D torso;
	public Texture2D left_arm;
	public Texture2D right_arm;
	public Texture2D left_leg;
	public Texture2D right_leg;
    Texture2D barTexture;
	
	PlayerVitals vitals;
	
	
    Color thirstMinColor = Color.red;
	Color thirstMaxColor = Color.blue;
	
	Color hungerMinColor = Color.red;
	Color hungerMaxColor = Color.green;
	
	
	const float SurvivalRange = 50.0f; //Temperature survival range
	const float NormalBodyTemp = 91.0f; //the average temperature of human skin
	const float DisplayWarningAt = 0.75f; //When warning 
	const float WarningRate = 1.5f; //Number of blinks per second
	
	float AlphaValue = 1.0f; 
	bool IncreasingAlpha;
	
	// Use this for initialization
	void Start () {
		statusHUDRectNormalized = new Rect(statusHUDRect.x * Screen.width, statusHUDRect.y * Screen.height, statusHUDRect.width, statusHUDRect.height);
		vitals = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVitals>();
        barTexture = new Texture2D(150, 20);
	}
	
	// Update is called once per frame
	void Update () {
		
		bool TemperatureWarning = Mathf.Abs((vitals.CurrentTemp - NormalBodyTemp)/SurvivalRange) > DisplayWarningAt;
	
		if(TemperatureWarning)
		{
			if (IncreasingAlpha)
			{
				AlphaValue += (WarningRate / 30f);
				if(AlphaValue > 1)
					IncreasingAlpha = !IncreasingAlpha;
			}
			else
			{
				AlphaValue -= (WarningRate / 30f);
				if(AlphaValue < 0)
					IncreasingAlpha = !IncreasingAlpha;
			}
			
		}
		else{
			AlphaValue = 1.0f;
		}
		
		
	}
	
	void OnGUI(){
		GUI.BeginGroup (statusHUDRectNormalized);
		
		//Draw the body parts
		GUI.color = colorForBodyPart(PlayerVitals.BodyPart.Head);
		GUI.DrawTexture(new Rect(34, 10, 32, 32), head);
		GUI.color = colorForBodyPart(PlayerVitals.BodyPart.Torso);
		GUI.DrawTexture(new Rect(37, 42, 26, 60), torso);
		GUI.color = colorForBodyPart(PlayerVitals.BodyPart.LeftArm);
		GUI.DrawTexture(new Rect(23, 42, 14, 50), left_arm);
		GUI.color = colorForBodyPart(PlayerVitals.BodyPart.RightArm);
		GUI.DrawTexture(new Rect(63, 42, 14, 50), right_arm);
		GUI.color = colorForBodyPart(PlayerVitals.BodyPart.LeftLeg);
		GUI.DrawTexture(new Rect(37, 102, 12, 51), left_leg);
		GUI.color = colorForBodyPart(PlayerVitals.BodyPart.RightLeg);
		GUI.DrawTexture(new Rect(51, 102, 12, 51), right_leg);
		
        //dray the health bar
		GUI.color = Color.gray;
        GUI.DrawTexture(new Rect(127, 50, 150, 20), barTexture);
		GUI.color = Color.Lerp(hungerMinColor, hungerMaxColor, vitals.GetHealthRatio());
        GUI.DrawTexture(new Rect(127, 50, 150 * vitals.GetHealthRatio(), 20), barTexture);
		
		/*/draw the thirst bar
		GUI.color = Color.gray;
        GUI.DrawTexture(new Rect(127, 80, 150, 20), barTexture);
        GUI.color = Color.Lerp(thirstMinColor, thirstMaxColor, vitals.GetThirstRatio());
        GUI.DrawTexture(new Rect(127, 80, 150 * vitals.GetThirstRatio(), 20), barTexture);
		
		
		
		GUI.color = TempToColor();
		GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b,AlphaValue);
		//Debug.Log (vitals.CurrentTemp);
		//Debug.Log (vitals.CurrentTemp.ToString (".0"));
		GUI.Label (new Rect(120,120,60,20),vitals.CurrentTemp.ToString (".0") + " F");*/
			
		GUI.EndGroup();
	}
	
	Color colorForBodyPart(PlayerVitals.BodyPart bp){
		float bpHealth = vitals.bodyPartHealth[(int)bp];
        float bpMaxHealth = vitals.bodyPartMaxHealth[(int)bp];

        return new Color((bpMaxHealth - bpHealth) / bpMaxHealth, bpHealth / bpMaxHealth, 0.0f, 1.0f);
	}
	
		//Returns a new color based on temperature
	Color TempToColor()
	{	
				
		float TempDifference = (vitals.CurrentTemp - NormalBodyTemp)/SurvivalRange;
		
		if (TempDifference >=0)
		{
			return Color.Lerp (Color.green, Color.red, Mathf.Min (1, TempDifference));
		}
		else
		{
			return Color.Lerp (Color.green, Color.blue, Mathf.Min (1, Mathf.Abs(TempDifference)));
		}
	}
	

}
