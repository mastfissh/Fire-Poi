using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent (typeof (Rigidbody))]
public class Ropes : MonoBehaviour
{
	public Transform target;
	public GameObject lineHolder;
	public float ropeDrag = 0.1F;
	public float ropeMass = 0.1F;
	public float ropeColRadius = 0.1F;
	public float ropeMin = 0.05f;
	public float ropeMax = 0.11f;
	public float damper = 25;
	public float springPower = 2000;
	public int jointCount = 5;
	private Vector3[] segmentPos;
	private GameObject[] joints;
	private int segments = 0;
 	private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
    	line = lineHolder.GetComponent<LineRenderer>();
        BuildRope();
    }

    // Update is called once per frame
    void Update()
    {
    	line.positionCount = segments + 2;
    	var joint_positions = joints.Select(
    		x => x.transform.position
    		).ToArray();
    	var positions = new Vector3[segments + 2];
    	positions[0] = transform.position;
    	Array.Copy(joint_positions, 0, positions, 1, joint_positions.Length);
    	positions[segments + 1] = target.position;
		line.SetPositions(positions);
    }

    void BuildRope()
	{
		segments = jointCount;
		segmentPos = new Vector3[segments];
		joints = new GameObject[segments];
		segmentPos[0] = transform.position;
		segmentPos[segments-1] = target.position;
 
		// Find the distance between each segment
		var segs = segments-1;
		var separation = ((target.position - transform.position)/segs);

		for(int s=0;s < segments ;s++)
		{
			// Find the each segments position using the slope from above
			Vector3 vector = (separation*s) + transform.position;	
			segmentPos[s] = vector;
 
			//Add Physics to the segments
			AddJointPhysics(s);
		}

		SpringJoint end = buildJoint(target.gameObject);
		end.connectedBody = joints[joints.Length-1].transform.GetComponent<Rigidbody>();		
	}

	SpringJoint buildJoint(GameObject target){
		SpringJoint joint = target.AddComponent<SpringJoint>();
		joint.minDistance = ropeMin;
		joint.maxDistance = ropeMax;
		joint.autoConfigureConnectedAnchor = false;
		joint.damper = damper;
		joint.connectedAnchor = new Vector3(0,0,0);
		joint.spring = springPower;
		return joint;
	}

	void AddJointPhysics(int n)
	{
		joints[n] = new GameObject("Joint_" + n);
		joints[n].transform.position = target.position;
		Rigidbody rigid = joints[n].AddComponent<Rigidbody>();
		SphereCollider col = joints[n].AddComponent<SphereCollider>();
		SpringJoint ph = buildJoint(joints[n]);
		rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		col.radius = ropeColRadius;
 
		if(n==0){		
			ph.connectedBody = transform.GetComponent<Rigidbody>();
		} else
		{
			ph.connectedBody = joints[n-1].GetComponent<Rigidbody>();	
		}
 
	}
	
}
