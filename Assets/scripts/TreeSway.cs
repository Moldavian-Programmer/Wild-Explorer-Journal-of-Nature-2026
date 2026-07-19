using UnityEngine;

public class TreeSway : MonoBehaviour
{
    public float swayAmount = 1f;
    public float swaySpeed = 1f;

    private Quaternion startRot;

    void Start()
    {
        startRot = transform.rotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = startRot * Quaternion.Euler(0, 0, angle);
    }
}