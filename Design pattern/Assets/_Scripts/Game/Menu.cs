using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject button1, button2;
    public int point;
    public TextMeshProUGUI pointText;
    public bool gameStarted; 

    public void StartGame()
    {
        point = 0; 
        button1.SetActive(false);
        gameStarted = true;
    }

    public void StopGame()
    {
        gameStarted = false;
        button2.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        SetCountText();
    }

    public void AddPoint()
    {
        point = point + 1;
        SetCountText();
    }

    void SetCountText()
    {
        pointText.text = "Point: " + point.ToString();
        if (point >= 50)
        {
            StopGame(); 
        }
    }

}
