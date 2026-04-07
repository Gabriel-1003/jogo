using UnityEngine;

public class LapManager : MonoBehaviour
{
    public Transform[] checkpoints;
    private int currentCP = 0;
    public int lapsCompleted = 0;
    public int totalLaps = 3;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint") && other.transform == checkpoints[currentCP])
        {
            currentCP = (currentCP + 1) % checkpoints.Length;
            if (currentCP == 0)
            {
                lapsCompleted++;
                Debug.Log($"✅ Volta {lapsCompleted} concluída!");
                if (lapsCompleted >= totalLaps)
                    Debug.Log("🏁 CORRIDA FINALIZADA!");
            }
        }
    }
}
