using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;


    public Transform[] startPoint;
    public Transform[] path;

    public int souls;
    public int defenseHealth;

    public TextMeshProUGUI soulsText;
    public TextMeshProUGUI defenseHealthText;


    public static bool isGameOver = false;

    public GameObject gameOverUI;

    public List<GameObject> uiElements;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        souls = 100;
        defenseHealth = 10;
        gameOverUI.SetActive(false);
        UpdateSoulsText();
        UpdateDefenseHealthText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // If 'r' is pressed, reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void IncreaseSouls(int amount)
    {
        souls += amount;
        UpdateSoulsText();
    }


    public bool SpendSouls(int amount)
    {
        if (amount <= souls)
        {
            souls -= amount;
            UpdateSoulsText();
            return true;


        } else {
            return false;
        }
    }

    public void DecreaseDefenseHealth(int amount)
    {
        defenseHealth -= amount;
        UpdateDefenseHealthText();
        if (defenseHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);

        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }

        isGameOver = true;

    }

    void UpdateSoulsText()
    {
        soulsText.text = "Souls: " + souls;
    }

    void UpdateDefenseHealthText()
    {
        defenseHealthText.text = "Health: " + defenseHealth;
    }

}
