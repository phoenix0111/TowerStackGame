using UnityEngine;

public class CraneMove : MonoBehaviour
{
    public float moveRange = 4f;
    public float moveSpeed = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * moveSpeed) * moveRange;

        transform.position = startPos + new Vector3(x, 0, 0);
    }

    public void MoveUp(float amount)
    {
        startPos.y += amount;
    }
}