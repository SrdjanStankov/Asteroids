using UnityEngine;
using TMPro;

public class MultiplayerCanvasScript : MonoBehaviour
{
    public float spaceBetweenWords;

    public RectTransform PlayerTextsPrefab;
    public TMP_Text LevelText;
    public TMP_Text RemainingAsteroidsText;

    private RectTransform[] playerTexts;
    private GameController controller;

    private TMP_Text[] playerNameTexts;
    private TMP_Text[] playerScoreTexts;
    private TMP_Text[] playerLifeTexts;

    void Start()
    {
        controller = FindObjectOfType<GameController>();
        playerTexts = new RectTransform[MultiplayerScenePlayerNumber.Number];

        playerNameTexts = new TMP_Text[MultiplayerScenePlayerNumber.Number];
        playerScoreTexts = new TMP_Text[MultiplayerScenePlayerNumber.Number];
        playerLifeTexts = new TMP_Text[MultiplayerScenePlayerNumber.Number];

        for (int i = 0; i < MultiplayerScenePlayerNumber.Number; i++)
        {
            playerTexts[i] = Instantiate(PlayerTextsPrefab, transform);
            playerTexts[i].position = new Vector3(playerTexts[i].position.x, playerTexts[i].position.y - (playerTexts[i].rect.height * i), playerTexts[i].position.z);

            var playerNameTextObj = playerTexts[i].GetChild(0);
            playerNameTexts[i] = playerNameTextObj.GetComponent<TMP_Text>();
            playerNameTexts[i].color = controller.Players[i].GetComponent<SpaceshipAttribute>().Color;

            var playerScoreTextObj = playerNameTextObj.GetChild(0);
            playerScoreTexts[i] = playerScoreTextObj.GetComponent<TMP_Text>();
            playerScoreTexts[i].color = controller.Players[i].GetComponent<SpaceshipAttribute>().Color;

            var playerLifeTextObj = playerScoreTextObj.GetChild(0);
            playerLifeTexts[i] = playerLifeTextObj.GetComponent<TMP_Text>();
            playerLifeTexts[i].color = controller.Players[i].GetComponent<SpaceshipAttribute>().Color;
        }
    }

    void Update()
    {
        for (int i = 0; i < MultiplayerScenePlayerNumber.Number; i++)
        {
            if (controller.Players[i] is null)
            {
                playerLifeTexts[i].text = "0";
                continue;
            }

            playerNameTexts[i].text = $"{controller.Players[i].GetComponent<SpaceshipAttribute>().PlayerName}";
            playerScoreTexts[i].text = $"{controller.Players[i].GetComponent<SpaceshipAttribute>().Score}";
            playerLifeTexts[i].text = $"{controller.Players[i].GetComponent<SpaceshipAttribute>().Lives}";

            playerNameTexts[i].rectTransform.sizeDelta = playerNameTexts[i].GetPreferredValues();
            playerScoreTexts[i].rectTransform.sizeDelta = playerScoreTexts[i].GetPreferredValues();
            playerLifeTexts[i].rectTransform.sizeDelta = playerLifeTexts[i].GetPreferredValues();

            playerScoreTexts[i].rectTransform.position = new Vector3(playerNameTexts[i].rectTransform.sizeDelta.x + playerNameTexts[i].rectTransform.position.x + spaceBetweenWords, playerScoreTexts[i].rectTransform.position.y, 0);
            playerLifeTexts[i].rectTransform.position = new Vector3(playerScoreTexts[i].rectTransform.sizeDelta.x + playerScoreTexts[i].rectTransform.position.x + spaceBetweenWords, playerLifeTexts[i].rectTransform.position.y, 0);
        }

        LevelText.text = $"Level: {controller.CurrentLvl}";
        RemainingAsteroidsText.text = $"Asteroids remaining: {controller.AsteroidsToDestroy}";
    }
}
