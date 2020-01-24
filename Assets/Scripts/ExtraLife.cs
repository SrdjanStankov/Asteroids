using UnityEngine;

public class ExtraLife : Pickupable
{
    protected override void OnPickup(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpaceshipAttribute>().Lives++;
    }
}
