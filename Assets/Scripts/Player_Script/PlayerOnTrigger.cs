using System.Collections.Generic;
using UnityEngine;

public class PlayerOnTrigger : MonoBehaviour
{
    #region Singleton

    public static PlayerOnTrigger Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #endregion

    [HideInInspector]
    public List<GameObject> InsideBalls = new List<GameObject>();

    [SerializeField]
    private float _expSpeed;
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "StopTrigger":
                GameManager.instance.PlayerForwardSpeed = 0;

                #region Add Force For Balls Jobs

                for (int i = 0; i < InsideBalls.Count; i++)
                {
                    InsideBalls[i].GetComponentInChildren<Rigidbody>().AddExplosionForce(_expSpeed, InsideBalls[i].transform.position, 20f, 20f);
                    //InsideBalls[i].transform.DORotate(Vector3.zero, 0.1f).OnComplete(() => InsideBalls[i].GetComponentInChildren<Rigidbody>().DOMoveZ(InsideBalls[i].transform.position.z + 8f, 1f));

                }

                #endregion

                break;

            case "Balls":
                InsideBalls.Add(other.gameObject.transform.parent.gameObject);
                break;

            case "FlyTrigger":
                LevelManager.instance.FlyForce();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Balls":

                if (InsideBalls.Count == 1)
                {
                    InsideBalls[0].gameObject.transform.GetChild(0).tag = "FinalBall";
                }
                InsideBalls.Remove(other.gameObject.transform.parent.gameObject);
                break;
        }
    }
}
