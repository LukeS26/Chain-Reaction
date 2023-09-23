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

    // Sprite Variables

    // TextMeshProUGUI Variables

    // Boolean Variables
    public static bool gameStarted;

    // Float Variables

    // Integer Variables

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        
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
        gameStarted = true;
    }

    // Update is called once per frame
    void Update() 
    {
        
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
}
