using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject arvore;

    private Transform treeTransform;

    private void Imprimindo()
    {
        
    }

    void Awake()
    {
        treeTransform = arvore.GetComponent<Transform>();
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        treeTransform.position += new Vector3(0.0f, 1.0f  * Time.deltaTime, 0.0f);
    }
}
