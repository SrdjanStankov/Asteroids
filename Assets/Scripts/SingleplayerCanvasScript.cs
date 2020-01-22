using TMPro;
using UnityEngine;

public class SingleplayerCanvasScript : MonoBehaviour
{
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text livesLeftText;
    [SerializeField] private TMP_Text remainingAsteroidsText;

    private GameController controller;

    public SpaceshipAttribute Spaceship { get; set; }

    private void Start()
    {
        controller = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        ScoreText.text = $"Score: {Spaceship.Score}";
        livesLeftText.text = $"Lives: {Spaceship.Lives}";
        playerName.text = $"Player: {Spaceship.PlayerName}";
        lvlText.text = $"Level: {controller.CurrentLvl}";
        remainingAsteroidsText.text = $"Asteroids remaining: {controller.AsteroidsToDestroy}";
    }
}
