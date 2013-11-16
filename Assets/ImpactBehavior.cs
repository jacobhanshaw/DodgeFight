using UnityEngine;
using System.Collections;

public class ImpactBehavior : MonoBehaviour 
{
	public int wallHealth = 3;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		--wallHealth;
		if(wallHealth == 0)
			Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider other)
	{
		--wallHealth;
		if(wallHealth == 0)
			Destroy(gameObject);
	}
}
