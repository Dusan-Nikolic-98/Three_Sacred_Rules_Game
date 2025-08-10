using UnityEngine;

public class FloatingCollectable : MonoBehaviour
{

    public float floatStrength = 0.2f;
    public float floatSpeed = 2f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = initialPosition + new Vector3(0f, yOffset, 0f);
    }
}
