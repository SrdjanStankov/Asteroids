using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
    [SerializeField] private float flightSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode backwards;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    [SerializeField] private Sprite sprite;
    private SpriteRenderer renderer;
    private float translation;
    private float rotation;

    public float FlightSpeed { get => flightSpeed; private set => flightSpeed = value; }
    public float RotationSpeed { get => rotationSpeed; private set => rotationSpeed = value; }


    public KeyCode Backwards { get => backwards; private set => backwards = value; }
    public KeyCode Forward { get => forward; set => forward = value; }
    public KeyCode Left { get => left; set => left = value; }
    public KeyCode Right { get => right; set => right = value; }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    private void Update()
    {
        translation = 0;
        rotation = -Input.GetAxis("Horizontal");

        if (Input.GetKey(Forward))
        {
            translation = 1;
        }
        else if (Input.GetKey(Backwards))
        {
            translation = -1;
        }

        if (Input.GetKey(Left))
        {
            rotation = 1;
        }
        else if (Input.GetKey(Right))
        {
            rotation = -1;
        }

        transform.Rotate(0, 0, rotation * Time.deltaTime * RotationSpeed);
        transform.Translate(0, translation * Time.deltaTime * FlightSpeed, 0);
    }
}
