using UnityEngine;

public class SwingMovement : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddTorque(Vector3.forward * 20000f);
        Debug.Log("SwingMovement Start");
    }
    void Update()
    {
       // float angle = Mathf.Sin(Time.time * swingSpeed) * maxAngle;
       // transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}