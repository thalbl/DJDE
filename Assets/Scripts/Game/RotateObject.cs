using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationXSpeed;
    [SerializeField] private float rotationYSpeed;
    [SerializeField] private float rotationZSpeed;
    [SerializeField] private float speed = 1.0f;

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(this.rotationXSpeed, this.rotationYSpeed, this.rotationZSpeed) * this.speed * Time.deltaTime);
    }
}
