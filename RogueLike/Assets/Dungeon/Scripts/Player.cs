using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public float rotationSpeed = 20;
    public float speed = 4f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 directionaVector = new Vector3(h, 0, v);
        if (directionaVector.magnitude > Mathf.Abs(0.05f))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionaVector), Time.deltaTime * rotationSpeed);
        }

        rb.velocity = Vector3.ClampMagnitude(directionaVector, 1) * speed;
    }
}