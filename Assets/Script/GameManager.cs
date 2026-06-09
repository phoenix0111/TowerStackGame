using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Instance")]
    public static GameManager Instance;

    [Header("Essentials")]
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
    public GameObject perfectPlacementVFX;
    ParticleSystem perfectPlacementParticles;
    AudioSource starsSFX;
    public GameObject perfectImage;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverPanel;
    bool isGameOver = false;

    [Header("Audio")]
    public AudioSource ad;
    public AudioClip placementSFX;
    public AudioClip buildingFail;


    void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
        craneMove = crane.GetComponent<CraneMove>();
        perfectPlacementParticles = perfectPlacementVFX.GetComponent<ParticleSystem>();
        starsSFX = perfectPlacementVFX.GetComponent<AudioSource>();
        ad = GetComponent<AudioSource>();
    }

    public void SpawnNextBlock()
    {
        if (!isGameOver)
        {
            Vector3 spawnPos = crane.position + Vector3.down * 3f;
            spawnPos.y = nextHeight + 8f;
            craneMove.MoveUp(blockHeight);
            Instantiate(buildingPrefab, spawnPos, Quaternion.Euler(0, -90, 0));
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
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
        StartCoroutine(PerfectImage());
        ad.PlayOneShot(placementSFX);
        perfectPlacementParticles.Play();
        starsSFX.Play();
        score += perfectBlockScore;
        scoreText.text = "Score: " + score;
    }

    IEnumerator PerfectImage()
    {
        perfectImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        perfectImage.SetActive(false);
    }
}