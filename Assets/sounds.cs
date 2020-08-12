using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class sounds : MonoBehaviour
{
	public GameObject debug_target;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    	Rigidbody rb = GetComponent<Rigidbody>();
		Vector3 v3Velocity = rb.velocity; 
		float velocity = v3Velocity.magnitude;
      	// this.GetComponent<AudioSource>().volume = (float)(Math.Sqrt(velocity) / 10);   
    }
}
