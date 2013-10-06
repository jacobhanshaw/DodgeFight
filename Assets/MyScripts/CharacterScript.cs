using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	private int timesHit = 0;
	private int timesPunched = 0;
	public GameObject statusTextBar;
	public GameObject hitTextBar;
	public GameObject blockTextBar;
	public GameObject floor;
	public GameObject invisibleWall;
	
	private TextRenderScript script;
	private AudioSource[] sources;
	
	// Use this for initialization
	void Start () 
	{
		script = statusTextBar.GetComponent<TextRenderScript>();
		sources = gameObject.GetComponents<AudioSource>();
		sources[0].pitch = 1.2f;
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
  			sources[0].Stop();
  			sources[0].Play();
  			hitTextBar.GetComponent<TextMesh>().text = "Hits: " + timesHit;
  			blockTextBar.GetComponent<TextMesh>().text = "Blocks: " + timesPunched;
  			Destroy(other.gameObject);
  		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == invisibleWall)
		{
			//script.allowTextToDisappear = true;
  			//script.DisplayText("Out of Bounds");
		}
	}
	
	public void PunchReceived()
	{
		timesPunched++;
		sources[1].Stop();
		sources[1].Play();
  		hitTextBar.GetComponent<TextMesh>().text = "Hits: " + timesHit;
  		blockTextBar.GetComponent<TextMesh>().text = "Blocks: " + timesPunched;
	}
}
