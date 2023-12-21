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
    public GameObject startMenu, inGameUI, pauseMenu, shareLoseMenu, popLoseMenu, legalMenu;
    public GameObject dayNightWheel;
    public GameObject shop;

    // TextMeshProUGUI Variables
    public TextMeshProUGUI moneyText, avgWorkHapText;
    public TextMeshProUGUI ceoHapText, custHapText;
    public TextMeshProUGUI numWorkersText;

    // Boolean Variables
    public static bool gameStarted, gamePaused;

    // Float Variables
    public static float timeCycle;
    public static float money, avgWorkHap, ceoHap, custHap;

    // Integer Variables
    public static int numWorkers;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        gamePaused = false;
        timeCycle = 0.0f;
        
        // Sets the Start Menu active, and every other Menu inactive
        startMenu.SetActive(true);
        inGameUI.SetActive(false);
        pauseMenu.SetActive(false);
        shareLoseMenu.SetActive(false);
        popLoseMenu.SetActive(false);
        legalMenu.SetActive(false);
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
            dayNightWheel.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180-(timeCycle * 360));

            UpdateHappinessLevels();
        }
    }

    // Called when the Pause Menu is opened
    public void OpenPause()
    {
        inGameUI.SetActive(false);
        pauseMenu.SetActive(true);
        gamePaused = true;
    }

    // Called when the Pause Menu is closed
    public void ClosePause()
    {
        inGameUI.SetActive(true);
        pauseMenu.SetActive(false);
        gamePaused = false;
    }

    // Called when the Legal Menu is opened
    public void OpenLegal()
    {
        legalMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    // Called when the Legal Menu is closed
    public void CloseLegal()
    {
        legalMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    // Toggles whether the shop menu is visible
    public void ToggleShop()
    {
        shop.SetActive(!shop.activeSelf);
    }

    // Updates the happiness level for several components, and how much money the player has
    void UpdateHappinessLevels()
    {
        // Updates text in UI
        moneyText.text = "Money: $" + money;
        avgWorkHapText.text = "Average Worker Happiness: " + avgWorkHap;
        ceoHapText.text = "Shareholder Happiness: " + ceoHap;
        custHapText.text = "Customer Happiness: " + custHap;
        numWorkersText.text = "Workers Employed: " + numWorkers;
    }

    // Resets the game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Opens Lose by Shares Lose Screen
    public void SharesLose()
    {
        // Makes sure only one lose menu is open at a time
        if(!popLoseMenu.activeSelf)
        {
            inGameUI.SetActive(false);
            shareLoseMenu.SetActive(true);
        }
    }

    // Opens Lose by Popularity Lose Screen
    public void PopularityLose()
    {
        // Makes sure only one lose menu is open at a time
        if(!shareLoseMenu.activeSelf)
        {
            inGameUI.SetActive(false);
            popLoseMenu.SetActive(true);
        }
    }
}
