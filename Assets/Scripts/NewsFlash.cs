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
    public string[] textChoices, happinessBoostText, greenwashingText, positiveText, negativeText;

    // Boolean Variables
    public bool happBought, greenBought;

    // StatManager Variables
    StatManager stats;
    
    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<RectTransform>().rect.width;
        pixsPerSec = width / textDuration;
        AddText(textChoices[0]);
        happBought = false;
        greenBought = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stats) { stats = FindObjectOfType<StatManager>(); }
        
        if(currText.GetXPosition <= -currText.GetWidth)
        {
            string nextText;

            // Decides what type of text to use
            if(happBought) 
            { 
                nextText = happinessBoostText[Random.Range(0, happinessBoostText.Length)];
                happBought = false;
            }
            else if(greenBought)
            {
                nextText = greenwashingText[Random.Range(0, greenwashingText.Length)];
                greenBought = false;
            }
            else if(stats.popularity >= 0.7f) { nextText = positiveText[Random.Range(0, positiveText.Length)]; }
            else if(stats.popularity <= 0.3f) { nextText = negativeText[Random.Range(0, negativeText.Length)]; }
            else { nextText = textChoices[Random.Range(0, textChoices.Length)]; }
            
            AddText(nextText);
        }
    }

    void AddText(string text)
    {
        currText = Instantiate(newsFlashTextPrefab, transform);
        currText.Initialize(width, pixsPerSec, text);
    }

    // Causes the next News Flash to be a Happiness Boost
    public void BuyHappiness() { happBought = true; }

    // Causes the next News Flash to be Greenwashing
    public void BuyGreen() { greenBought = true; }
}
