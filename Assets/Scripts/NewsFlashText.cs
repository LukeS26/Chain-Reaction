using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewsFlashText : MonoBehaviour
{
    // Float Variables
    float width, pixsPerSec;

    // RectTransform Variables
    RectTransform rt;
    
    // Update is called once per frame
    void Update()
    {
        rt.position += Vector3.left * pixsPerSec * Time.deltaTime;

        // Destroys the text's prefab once it moves off the screen
        if(GetXPosition <= (0 - width - GetWidth))
        {
            Destroy(gameObject);
        }
    }

    // Returns the X Position of the gameObject's anchor
    public float GetXPosition{ get { return rt.anchoredPosition.x; } }

    // Returns the width of the prefab
    public float GetWidth{ get { return rt.rect.width; } }

    // Initializes the prefab
    public void Initialize(float width, float pixsPerSec, string text)
    {
        this.width = width;
        this.pixsPerSec = pixsPerSec;
        rt = GetComponent<RectTransform>();
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
