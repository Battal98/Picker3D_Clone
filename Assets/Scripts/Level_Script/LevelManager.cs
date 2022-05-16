using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [System.Serializable]
    public class Waves {

        public int WaveBallLimit = 5;
        public TextMeshPro WaveLimitText;
        public GameObject Wave;
        public List<GameObject> WaveBalls = new List<GameObject>();
        public GameObject ScalebleGround;
        public Vector3 OldScale;
        public int CurrentBallIndex;
        public List<GameObject> gateObjs = new List<GameObject>();
    }

    public int WaveCount = 0;
    public List<Waves> WavesList = new List<Waves>();

    private float _oldSpeed;
    private bool _isFinalRide = false;
    private Rigidbody _playerRb;

    #region TapTap Jobs

    public bool IsTapTapTime = false;
    public float MaxTapTapCount;
    public float IncreaseTapTapBarValue;
    private float _currentTaptapCount = 0;

    [SerializeField]
    private float expValue;

    #endregion

    #region Final Rainbow Ground Values 

    [Header("Rainbow Props")]
    [SerializeField]
    private GameObject RainbowGroundObj;
    [SerializeField]
    private int RainbowGroundPoolCount = 10;
    [SerializeField]
    private Transform rainbowStartPoint;
    [SerializeField]
    private Transform groundSpawner;
    private List<GameObject> groundSpawns = new List<GameObject>();
    private float _color;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
            return;

        if (_isFinalRide)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isFinalRide = false;
                GameManager.instance.PlayerForwardSpeed = _oldSpeed * 3;
                IsTapTapTime = true;
            }
        }

        TapTap();

    }

    private void Init()
    {
        for (int i = 0; i < WavesList.Count; i++)
        {
            WavesList[i].Wave.name = "Wave " + i;
            WavesList[i].WaveLimitText.name = "WaveText " + i;
            WavesList[i].ScalebleGround.name = "ScalebleGround " + i;
            WavesList[i].WaveLimitText.text = "0/" + WavesList[i].WaveBallLimit.ToString();

            WavesList[i].OldScale = WavesList[i].ScalebleGround.transform.localScale;
            WavesList[i].ScalebleGround.transform.localScale = Vector3.zero;
        }

        _oldSpeed = GameManager.instance.PlayerForwardSpeed;
        _playerRb = PlayerForward.instance.PlayerPrefab.GetComponentInParent<Rigidbody>();
        RainBowGroundPool();
    }

    public async void WaitForCalculated()
    {

        await Task.Delay(2000);

        if (WavesList[WaveCount].CurrentBallIndex >= WavesList[WaveCount].WaveBallLimit)
        {
            #region Clear Wave Balls Jobs

            ClearWaveBalls();

            #endregion

            #region Scale Up Ground & Open Door/Gate Jobs

            #region Gate Jobs
            // 0 - Left, 1 - Right
            WavesList[WaveCount].gateObjs[0].transform.DOLocalRotate(new Vector3(0, 0, 90), 0.5f);
            WavesList[WaveCount].gateObjs[1].transform.DOLocalRotate(new Vector3(0, 0, -90), 0.5f);

            #endregion

            #region Ground Jobs

            WavesList[WaveCount].ScalebleGround.transform.DOScale(WavesList[WaveCount].OldScale, 0.5f).OnComplete(() =>
            {
                if (_isFinalRide)
                {
                    GameManager.instance.PlayerForwardSpeed = _oldSpeed / 2f;
                    _oldSpeed = GameManager.instance.PlayerForwardSpeed;
                }
                else
                    GameManager.instance.PlayerForwardSpeed = _oldSpeed;

            });

            #endregion


            #endregion

            #region InitWave

            WaveCount++;
            if (WaveCount < WavesList.Count)
            {
                WavesList[WaveCount] = WavesList[WaveCount];
                WavesList[WaveCount].CurrentBallIndex = 0;
            }
            else
            {
                _isFinalRide = true;
 
                //GameManager.instance.OnLevelCompleted();
            }
            return;

            #endregion

        }

        else
        {
            #region Clear Wave Balls Jobs

            ClearWaveBalls();

            #endregion

            GameManager.instance.OnLevelFailed();
            return;
        }
    }

    public void IncreaseText()
    {
        WavesList[WaveCount].WaveLimitText.text = WavesList[WaveCount].CurrentBallIndex.ToString() + "/" + WavesList[WaveCount].WaveBallLimit.ToString();
    }

    private void ClearWaveBalls()
    {
        if (WavesList[WaveCount].WaveBalls != null)
        {
            for (int i = 0; i < WavesList[WaveCount].WaveBalls.Count; i++)
            {
                WavesList[WaveCount].WaveBalls[i].SetActive(false);
            }
        }
        WavesList[WaveCount].WaveBalls.Clear();
    }

    private void TapTap()
    {
        if (IsTapTapTime)
        {
            if (_currentTaptapCount > 0)
                _currentTaptapCount -= Time.deltaTime;
            else
                _currentTaptapCount = 0;

            if (Input.GetMouseButtonDown(0))
            {
                if (_currentTaptapCount < MaxTapTapCount - 0.1f)
                    _currentTaptapCount += IncreaseTapTapBarValue;
                    
            }
        }
    }
    public async void FlyForce()
    {
        IsTapTapTime = false;

        GameManager.instance.OnLevelEnded();

        if (_currentTaptapCount <= 0)
            _currentTaptapCount = 2.5f;

        #region RigidBody & AddForce Jobs

        _playerRb.constraints = RigidbodyConstraints.None;
        _playerRb.useGravity = true;
        _playerRb.AddExplosionForce(_playerRb.mass * _currentTaptapCount * expValue, _playerRb.transform.position, _playerRb.mass, _playerRb.mass);
        DOTween.To(() => GameManager.instance.PlayerForwardSpeed, x => GameManager.instance.PlayerForwardSpeed = x, 0, 1f);

        #endregion

        int timer = (int)_currentTaptapCount;
        await Task.Delay(1000 * timer);

        GameManager.instance.OnLevelCompleted();
    }

    private void SetColor(GameObject _coloredObj)
    {
        _color += 0.2f;
        if (_color >= 0.9f)
        {
            _color = 0;
        }
        _coloredObj.GetComponentInChildren<Renderer>().material.color = Color.HSVToRGB(_color, 1, 1);
    }

    private void SetText(GameObject _getTextObj, int _multiplyValue)
    {
        int _value = (10 + _multiplyValue) * 10;
        _getTextObj.GetComponent<GroundController>().DiamondIndex = _value;
        _getTextObj.GetComponentInChildren<TextMeshPro>().text = _value.ToString();
    }

    private void RainBowGroundPool()
    {
        for (int i = 0; i < RainbowGroundPoolCount; i++)
        {
            if (i == 0)
            {
                SetRainbow(i, rainbowStartPoint.transform.position);
            }
            else
            {
                SetRainbow(i, groundSpawns[i - 1].transform.position + new Vector3(0, 0, RainbowGroundObj.transform.localScale.z));
            }
        }
    }

    private void SetRainbow(int _index, Vector3 _pos)
    {
        GameObject _ground = Instantiate(RainbowGroundObj, _pos, Quaternion.identity, groundSpawner);
        groundSpawns.Add(_ground);
        SetColor(_ground);
        SetText(_ground, _index);
    }
}
