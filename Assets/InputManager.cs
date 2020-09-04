using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
    {
    public GameObject Ltarget;
    public GameObject Rtarget;
    private float fixedDeltaTime;
    private Rigidbody Lrigid;
    private Rigidbody Rrigid;
    private Transform Ltrans;
    private Transform Rtrans;
    private Vector3 shadowScale;

    private GameObject Leye;
    private GameObject Reye;
    // Start is called before the first frame update
    void Start()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
        Lrigid = Ltarget.GetComponent<Rigidbody>();
        Rrigid = Rtarget.GetComponent<Rigidbody>();
        Ltrans = Ltarget.GetComponent<Transform>();
        Rtrans = Rtarget.GetComponent<Transform>();
        shadowScale = Ltrans.localScale;
        Leye = GameObject.Find("Leye");
        Reye = GameObject.Find("Reye");
        Leye.SetActive(false);
        Reye.SetActive(false);
    }	

    // Update is called once per frame
    void Update()
    {
        var baseColorScale = new Vector4(1, 1, 1, 1);
        var alteredColorScale = new Vector4(0.7f, 0.9f, 0.9f, 1);
        var index_triggers = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) + 
    		OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        if (index_triggers > 0.5f){
            Time.timeScale = 1.0f;
            Unity.XR.Oculus.Utils.SetColorScaleAndOffset(baseColorScale, baseColorScale);
            Leye.SetActive(false);
            Reye.SetActive(false);
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }

        var hand_triggers = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) + 
    		OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

        if (hand_triggers > 0.5f){
            Time.timeScale = 0.5f;
            Unity.XR.Oculus.Utils.SetColorScaleAndOffset(alteredColorScale, baseColorScale);
            Leye.SetActive(true);
            Reye.SetActive(true);
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        var thumbsticks = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) +
            OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        var vertical = thumbsticks[1];
        if (Math.Abs(vertical) > 0.2f)
        {
            var maxVec = new Vector3(1, 1, 1) * 0.2f;
            float factor = (float)(1 + (vertical * 0.03));
            float geoFactor = (float)(1 + (vertical * 0.004));
            Lrigid.mass = Math.Min(Lrigid.mass * factor, 40);
            Rrigid.mass = Math.Min(Rrigid.mass * factor, 40);
            Lrigid.drag = Math.Min(Lrigid.drag * factor, 1);
            Rrigid.drag = Math.Min(Rrigid.drag * factor, 1);
            Lrigid.angularDrag = Math.Min(Lrigid.angularDrag * factor, 1);
            Rrigid.angularDrag = Math.Min(Rrigid.angularDrag * factor, 1);
            shadowScale = shadowScale * geoFactor;
            Ltrans.localScale = Vector3.Min(shadowScale, maxVec);
            Rtrans.localScale = Vector3.Min(shadowScale, maxVec);
        }
    }
}