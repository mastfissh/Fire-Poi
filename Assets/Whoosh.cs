﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whoosh : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float offset;
    private float last_velocity;
    private float last_accel;
    private Rigidbody body;
    private AudioLowPassFilter filt;
    private AudioSource src;
    private float minPass = 1500f;
    private float maxPass = 8000f;
    private int maxVel = 100;

    System.Random rand = new System.Random();

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (float)(rand.NextDouble() * 2.0 - 1.0 + offset);
        }
    }

    private void Start()
    {
        last_velocity = 0.0f;
        last_accel = 0.0f;
        body = this.gameObject.GetComponentInParent<Rigidbody>();
        filt = this.gameObject.GetComponent<AudioLowPassFilter>();
        src = this.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        var accel = Math.Abs(body.velocity.sqrMagnitude - last_velocity);
        var flutter = accel - last_accel;
        last_velocity = body.velocity.sqrMagnitude;
        last_accel = accel;
        var diff = minPass;
        diff += last_accel * 8;
        diff += Math.Min(last_velocity, maxVel) * 14;
        diff = Math.Min(diff, maxPass);
        filt.cutoffFrequency = (diff + filt.cutoffFrequency * 5) /6;
        var minVol = 0.04f;
        var maxVol = 0.16f;
        var vol = minVol + accel / 50;
        src.volume = (Clamp(vol, minVol, maxVol) + src.volume * 9) / 10;
    }
    private float Clamp(float inp, float min, float max)
    {
        return Math.Max(Math.Min(inp, max), min);
    }

}
