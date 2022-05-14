using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("-- UI Menus --")]
    public GameObject Canvas;
    public GameObject MainMenu;
    public GameObject GameMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public TextMeshProUGUI LevelsText;
    [SerializeField] public TextMeshProUGUI currMoneyText;
    [SerializeField] public TextMeshProUGUI totalMoneyText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
