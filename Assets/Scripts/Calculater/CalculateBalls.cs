using UnityEngine;

public class CalculateBalls : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Balls"))
        {
            LevelManager.instance.WavesList[LevelManager.instance.WaveCount].CurrentBallIndex++;
            LevelManager.instance.IncreaseText();

        }

        if (other.gameObject.CompareTag("FinalBall"))
        {
            LevelManager.instance.WaitForCalculated();
        }
    }
}
