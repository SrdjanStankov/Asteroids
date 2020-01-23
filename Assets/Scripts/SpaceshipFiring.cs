using UnityEngine;

public class SpaceshipFiring : MonoBehaviour
{
    [SerializeField] private KeyCode fire;
    [SerializeField] private GameObject projectile;

    private Sprite projectileSprite;

    public KeyCode Fire { get => fire; set => fire = value; }

    private void Start()
    {
        switch (GetComponent<SpaceshipAttribute>().Color.ToString())
        {
            case "RGBA(0.000, 0.000, 1.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Blue");
                break;
            case "RGBA(0.000, 1.000, 0.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Green");
                break;
            case "RGBA(1.000, 0.000, 0.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Red");
                break;
            case "RGBA(1.000, 0.000, 1.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Purple");
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Fire))
        {
            CreateProjectile();
        }
    }

    private void CreateProjectile()
    {
        var project = Instantiate(projectile, transform.position, Quaternion.identity);
        project.GetComponent<StraightLineMovement>().Angle = transform.rotation.eulerAngles.z;
        project.GetComponent<SpriteRenderer>().sprite = projectileSprite;
        project.GetComponent<ProjectileAttributes>().Spaceship = GetComponent<SpaceshipAttribute>();
    }
}
