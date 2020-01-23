using UnityEngine;

public class SpaceshipAttribute : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private string playerName;
    private Color color;

    public float Score { get; private set; }
    public int Lives { get => lives; set => lives = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public Color Color
    {
        get => color;
        set
        {
            color = value;
            switch (color.ToString())
            {
                case "RGBA(0.000, 0.000, 1.000, 1.000)":
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spaceship Blue");
                    break;
                case "RGBA(0.000, 1.000, 0.000, 1.000)":
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spaceship Green");
                    break;
                case "RGBA(1.000, 0.000, 0.000, 1.000)":
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spaceship Red");
                    break;
                case "RGBA(1.000, 0.000, 1.000, 1.000)":
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spaceship Purple");
                    break;
                default:
                    break;
            }
        }
    }

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
