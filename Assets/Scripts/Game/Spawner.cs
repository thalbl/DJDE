using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private float offset = 0.5f;

    public GameObject Item
    {
        get { return this.item; }
        set { this.item = value; } 
    }

    void Awake()
    {
        if(this.Item)
            this.Item = Instantiate(this.Item, transform.position + new Vector3(0.0f, this.offset, 0.0f), Quaternion.identity);
    }
}
