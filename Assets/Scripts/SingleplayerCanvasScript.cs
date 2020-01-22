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

    private SpaceshipAttribute spaceship;

    private void Start()
    {
        controller = FindObjectOfType<GameController>();
        spaceship = FindObjectOfType<SpaceshipAttribute>();
    }

    private void Update()
    {
        ScoreText.text = $"Score: {spaceship.Score}";
        livesLeftText.text = $"Lives: {spaceship.Lives}";
        playerName.text = $"Player: {spaceship.PlayerName}";
        lvlText.text = $"Level: {controller.CurrentLvl}";
        remainingAsteroidsText.text = $"Asteroids remaining: {controller.AsteroidsToDestroy}";
    }
}
