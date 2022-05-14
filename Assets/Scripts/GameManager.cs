using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Cinemachine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Bools
    [Header("-- Bools --")]
    public static bool isGameStarted = false;
    public static bool isGameEnded = false;
    public static bool isGameRestarted = false;
    #endregion

    #region GameObject, TextMeshPro and Lists
    [Header("-- Lists --")]
    public List<GameObject> Levels;
    [SerializeField]
    private GameObject RainbowGroundObj;

    [Space]
    [Header("-- Cams --")]
    public CinemachineVirtualCamera vCamGame;
    //public GameObject camTarget;
    [Space]
    [Header(" *-_Player Values_-*")]
    public float PlayerForwardSpeed = 2;
    #endregion

    public int levelCount = 0;
    public int nextLevel = 0;
    private Volume _volume;

    #region Final Rainbow Ground Values 

    [Header("Rainbow Props")]
    [SerializeField]
    private int RainbowGroundPoolCount = 10;
    [SerializeField]
    private Transform rainbowStartPoint;
    [SerializeField]
    private Transform groundSpawner;
    [SerializeField]
    private List<GameObject> groundSpawns = new List<GameObject>();
    [SerializeField]
    private float _color;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _volume = this.GetComponent<Volume>();
    }

    private IEnumerator Start()
    {
        isGameStarted = false;
        isGameEnded = true;
        isGameRestarted = false;
        StartGame();
        UIManager.instance.LevelsText.text = (nextLevel + 1).ToString();
        yield return new WaitForSeconds(1);

    }
    public void StartGame()
    {
        if (isGameRestarted)
        {
            UIManager.instance.MainMenu.SetActive(false);
            _volume.enabled = false;
        }
        levelCount = PlayerPrefs.GetInt("levelCount", levelCount);
        nextLevel = PlayerPrefs.GetInt("nextLevel", nextLevel);

        if (levelCount < 0 || levelCount >= Levels.Count)
        {
            levelCount = 0;
            PlayerPrefs.SetInt("levelCount", levelCount);
        }
        CreateLevel(levelCount);
    }
    // Create Level.
    public void CreateLevel(int Levelindex)
    {
        Instantiate(Levels[Levelindex], new Vector3(0, 0, 0), Levels[Levelindex].transform.rotation);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGameEnded && !isGameStarted)
        {
            UIManager.instance.MainMenu.SetActive(false);
            UIManager.instance.GameMenu.SetActive(true);
            _volume.enabled = false;
            isGameStarted = true;
            isGameEnded = false;
        }
    }

    public IEnumerator WaitForFinish(float _waitTime)
    {
        isGameEnded = true;
        yield return new WaitForSeconds(_waitTime);
        UIManager.instance.WinMenu.SetActive(true);
        UIManager.instance.GameMenu.SetActive(false);
        _volume.enabled = true;
    }

    public void OnLevelCompleted()
    {
        StartCoroutine(WaitForFinish(1f));
    }

    //if Level Failed
    public void OnLevelFailed()
    {
        //Debug.Log("fail");
        UIManager.instance.LoseMenu.SetActive(true);
        UIManager.instance.GameMenu.SetActive(false);
        isGameEnded = true;
        _volume.enabled = true;
    }

    // When Game is Start
    public void StartTheGameButton()
    {
        UIManager.instance.MainMenu.SetActive(false);
        UIManager.instance.WinMenu.SetActive(false);
        isGameStarted = true;
        isGameEnded = false;
    }

    // Next Level Button
    public void NextLevelButton()
    {
        isGameEnded = false;
        isGameRestarted = true;
        isGameStarted = true;
        levelCount++;
        nextLevel++;
        PlayerPrefs.SetInt("levelCount", levelCount);
        PlayerPrefs.SetInt("nextLevel", nextLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Rest.
    public void RestartButton()
    {
        isGameRestarted = true;
        isGameStarted = true;
        isGameEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnLevelEnded()
    {
        isGameEnded = true;
        vCamGame.m_Follow = PlayerController.instance.transform;
        vCamGame.m_LookAt = null;
        PlayerForwardSpeed = 150f;
    }


}