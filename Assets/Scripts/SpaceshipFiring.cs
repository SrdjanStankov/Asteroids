using UnityEngine;

public class SpaceshipFiring : MonoBehaviour
{
    [SerializeField] private KeyCode fire;
    [SerializeField] private GameObject projectile;

    private Sprite projectileSprite;

    public KeyCode Fire { get => fire; set => fire = value; }

    private void Start()
    {
        switch (GetComponent<SpriteRenderer>().sprite.name.Split(' ')[1])
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
