using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainforest : MonoBehaviour
{
    public GameObject particle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyEffect() {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
}
