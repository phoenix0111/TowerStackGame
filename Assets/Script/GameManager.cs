using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject buildingPrefab;

    public Transform crane;

    public float nextHeight = 0;

    public float blockHeight = 1f;

    CraneMove craneMove;

    void Awake()
    {
        Instance = this;

        craneMove = crane.GetComponent<CraneMove>();
    }

    public void SpawnNextBlock()
    {
        Vector3 spawnPos = crane.position + Vector3.down * 3f;

        spawnPos.y = nextHeight + 8f;

        craneMove.MoveUp(blockHeight);

        Instantiate(buildingPrefab, spawnPos, Quaternion.Euler(0,-90,0));
       
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
      
    }

}