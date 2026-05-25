using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject buildingPrefab;

    public Transform crane;

    public float nextHeight = 0;

    public float blockHeight = 1f;

    CraneMove craneMove;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;
    public int placedBlockScore = 10;
    public int perfectBlockScore = 20;
    [SerializeField] GameObject starsVFX;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverPanel;
    bool isGameOver = false;


    void Awake()
    {
        Instance = this;

        craneMove = crane.GetComponent<CraneMove>();
    }

    public void SpawnNextBlock()
    {
        if (!isGameOver) 
        {
        Vector3 spawnPos = crane.position + Vector3.down * 3f;

        spawnPos.y = nextHeight + 8f;

        craneMove.MoveUp(blockHeight);

        Instantiate(buildingPrefab, spawnPos, Quaternion.Euler(0,-90,0));
       }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        isGameOver = true;
        gameOverPanel.SetActive(true);


    }

    public void BlockPlaced()
    {
        score += placedBlockScore;
        scoreText.text = "Score: " + score;
    }


    public void PerfectBlockPlaced()
    {
        score += perfectBlockScore;
        scoreText.text = "Score: " + score;
    }

}