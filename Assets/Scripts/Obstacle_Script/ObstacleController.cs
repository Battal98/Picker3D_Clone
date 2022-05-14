using UnityEngine;
using DG.Tweening;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] ParticleSystem ChangingPS;
    public enum ObstacleMovement
    {
        None, Spinner, Saw, Pushing, Smasher
    }
    public enum ObstacleType
    {
        None, Fire, Hand, Punch
    }
    public ObstacleMovement ObsMov = ObstacleMovement.Pushing; // For Movement
    public ObstacleType ObsType = ObstacleType.None; // For Type of Machine
    [System.Serializable]
    public class PushOptions
    {
        public float Speed = 2f;//Move Side    
        public bool Status = false; //Status
        public Vector2 minMaxPushValueX;
    }
    [System.Serializable]
    public class SawOptions
    {
        public float spinSpeed = 700;
        public float direction = 1;
        [Tooltip("Give 1 whichever angle you want it to rotate.")]
        public Vector3 angle;
    }
    [System.Serializable]
    public class SmasherOptions
    {
        public float Speed = 2f;//Move Side    
        public bool Status = false; //Status
        public Vector2 minMaxPushValueX;
    }
    public SmasherOptions smasher;
    public PushOptions push;
    public SawOptions saw;

    private void Start()
    {
        push.Status = true;
    }
    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            return;
        }
        XAxisMovement();
        YAxisMovement();
        RotateMovement();
    }

    void XAxisMovement()
    {
        if (ObsMov == ObstacleMovement.Pushing)
        {
            if (transform.position.x > push.minMaxPushValueX.x)
            {
                push.Status = false;
            }
            if (transform.position.x < push.minMaxPushValueX.y)
            {
                push.Status = true;
            }
            if (push.Status == true)
            {
                transform.Translate(push.Speed * Time.deltaTime, 0, 0);
            }
            if (push.Status == false)
            {
                transform.Translate(-push.Speed * Time.deltaTime, 0, 0);
            }
        }
    }

    void YAxisMovement()
    {
        if (ObsMov == ObstacleMovement.Smasher)
        {
            if (transform.position.y > smasher.minMaxPushValueX.x)
            {
                smasher.Status = false;
            }
            if (transform.position.y < smasher.minMaxPushValueX.y)
            {
                smasher.Status = true;
            }
            if (smasher.Status == true)
            {
                transform.Translate(0, smasher.Speed * Time.deltaTime, 0);
            }
            if (smasher.Status == false)
            {
                transform.Translate(0, -smasher.Speed * Time.deltaTime, 0);
            }
        }
    }

    void RotateMovement()
    {
        if (ObsMov == ObstacleMovement.Saw)
        {
            transform.Rotate(new Vector3(saw.angle.x, saw.angle.y, saw.angle.z) * Time.deltaTime * saw.spinSpeed * saw.direction);
        }
    }
}
