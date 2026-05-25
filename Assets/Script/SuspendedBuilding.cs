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

    bool spawnedNext = false;
    bool checkGroundTouch = false;
    bool checkingPlacement = false;

    private Transform oldBlockTransform;
    private float xPosition;

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

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !dropped)
        {
            Drop();
        }
    }

    void FollowCrane()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        Vector3 targetPos = crane.position;

        targetPos += new Vector3(sway, -ropeLength, 0);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
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

            if (
                collision.gameObject.CompareTag("Floor") && !checkGroundTouch && !checkingPlacement
            )
            {
                checkingPlacement = true;
                oldBlockTransform = collision.gameObject.transform;

                StartCoroutine(CheckPlacement());
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
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
        

            GameManager.Instance.nextHeight +=
                GameManager.Instance.blockHeight;

            xPosition = Mathf.Abs(transform.position.x - oldBlockTransform.position.x);

            if (xPosition < 0.1f && xPosition > -0.1)
            {
                Debug.Log(xPosition + "perfect");
                rb.isKinematic = true;
                rb.isKinematic = true;
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

