﻿using UnityEngine;
using System.Collections;

public class PassOnCollision : MonoBehaviour
{
	
	public CharacterScript script;
	
	// Use this for initialization
	/*
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	*/
	
	void OnCollisionEnter (Collision collision)
	{
		Destroy (collision.gameObject);
		script.BlockReceived ();
	}
	
	void OnTriggerEnter (Collider other)
	{
		Destroy (other.gameObject);
		script.BlockReceived ();
	}
}
