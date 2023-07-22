using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private float phi;
    [SerializeField] private float theta;
    [SerializeField] private float timeToRotate;

    private CameraFollow cameraRef;

    void Awake() => this.cameraRef = this.camera.GetComponent<CameraFollow>();
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            StartCoroutine(RotateCoroutine(other.transform));
    }

    private IEnumerator RotateCoroutine(Transform target)
    {
        float timeElapsed = 0.0f;
        while(timeElapsed < timeToRotate)
        {
            float latitude   = Mathf.Lerp(this.cameraRef.Phi, phi, timeElapsed/timeToRotate);
            float colatitude = Mathf.Lerp(this.cameraRef.Theta, theta, timeElapsed/timeToRotate);

            cameraRef.Phi = latitude;
            cameraRef.Theta = colatitude;

            float x = Mathf.Sin(latitude * Mathf.Deg2Rad) * Mathf.Cos(colatitude * Mathf.Deg2Rad);
            float y = Mathf.Cos(latitude * Mathf.Deg2Rad);
            float z = Mathf.Sin(latitude * Mathf.Deg2Rad) * Mathf.Sin(colatitude * Mathf.Deg2Rad);

            cameraRef.Direction = new Vector3(x, y, z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
