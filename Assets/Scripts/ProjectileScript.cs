using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle;

    private Camera camera;
    private Sprite sprite;
    private SpriteRenderer renderer;

    public float Speed { get => speed; set => speed = value; }
    public float Angle { get => angle; set => angle = value; }
    public Sprite Sprite
    {
        get => sprite;
        set
        {
            sprite = value;
            renderer.sprite = value;
        }
    }
    public SpaceshipScript Spaceship { get; set; }

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.Rotate(0, 0, Angle);
        camera = Camera.main;
    }

    private void Update()
    {
        transform.Translate(0, Time.deltaTime * Speed, 0);
        var worldPointMax = camera.ScreenToWorldPoint(new Vector3(Screen.width + 100, Screen.height + 100, 0));
        var worldPointMin = camera.ScreenToWorldPoint(new Vector3(-100, -100, 0));
        if (transform.position.x > worldPointMax.x || transform.position.x < worldPointMin.x ||
            transform.position.y > worldPointMax.y || transform.position.y < worldPointMin.y)
        {
            Destroy(gameObject);
        }
    }
}
