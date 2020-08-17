using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticDrag : MonoBehaviour
{

    public float DragCoefficient = 0.5f;

    private Rigidbody Body;

    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Fetch the body velocity.
        var velocity = Body.velocity;

        // Calculate the body speed.
        var speed = velocity.sqrMagnitude;
        if (speed > 0f)
        {
            // Calculate and apply the quadratic drag force (speed squared).
            var dragForce = (DragCoefficient * speed) * -velocity.normalized;
            Body.AddForce(dragForce, ForceMode.Force);
        }
    }
}
