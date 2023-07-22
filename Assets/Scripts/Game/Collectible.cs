using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private ItemObj itemObj;

    void Start()
    {
        this.itemObj = GetComponent<ItemObj>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Player gets the item
        if(other.CompareTag("Player"))
        {
            Debug.Log("Você pegou um item!");

            Player player = other.GetComponentInParent<Player>();
            this.itemObj.Item.AddItem(player);
            
            Destroy(gameObject);
        }
    }
}
