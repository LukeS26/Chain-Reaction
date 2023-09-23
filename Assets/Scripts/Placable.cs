using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placable : MonoBehaviour
{
    bool isPickedUp;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Drag(Vector3 target) {
        transform.position = ((transform.position * 0.9f) + (target * 0.1f));
    }
}
