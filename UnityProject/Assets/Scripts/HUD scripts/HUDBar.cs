using UnityEngine;
using System.Collections;

public class HUDBar
{
	private Texture2D background, foreground;
	private Color minColor, maxColor;
	private float ratio;
	private Rect box;
	
	public HUDBar(Color min, Color max, float initialRatio, Rect bound)
	{
		minColor = min;
		maxColor = max;
		ratio = initialRatio;
		box = bound;
	}
	
	public void Initialize()
	{ 
		background = new Texture2D(1, 1, TextureFormat.RGB24, false);
		foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);
		
		background.SetPixel(0, 0, Color.gray);
		
		Color newColor = Color.Lerp(minColor, maxColor, ratio);
		
		foreground.SetPixel(0, 0, newColor);
 
		background.Apply();
		foreground.Apply();
	}
	
	public void Draw()
	{
		GUI.BeginGroup(box);
		{
			GUI.DrawTexture(new Rect(0, 0, box.width, box.height), background, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(0, 0, box.width*ratio, box.height), foreground, ScaleMode.StretchToFill);
		}
		GUI.EndGroup();
	}
	
	public void Update(float newRatio)
	{
		ratio = newRatio;
		
		Color newColor = Color.Lerp(minColor, maxColor, ratio);
		
		foreground.SetPixel(0, 0, newColor);
		foreground.Apply();
	}
}

