using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform target;
    public float delay = 0.13f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, delay);
    }
}