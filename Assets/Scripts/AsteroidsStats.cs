using UnityEngine;

public class AsteroidsStats : MonoBehaviour
{
    [SerializeField] private float pointsWorthSmall;
    [SerializeField] private float pointsWorthMedium;
    [SerializeField] private float pointsWorthLarge;
    [SerializeField] private AsteroidType type;

    public float PointsWorth
    {
        get
        {
            switch (Type)
            {
                case AsteroidType.Large:
                    return pointsWorthLarge;
                case AsteroidType.Medium:
                    return pointsWorthMedium;
                case AsteroidType.Small:
                    return pointsWorthSmall;
                default:
                    return 0;
            }
        }
    }

    public AsteroidType Type { get => type; set => type = value; }
}
