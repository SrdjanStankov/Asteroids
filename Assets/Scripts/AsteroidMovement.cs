using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle;

    public float Angle { get => angle; set => angle = value; }
    public float Speed { get => speed; set => speed = value; }

    private void Start()
    {
        transform.Rotate(0, 0, Angle);
    }

    private void Update()
    {
        transform.Translate(0, Time.deltaTime * Speed, 0);
    }
}
