using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle;

    private Camera camera;

    public float Angle { get => angle; set => angle = value; }
    public float Speed { get => speed; set => speed = value; }

    private void Start()
    {
        transform.Rotate(0, 0, Angle);
        camera = Camera.main;
    }

    private void Update()
    {
        transform.Translate(0, Time.deltaTime * Speed, 0);

        var negativeBoundary = camera.ScreenToWorldPoint(new Vector3(-100, -100, transform.position.z));
        var positiveBoundary = camera.ScreenToWorldPoint(new Vector3(Screen.width + 100, Screen.height + 100, transform.position.z));

        if (transform.position.x < negativeBoundary.x)
        {
            transform.position = new Vector3(positiveBoundary.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < negativeBoundary.y)
        {
            transform.position = new Vector3(transform.position.x, positiveBoundary.y, transform.position.z);
        }

        if (transform.position.x > positiveBoundary.x)
        {
            transform.position = new Vector3(negativeBoundary.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y > positiveBoundary.y)
        {
            transform.position = new Vector3(transform.position.x, negativeBoundary.y, transform.position.z);
        }
    }
}
