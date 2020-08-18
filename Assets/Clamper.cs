using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamper : MonoBehaviour
{
	private float maxSpeed = 1000f;
	private float maxSpeedSqr;
    // Start is called before the first frame update
    void Start()
    {
        maxSpeedSqr = maxSpeed * maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.frameCount % 5) == 0) {
        	if(GetComponent<Rigidbody>().velocity.sqrMagnitude > maxSpeedSqr){
	            GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxSpeed);
	        }
        }
    }
}
