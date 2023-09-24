using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFlash : MonoBehaviour
{
    // NewsFlashText Variables
    public NewsFlashText newsFlashTextPrefab;
    NewsFlashText currText;

    // Float Variables
    [Range(1f, 10f)]
    public float textDuration = 3.0f;
    float width, pixsPerSec;

    // String Variables
    public string[] textChoices;
    
    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<RectTransform>().rect.width;
        pixsPerSec = width / textDuration;
        AddText(textChoices[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if(currText.GetXPosition <= -currText.GetWidth)
        {
            AddText(textChoices[Random.Range(0, textChoices.Length)]);
        }
    }

    void AddText(string text)
    {
        currText = Instantiate(newsFlashTextPrefab, transform);
        currText.Initialize(width, pixsPerSec, text);
    }
}
