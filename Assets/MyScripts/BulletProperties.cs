using UnityEngine;
using System.Collections;

public class BulletProperties : MonoBehaviour {
	
	public float invincibleTime = 0.25f;
	public int creatorId;
	
	private float timeSinceCreate = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		AudioSource source = gameObject.GetComponent<AudioSource>();
		source.pitch = 0.25f;
	//	gameObject.collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeSinceCreate += Time.deltaTime;
		if(timeSinceCreate > invincibleTime)
			creatorId = 0;
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Destroy(gameObject);
	}
	
}
