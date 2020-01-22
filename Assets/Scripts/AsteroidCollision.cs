using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // collision asteroids with projectile
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var asteroidsStats = GetComponent<AsteroidsStats>();
        collision.gameObject.GetComponent<ProjectileScript>().Spaceship.AddPoints(asteroidsStats.PointsWorth);
        gameController.DestroyAsteroid(asteroidsStats);
        Destroy(collision.gameObject);
    }

    // collision asteroid with spaceship
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpaceshipScript>().RemoveLife(1);
    }
}
