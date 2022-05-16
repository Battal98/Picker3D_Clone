using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("-- UI Menus --")]
    public GameObject Canvas;
    public GameObject MainMenu;
    public GameObject GameMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public GameObject TaptapMenus;

    [Header("-- Texts --")]
    public TextMeshProUGUI LevelsText;

    [Header("-- Sprites --")]
    public Image FillImage;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
