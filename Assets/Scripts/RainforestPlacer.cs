using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestPlacer : MonoBehaviour {
    public GameObject rainforestPrefab;

    public static bool isLoading = true;

    int size = 128;

    float x, y;
    int i;

    void Start() {
        y = size/4;
    }

    void Update() {
        if(i >= size*2) { 
            isLoading = false;
            return; 
        }
        
        int num = i > size ? size - (i % size) : i;

        x = -num / 2f;
        for(int j = 0; j < num; j++) {
            if(Mathf.Abs(x) > Random.Range(1.7f, 2.1f) || Mathf.Abs(y) > Random.Range(1.5f, 1.9f)) {
                Instantiate(rainforestPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
            x += 1;
        }

        y -= 0.25f;

        i++;
    }
}
