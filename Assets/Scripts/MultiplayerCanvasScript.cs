using UnityEngine;
using TMPro;

public class MultiplayerCanvasScript : MonoBehaviour
{
    public RectTransform PlayerTextsPrefab;
    public TMP_Text LevelText;
    public TMP_Text RemainingAsteroidsText;

    private RectTransform[] playerTexts;
    private GameController controller;

    void Start()
    {
        controller = FindObjectOfType<GameController>();
        MultiplayerScenePlayerNumber.Number = 4;
        playerTexts = new RectTransform[MultiplayerScenePlayerNumber.Number];
        for (int i = 0; i < MultiplayerScenePlayerNumber.Number; i++)
        {
            playerTexts[i] = Instantiate(PlayerTextsPrefab, transform);
            playerTexts[i].position = new Vector3(playerTexts[i].position.x, playerTexts[i].position.y - (playerTexts[i].rect.height * i), playerTexts[i].position.z);
        }
    }

    void Update()
    {
        for (int i = 0; i < MultiplayerScenePlayerNumber.Number; i++)
        {
            var childPlayerName = playerTexts[i].GetChild(0);
            var childScore = childPlayerName.GetChild(0);
            var childLife = childScore.GetChild(0);
            
            if (controller.Players[i] is null)
            {
                childLife.GetComponent<TMP_Text>().text = $"0";
                continue;
            }

            childPlayerName.GetComponent<TMP_Text>().text = $"{controller.Players[i].GetComponent<SpaceshipAttribute>().PlayerName}";
            childScore.GetComponent<TMP_Text>().text = $"{controller.Players[i].GetComponent<SpaceshipAttribute>().Score}";
            childLife.GetComponent<TMP_Text>().text = $"{controller.Players[i].GetComponent<SpaceshipAttribute>().Lives}";
        }

        LevelText.text = $"Level: {controller.CurrentLvl}";
        RemainingAsteroidsText.text = $"Asteroids remaining: {controller.AsteroidsToDestroy}";
    }
}
