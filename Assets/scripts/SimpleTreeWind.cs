using UnityEngine;

public class SimpleTreeWind : MonoBehaviour
{
    public float speed = 1f;
    public float amount = 2f;

    private Vector3 startRotation;

    void Start()
    {
        startRotation = transform.localEulerAngles;
    }

    void Update()
    {
        float sway = Mathf.Sin(Time.time * speed) * amount;
        transform.localRotation = Quaternion.Euler(
            startRotation.x,
            startRotation.y,
            startRotation.z + sway
        );
    }
}