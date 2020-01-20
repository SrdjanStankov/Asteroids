using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
    [SerializeField] private float flightSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode backwards;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode fire;

    [SerializeField] private Sprite sprite;
    private SpriteRenderer renderer;
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

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    private void Update()
    {
        translation = 0;

        translation = Input.GetKey(Forward) ? 1 : Input.GetKey(Backwards) ? -1 : 0;

        rotation = Input.GetKey(Left) ? 1 : Input.GetKey(Right) ? -1 : 0;

        transform.Rotate(0, 0, rotation * Time.deltaTime * RotationSpeed);
        transform.Translate(0, translation * Time.deltaTime * FlightSpeed, 0);
    }
}
