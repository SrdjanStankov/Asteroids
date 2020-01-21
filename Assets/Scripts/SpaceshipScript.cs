using System;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private float flightSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode backwards;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode fire;

    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject projectile;

    private SpriteRenderer renderer;
    private Sprite projectileSprite;

    private float translation;
    private float rotation;

    public float FlightSpeed { get => flightSpeed; set => flightSpeed = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }


    public KeyCode Backwards { get => backwards; set => backwards = value; }
    public KeyCode Forward { get => forward; set => forward = value; }
    public KeyCode Left { get => left; set => left = value; }
    public KeyCode Right { get => right; set => right = value; }
    public KeyCode Fire { get => fire; set => fire = value; }
    public int Points { get; set; }
    public float Score { get; set; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public int Lives { get => lives; set => lives = value; }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Sprite;
        switch (sprite.name.Split(' ')[1])
        {
            case "Blue":
                projectileSprite = Resources.Load<Sprite>("Laser Blue");
                break;
            case "Green":
                projectileSprite = Resources.Load<Sprite>("Laser Green");
                break;
            case "Purple":
                projectileSprite = Resources.Load<Sprite>("Laser Purple");
                break;
            case "Red":
                projectileSprite = Resources.Load<Sprite>("Laser Red");
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        translation = 0;

        translation = Input.GetKey(Forward) ? 1 : Input.GetKey(Backwards) ? -1 : 0;

        rotation = Input.GetKey(Left) ? 1 : Input.GetKey(Right) ? -1 : 0;

        transform.Rotate(0, 0, rotation * Time.deltaTime * RotationSpeed);
        transform.Translate(0, translation * Time.deltaTime * FlightSpeed, 0);

        if (Input.GetKeyDown(Fire))
        {
            var projectileScript = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<ProjectileScript>();
            var ang = transform.rotation.eulerAngles.z;
            projectileScript.Angle = ang;
            projectileScript.Sprite = projectileSprite;
            projectileScript.Spaceship = this;
        }
    }

    public void AddPoints(float amount)
    {
        Score += amount;
    }

    public void RemoveLife(int amount)
    {
        Lives--;
        if (Lives == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameController>().RemovePlayer(gameObject);
    }
}
