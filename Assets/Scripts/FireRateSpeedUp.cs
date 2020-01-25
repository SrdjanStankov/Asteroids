using UnityEngine;

public class FireRateSpeedUp : Pickupable
{
    private SpaceshipAttribute attribute;

    protected override void OnEndOfDuration()
    {
        try
        {
            attribute.FireRate *= 2;
        }
        catch (System.Exception) { }
        Destroy(gameObject);
    }

    protected override void OnPickup(Collider2D collision)
    {
        attribute = collision.GetComponent<SpaceshipAttribute>();
        attribute.FireRate /= 2;
    }
}