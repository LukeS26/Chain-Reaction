using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // GameObject Variables
    public GameObject startMenu, inGameUI, pauseMenu;
    public GameObject dayNightWheel;
    public GameObject shop;

    // Sprite Variables

    // TextMeshProUGUI Variables

    // Boolean Variables
    public static bool gameStarted;

    // Float Variables
    public static float timeCycle;

    // Integer Variables

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        timeCycle = 0.0f;
        
        // Sets the Start Menu active, and every other Menu inactive
        startMenu.SetActive(true);
        inGameUI.SetActive(false);
        pauseMenu.SetActive(false);
    }

    // Called when the game is started
    public void StartGame()
    {
        startMenu.SetActive(false);
        inGameUI.SetActive(true);
        shop.SetActive(false);
        gameStarted = true;
    }

    // Update is called once per frame
    void Update() 
    {
        // Only updates if the game has started
        if(gameStarted)
        {
            // Resets the timeCycle when a day has passed
            if(timeCycle > 1.0f) { timeCycle = 0.0f; }

            // Spins the dayNightWheel
            dayNightWheel.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -timeCycle * 360);
        }
    }

    // Starts End Game Sequence
    void EndGame() 
    {
        gameStarted = false;
        /* StartCoroutine(OpenEndGameMenu()); */
    }

    // Opens End Game Menu
    /* IEnumerator OpenEndGameMenu()
    {
        
    } */

    // Called when the Pause Menu is opened
    public void OpenPause()
    {
        inGameUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    // Called when the Pause Menu is closed
    public void ClosePause()
    {
        inGameUI.SetActive(true);
        pauseMenu.SetActive(false);
    }

    // Toggles whether the shop menu is visible
    public void toggleShop()
    {
        shop.SetActive(!shop.activeSelf);
    }
}
