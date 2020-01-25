using UnityEngine;

public class SpeedUp : Pickupable
{
    private SpaceshipAttribute spaceshipMovement;

    protected override void OnEndOfDuration()
    {
        try
        {
            spaceshipMovement.FlightSpeed /= 2;
            spaceshipMovement.RotationSpeed /= 2;
        }
        catch (System.Exception) { } // if spaceship is destroyed before duration expires
        Destroy(gameObject);
    }

    protected override void OnPickup(Collider2D collision)
    {
        spaceshipMovement = collision.GetComponent<SpaceshipAttribute>();
        spaceshipMovement.FlightSpeed *= 2;
        spaceshipMovement.RotationSpeed *= 2;
    }
}
