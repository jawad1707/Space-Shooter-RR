using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private Sprite[] _liveSprite;
    [SerializeField]
    private Image _currentLive;
    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private GameObject _restart;
    [SerializeField]
    private GameObject _pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: ";
    }

    // Update is called once per frame
    void Update()
    {
      if(_restart.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("GameRR");
            }
        }

        PauseMenu();
    }
    public void UpdateScore(int points)
    {
        _scoreText.text = "Score: " + points;
    }

    public void UpdateLives(int lifecount)
    {
        _currentLive.sprite = _liveSprite[lifecount];
    }

    public void GameOver()
    {

        StartCoroutine(FlickerRoutine());
        
    }

    public void RestartGame()
    {
        _restart.SetActive(true);
    }

    public void PauseMenu()
    {
        if(_pauseMenu.activeInHierarchy == false && Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
        }

        else if(_pauseMenu.activeInHierarchy == true && Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1;
            _pauseMenu.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }


    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            _gameOver.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            _gameOver.SetActive(false);
            yield return new WaitForSeconds(0.2f);

        }

    }
}
