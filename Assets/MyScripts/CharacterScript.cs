using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	private int timesHit = 0;
	private int timesPunched = 0;
	public GameObject statusTextBar;
	public GameObject floor;
	public GameObject invisibleWall;
	
	private TextRenderScript script;
	
	// Use this for initialization
	void Start () 
	{
		script = statusTextBar.GetComponent<TextRenderScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnTriggerEnter(Collider other)
	{
  		if(other.gameObject != floor && other.gameObject != invisibleWall)
  		{
  			timesHit++;
  			script.allowTextToDisappear = true;
  			script.DisplayText("Times Hit: " + timesHit);
  			Destroy(other.gameObject);
  		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == invisibleWall)
		{
			script.allowTextToDisappear = false;
  			script.DisplayText("Out of Bounds");
		}
	}
	
	public void PunchReceived()
	{
		timesPunched++;
  		script.allowTextToDisappear = true;
  		script.DisplayText("Times Punched: " + timesPunched);
	}
}
