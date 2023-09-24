using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour {
    bool isPickedUp;
    Vector3 tileSizeInUnits = new Vector3(1f, 0.5f, 1f);

    float hoverTimer;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Drag(Vector3 target) {
        hoverTimer += Time.deltaTime * 2;
        hoverTimer %= Mathf.PI * 2;

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);

        transform.position = Snap(target, -1) + Vector3.up * 0.03f * Mathf.Sin(hoverTimer);
        // transform.position = (transform.position * 0.9f) + (target * 0.1f);
    }

    virtual public void Drop() {
        hoverTimer = 0;

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        GetComponent<AudioSource>().Play();

        transform.position = Snap(transform.position, 0);
    }

    virtual public void ClickOn() {
        GetComponent<AudioSource>().Play();
     }

    Vector3 Snap(Vector3 localPosition, float z) {
        // Calculate ratios for simple grid snap
        float xx = Mathf.Round(localPosition.y / tileSizeInUnits.y - localPosition.x / tileSizeInUnits.x);
        float yy = Mathf.Round(localPosition.y / tileSizeInUnits.y + localPosition.x / tileSizeInUnits.x);

        // Calculate grid aligned position from current position
        float x = (yy - xx) * 0.5f * tileSizeInUnits.x;
        float y = (yy + xx) * 0.5f * tileSizeInUnits.y;

        return new Vector3(x,y,z);
    }
}
