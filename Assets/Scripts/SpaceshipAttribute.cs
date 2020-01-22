using UnityEngine;

public class SpaceshipAttribute : MonoBehaviour
{
    [SerializeField] private int lives;

    public float Score { get; private set; }
    public int Lives { get => lives; set => lives = value; }

    public void AddPoints(float amount)
    {
        Score += amount;
    }

    public void RemoveLife(int amount)
    {
        Lives -= amount;
        if (Lives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameController>().RemovePlayer(gameObject);
    }
}
