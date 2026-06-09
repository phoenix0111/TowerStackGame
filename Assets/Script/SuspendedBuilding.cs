using System.Collections;
using UnityEngine;

public class SuspendedBuilding : MonoBehaviour
{
    public Transform crane;
    Rigidbody rb;
    bool dropped = false;

    [Header("Suspension")]
    public float ropeLength = 5f;
    public float swayAmount = 2f;
    public float swaySpeed = 2f;

    [Header("Follow")]
    public float smoothTime = 0.2f;
    Vector3 velocity;

    [Header("Tower Placement")]
    bool spawnedNext = false;
    bool checkGroundTouch = false;
    bool checkingPlacement = false;
    private Transform oldBlockTransform;
    private float xPosition;
    public float perfectPlacementThreshold = 0.13f;

    public bool isMainMenu;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        crane = FindAnyObjectByType<CraneMove>().gameObject.transform;

        rb.isKinematic = true;
    }

    void Update()
    {
        if (!dropped)
        {
            FollowCrane();
        }

        if ((Input.GetMouseButtonDown(0) && !isMainMenu || Input.GetKeyDown(KeyCode.Space)) && !isMainMenu && !dropped)
        {
            Drop();
        }
    }

    void FollowCrane()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        Vector3 targetPos = crane.position;

        targetPos += new Vector3(sway, -ropeLength, 0);

        transform.position = Vector3.SmoothDamp(transform.position,targetPos,ref velocity, smoothTime);
    }

    void Drop()
    {
        dropped = true;

        transform.parent = null;

        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!spawnedNext && dropped)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                checkGroundTouch = true;
                return;
            }

            if ( collision.gameObject.CompareTag("Floor") && !checkGroundTouch && !checkingPlacement )
            {
                checkingPlacement = true;
                oldBlockTransform = collision.gameObject.transform;

                GameManager.Instance.ad.PlayOneShot(GameManager.Instance.placementSFX);

                StartCoroutine(CheckPlacement());
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.ad.PlayOneShot(GameManager.Instance.buildingFail);
            checkGroundTouch = true;
            GameManager.Instance.GameOver();

            return;
        }

    }

    IEnumerator CheckPlacement()
    {
        yield return new WaitForSeconds(0.5f);

        checkingPlacement = false;

        if (spawnedNext)
            yield break;

        bool notFallen = transform.position.y > 0f;

        if (checkGroundTouch)
        {
            GameManager.Instance.GameOver();

            yield break;
        }

        if (notFallen)
        {
            spawnedNext = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            GameManager.Instance.nextHeight += GameManager.Instance.blockHeight;

            xPosition = Mathf.Abs(transform.position.x - oldBlockTransform.position.x);

            if (xPosition < perfectPlacementThreshold && xPosition > -perfectPlacementThreshold)      // here it checks if the block is placed within the perfect placement threshold,
            {
                Debug.Log(xPosition + "perfect");
                rb.isKinematic = true;
                rb.isKinematic = true;

                GameManager.Instance.perfectPlacementVFX.transform.position = new Vector3(transform.position.x, transform.position.y + 2, -1.3f);

                GameManager.Instance.PerfectBlockPlaced();
            }
            else
            {
                Debug.Log(xPosition + "placed");
                GameManager.Instance.BlockPlaced();
            }

            GameManager.Instance.SpawnNextBlock();
        }
        else
        {
            Debug.Log("fallen");
        }
    }


}

