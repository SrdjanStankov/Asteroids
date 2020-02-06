using UnityEngine;

public class ExtraLife : Pickupable
{
    protected override void OnPickup(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpaceshipAttribute>().Lives++;
        GetComponent<AudioSource>().Play();
    }

    protected override void OnEndOfDuration()
    {
        Destroy(gameObject);
    }
}
