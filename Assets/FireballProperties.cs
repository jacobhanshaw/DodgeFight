using UnityEngine;
using System.Collections;

public class FireballProperties : MonoBehaviour 
{
	private Component[] emitters;
	
	// Use this for initialization
	void Start () 
	{
		emitters = gameObject.GetComponentsInChildren<ParticleEmitter>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		Destroy(gameObject);
	}
	
	public void Scale(float scale)
	{
		Vector3 newScale = gameObject.transform.localScale;
		newScale.x += scale;
		newScale.y += scale;
		newScale.z += scale;
		gameObject.transform.localScale = newScale;
		
		//foreach(ParticleEmitter emitter in emitters)
		//{
		//	emitter.minSize += scale;
		//	emitter.maxSize += scale;
		//}
	}
}
