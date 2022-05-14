using UnityEngine;

public class PlayerForward : MonoBehaviour
{
    public static PlayerForward instance;
    public GameObject PlayerPrefab;
    [SerializeField]
    private Rigidbody rb;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            return;
        }

        if (rb != null)
        {
            rb.velocity = Vector3.forward * Time.deltaTime* GameManager.instance.PlayerForwardSpeed;
        }
    }
}
