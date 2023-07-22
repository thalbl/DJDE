using UnityEngine;

public class CameraFollow : MonoBehaviour
{  
    private Transform target;

    public Transform Target
    {
        get { return this.target; }
        set { this.target = value; }
    }

    public float Phi;
    public float Theta;
    public float Distance;

    public Vector3 Direction;

    void Awake()
    {
        float x = Mathf.Sin(Phi * Mathf.Deg2Rad) * Mathf.Cos(Theta * Mathf.Deg2Rad);
        float y = Mathf.Cos(Phi * Mathf.Deg2Rad);
        float z = Mathf.Sin(Phi * Mathf.Deg2Rad) * Mathf.Sin(Theta * Mathf.Deg2Rad);

        this.Direction = new Vector3(x, y, z);

        transform.position = this.target.position + this.Direction * Distance;
        transform.LookAt(this.target);
    }

    void LateUpdate()
    {
        transform.position = this.target.position + this.Direction * Distance;
        transform.LookAt(this.target);
    }
}
