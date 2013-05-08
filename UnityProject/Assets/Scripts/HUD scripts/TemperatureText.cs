using UnityEngine;

public class TemperatureText : MonoBehaviour
{
	private const float SurvivalRange = 50.0f; //Temperature survival range
	private const float NormalBodyTemp = 97.0f;
	private const float DisplayWarningAt = 0.75f; //When warning 
	private const float WarningRate = 1.5f; //Number of blinks per second
	
    private GUIText DegreeText;
	
	private float CurrentTemp;
	private float TempDifference; //Ratio of current temperature to normal body temp
	
	private float AlphaValue = 1.0f; 
	
	private bool TemperatureWarning, IncreasingAlpha;
	

	//Use this for initialization
    void Start()
    {
        DegreeText= GameObject.Find("TemperatureText").guiText;
		DegreeText.font.material.color = new Color(0f,255f,0f);
		CurrentTemp = NormalBodyTemp;
    }

	//Update is called once per frame
    void Update()
    {
		Color newColor;
		
		//CurrentTemp-= 0.1f;
		
		TempDifference = (CurrentTemp - NormalBodyTemp)/SurvivalRange;
		TemperatureWarning = Mathf.Abs(TempDifference) > DisplayWarningAt;
		
		newColor = TempToColor();
		
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
		else
			AlphaValue = 1.0f;
		
		DegreeText.font.material.color = new Color(newColor.r, newColor.g, newColor.b, AlphaValue);
			
        DegreeText.text = CurrentTemp.ToString(".0") + " F";	
    }
	
	//Returns a new color based on temperature
	Color TempToColor()
	{	
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
