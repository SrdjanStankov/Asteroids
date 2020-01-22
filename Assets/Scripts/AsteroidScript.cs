using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle;

    private Camera cameraMain;
    private GameController gameController;

    public float Angle { get => angle; set => angle = value; }
    public float Speed { get => speed; set => speed = value; }
    public float PointsWorth { get; set; }
    public AsteroidType Type { get; set; }

    private void Start()
    {
        transform.Rotate(0, 0, Angle);
        cameraMain = Camera.main;
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        transform.Translate(0, Time.deltaTime * Speed, 0);

        var negativeBoundary = cameraMain.ScreenToWorldPoint(new Vector3(-100, -100, transform.position.z));
        var positiveBoundary = cameraMain.ScreenToWorldPoint(new Vector3(Screen.width + 100, Screen.height + 100, transform.position.z));

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

    internal void DestroyAsteroid(AsteroidType asteroid)
    {
        switch (asteroid)
        {
            case AsteroidType.Large:
                gameController.CreateAsteroid(AsteroidType.Medium, transform.position);
                gameController.CreateAsteroid(AsteroidType.Medium, transform.position);
                gameController.DestroyAsteroid(gameObject);
                break;
            case AsteroidType.Medium:
                gameController.CreateAsteroid(AsteroidType.Small, transform.position);
                gameController.CreateAsteroid(AsteroidType.Small, transform.position);
                gameController.DestroyAsteroid(gameObject);
                break;
            case AsteroidType.Small:
                gameController.DestroyAsteroid(gameObject);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<ProjectileScript>().Spaceship.AddPoints(PointsWorth);
        DestroyAsteroid(Type);
        Destroy(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpaceshipScript>().RemoveLife(1);
    }
}
