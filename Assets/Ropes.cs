﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent (typeof (Rigidbody))]
public class Ropes : MonoBehaviour
{

	public GameObject destination;
	public GameObject lineHolder;
	// all of these are total, not per segment
	public float drag = 0.1f;
	public float mass = 0.1f;
	public float angular_drag = 0.5f;
	public float width = 0.05f;
	public int jointCount = 5;

	private float maxSpeed = 10000f;
	private float maxSpeedSqr;

	private GameObject source;
	private int segmentCount;
	private ConfigurableJoint baseJoint;
	private LineRenderer line;
	private GameObject[] jointHolders;
	private float ropeDrag;
	private float ropeMass;
	private float ropeAngDrag;

    // Start is called before the first frame update
    void Start()
    {
    	line = lineHolder.GetComponent<LineRenderer>();
    	baseJoint = lineHolder.GetComponent<ConfigurableJoint>();
    	// One to join with the destination, one to join the source
    	segmentCount = jointCount + 2;
		jointHolders = new GameObject[jointCount];
		ropeDrag = drag / jointCount;
		ropeMass = mass / jointCount;
		ropeAngDrag = angular_drag / jointCount;
		line.positionCount = segmentCount;
		source = this.gameObject;
		maxSpeedSqr = maxSpeed * maxSpeed;
        BuildRope();
    }

    // Update is called once per frame
    void Update()
    {
		var positions = new Vector3[segmentCount];
		int idx = 0;
		positions[idx] = source.transform.position;
		idx++;
		foreach (GameObject jointHolder in jointHolders)
	    {
    		positions[idx] = jointHolder.transform.position;
    		idx++;
    		if(jointHolder.GetComponent<Rigidbody>().velocity.sqrMagnitude > maxSpeedSqr){
	            jointHolder.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxSpeed);
	        }
	    }
    	positions[idx] = destination.transform.position;
		line.SetPositions(positions);
    }

    void BuildRope()
	{
		Vector3 delta = ((destination.transform.position - transform.position)/jointCount);
		float size = delta.magnitude;
		GameObject last = source;
		for(int idx=0; idx < jointCount ;idx++)
		{
			Vector3 jointPosition = (delta*(idx+1)) + transform.position;
			var jointHolder = buildJointHolder(idx, size);
			jointHolders[idx] = jointHolder;
			jointHolder.transform.position = jointPosition;
			Join(last, jointHolder);
			last = jointHolder;
		}
		Join(last, destination);	
	}

	void Join(GameObject src, GameObject dest)
	{
		var joint = buildJoint(src);
		Physics.IgnoreCollision(src.GetComponent<Collider>(), dest.GetComponent<Collider>());
		joint.connectedBody = dest.GetComponent<Rigidbody>();
	}

	T CopyComponent<T>(T original, GameObject destination) where T : Component
	{
	     System.Type type = original.GetType();
	     Component copy = destination.AddComponent(type);
	     System.Reflection.FieldInfo[] fields = type.GetFields();
	     foreach (System.Reflection.FieldInfo field in fields)
	     {
	         field.SetValue(copy, field.GetValue(original));
	     }
	     return copy as T;
	}

	GameObject buildJointHolder(int idx, float size) {
		var item = new GameObject("JointHolder_" + idx);
		Rigidbody rigid = item.AddComponent<Rigidbody>();
		CapsuleCollider col = item.AddComponent<CapsuleCollider>();
		rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		rigid.angularDrag = ropeAngDrag;
		rigid.solverIterations = 12;
		col.radius = width;
		col.height = size;
		return item;
	}

	ConfigurableJoint buildJoint(GameObject target)
	{
		ConfigurableJoint joint = CopyComponent(baseJoint, target);
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		var spring = new SoftJointLimitSpring();
		spring.spring = 8000;
		spring.damper = 8000;
		joint.linearLimitSpring = spring;
		return joint;
	}
	
}
