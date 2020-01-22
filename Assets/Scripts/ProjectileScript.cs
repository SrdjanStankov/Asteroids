using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle;

    private Camera cameraMain;
    private Sprite sprite;
    private SpriteRenderer rendererSprite;

    public float Speed { get => speed; set => speed = value; }
    public float Angle { get => angle; set => angle = value; }
    public Sprite Sprite
    {
        get => sprite;
        set
        {
            sprite = value;
            rendererSprite.sprite = value;
        }
    }
    public SpaceshipScript Spaceship { get; set; }

    private void Awake()
    {
        rendererSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.Rotate(0, 0, Angle);
        cameraMain = Camera.main;
    }

    private void Update()
    {
        transform.Translate(0, Time.deltaTime * Speed, 0);
        var worldPointMax = cameraMain.ScreenToWorldPoint(new Vector3(Screen.width + 100, Screen.height + 100, 0));
        var worldPointMin = cameraMain.ScreenToWorldPoint(new Vector3(-100, -100, 0));
        if (transform.position.x > worldPointMax.x || transform.position.x < worldPointMin.x ||
            transform.position.y > worldPointMax.y || transform.position.y < worldPointMin.y)
        {
            Destroy(gameObject);
        }
    }
}
