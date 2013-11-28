using UnityEngine;
using System.Collections;

public class TextRenderScript : MonoBehaviour
{
	
	public  bool allowTextToDisappear = true;
	
	private bool textDisplayed = false;
	private float timeSinceTextDisplayed = 0.0f;
	private float durationOfTextAppearance = 2.0f;
	
	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponent<MeshRenderer> ().enabled = textDisplayed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.GetComponent<MeshRenderer> ().enabled = textDisplayed;
		
		if (textDisplayed && allowTextToDisappear) {
			timeSinceTextDisplayed += Time.deltaTime;
			if (timeSinceTextDisplayed >= durationOfTextAppearance)
				textDisplayed = false;
		}
	}
	
	public void DisplayText (string text)
	{
		textDisplayed = true;
		timeSinceTextDisplayed = 0.0f;
		gameObject.GetComponent<TextMesh> ().text = text;
	}
}
