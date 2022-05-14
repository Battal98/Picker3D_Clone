using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    #region Swerve Movement Value

    [SerializeField] 
    private Vector2 minMaxPlayerPos;
    [SerializeField] 
    private Vector2 minMaxPlayerSensivity;
    [SerializeField] 
    private float calculatedXSens = 0.75f;

    private GameObject _offsetObj;
    private Vector3 _oldPosition = Vector3.zero;
    private Vector3 _temp, _temp2;
    private float _distanceFixer = 0;
    //private float _lastX = 0;

    enum PlayerDirection
    {
        none,
        left,
        right
    }
    PlayerDirection playerDirection = PlayerDirection.none;
    PlayerDirection detectingDirection = PlayerDirection.none;

    #endregion

    private void Awake()
    {
        if (instance == null) 
        { 
            instance = this; 
        }

    }

    private void Start()
    {
        _offsetObj = new GameObject();
        _offsetObj.name = "OffSetObje";
        _offsetObj.transform.parent = this.transform.parent;
    }

    private void FixedUpdate()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            return;
        }

        SwerveMovements();

    }

    #region Swerve Jobs
    private void SwerveMovements()
    {
        #region Movement 
        if (Input.GetMouseButtonDown(0))
        {
            _offsetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
            _temp = this.transform.localPosition - _offsetObj.transform.localPosition;
            _distanceFixer = Vector3.Distance(this.transform.position, _offsetObj.transform.localPosition);
            _oldPosition = _offsetObj.transform.localPosition;
        }
        if (Input.GetMouseButton(0))
        {
            _offsetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
           // DetectPlayerDirection();
            _temp2 = _offsetObj.transform.localPosition + _temp;
            _temp2.y = this.transform.localPosition.y;
            _temp2.z = 0;
            _temp2.x = Mathf.Clamp(_temp2.x, minMaxPlayerPos.x, minMaxPlayerPos.y);

            if (detectingDirection == PlayerDirection.none || detectingDirection != playerDirection)
                this.transform.localPosition = _temp2;

            if (_distanceFixer - 0.1f > Vector3.Distance(this.transform.position, _offsetObj.transform.position))
            {
                _offsetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
                _temp = this.transform.localPosition - _offsetObj.transform.localPosition;
                _distanceFixer = Vector3.Distance(this.transform.position, _offsetObj.transform.localPosition);
            }
            //RotationCalculator();

        }
        if (!Input.GetMouseButton(0))
        {

            _offsetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
            _temp = this.transform.localPosition - _offsetObj.transform.localPosition;
            _distanceFixer = Vector3.Distance(this.transform.position, _offsetObj.transform.localPosition);
            //playerDirection = PlayerDirection.none;
        }
        // this.transform.localEulerAngles = Vector3.zero;
        #endregion
    }

    private float CalculateX()
    {
        Vector3 location = Input.mousePosition;
        return (location.x / (Screen.width / (minMaxPlayerSensivity.y + Mathf.Abs(minMaxPlayerSensivity.x)))) - calculatedXSens;

    }


    #endregion


}
