using UnityEngine;
using System.Collections;

public class HUDOverlay : MonoBehaviour
{
	private Color thirstMinColor = Color.red;
	private Color thirstMaxColor = Color.blue;
	
	private Color hungerMinColor = Color.red;
	private Color hungerMaxColor = Color.green;
	
	Rect hungerBox = new Rect(127, 50, 150, 20);
	Rect thirstBox = new Rect(127, 15, 150, 20);
	
	public float health = 100;
	public float maxHealth = 100;
	
	private HUDBar thirstBar, hungerBar;
	private GUIText TemperatureText;
	
	private PlayerVitals PlayerData;
	
	// Use this for initialization
	void Start()
	{
		PlayerData = (PlayerVitals) GameObject.Find("First Person Controller").GetComponent ("PlayerVitals");
		
		thirstBar = new HUDBar(thirstMinColor, thirstMaxColor, PlayerData.GetThirstRatio(), thirstBox);
		hungerBar = new HUDBar(hungerMinColor, hungerMaxColor, PlayerData.GetHealthRatio(), hungerBox);
		
		//InitializeTemperature();
		thirstBar.Initialize();
		hungerBar.Initialize();
	}
	
	// Update is called once per frame
	void Update()
	{
		health += Input.GetAxisRaw("Horizontal");
		if (health < 0) health = 0;
		if (health > maxHealth) health = maxHealth;
		
		thirstBar.Update(PlayerData.GetThirstRatio());
		hungerBar.Update(PlayerData.GetHealthRatio());
	}
	
	// Draw
	void OnGUI()
	{
		thirstBar.Draw();
		hungerBar.Draw();
	}
	
	private void InitializeTemperature()
	{
		TemperatureText = new GUIText();
		TemperatureText.font = (Font)Resources.Load("Fonts/Arial Black", typeof(Font));
		TemperatureText.font.material.color = new Color(0f,255f,0f);
		//CurrentTemp = NormalBodyTemp;
	}
}

