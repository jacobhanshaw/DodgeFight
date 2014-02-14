using UnityEngine;
using System.Collections;

public class TextRenderScript : MonoBehaviour
{
		public  bool allowTextToDisappear = true;
	
		private bool textDisplayed = false;
		private float timeSinceTextDisplayed = 0.0f;
		private float durationOfTextAppearance = 2.0f;
	
		private TextMesh textMesh;

		void Start ()
		{
				textMesh = gameObject.GetComponent<TextMesh> ();
		}
	
		void Update ()
		{
				textMesh.renderer.enabled = textDisplayed;
		
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
				textMesh.text = text;
		}
}
