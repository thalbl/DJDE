using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    [SerializeField] private GameObject renderable;
    private Material material;
    
    void Awake()
    {
        this.material = this.renderable.GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            material.EnableKeyword("_EMISSION");
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            material.DisableKeyword("_EMISSION");
    }
}
