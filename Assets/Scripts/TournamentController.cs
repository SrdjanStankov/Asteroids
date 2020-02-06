using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TournamentController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;
    public GameObject GameControllerObj;
    public Canvas MultiplayerCanvas;
    public Canvas WinningCanvas;

    private List<string> playerNames = new List<string>();
    private List<(string, string)> brackets = new List<(string, string)>();
    private List<string> bracketsWinners = new List<string>();
    private GameController gameController;
    private Canvas multiplayerCanvas;

    private bool done = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerNames.AddRange(MultiplayerScenePlayers.PlayerNames);
        SetupBrackets();
        SpawnBracket();
    }

    private void Update()
    {
        if (bracketsWinners.Count == 1 && brackets.Count <= 0 && !done)
        {
            // logika za kraj igre
            var btnObj = WinningCanvas.transform.GetChild(2);
            btnObj.GetChild(0).GetComponent<TMP_Text>().text = "Back to main menu";
            var btnOnClick = btnObj.GetComponent<Button>().onClick;
            btnOnClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            btnOnClick.RemoveAllListeners();
            btnOnClick.AddListener(() => EndGameEvent());
            done = true;
        }

        if (gameController is null)
        {
            return;
        }

        if (gameController == null)
        {
            gameController = null;
            return;
        }

        if (string.IsNullOrEmpty(gameController.Winner.Item1))
        {
            return;
        }

        var winnerName = gameController.Winner;
        bracketsWinners.Add(winnerName.Item1);
        DestroyImmediate(gameController);
        DestroyImmediate(multiplayerCanvas.gameObject);
        brackets.RemoveAt(0);
    }

    private void SetupBrackets()
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            brackets.Add((GetPlayer(), GetPlayer()));
        }
    }

    private string GetPlayer()
    {
        int index = Random.Range(0, playerNames.Count);
        string player = playerNames[index];
        playerNames.RemoveAt(index);
        return player;
    }

    public void SpawnBracket()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Asteroid"))
        {
            Destroy(item.gameObject);
        }

        if (bracketsWinners.Count >= 2)
        {
            brackets.Add((bracketsWinners[0], bracketsWinners[1]));
            bracketsWinners.Remove(bracketsWinners[0]);
            bracketsWinners.Remove(bracketsWinners[0]);
        }

        if (brackets.Count > 0)
        {
            MultiplayerScenePlayers.PlayerNumber = 2;
            if (gameController == null)
            {
                gameController = GameControllerObj.AddComponent<GameController>();
                gameController.AsteroidPrefab = AsteroidPrefab;
                gameController.PlayerPrefab = PlayerPrefab;
                gameController.WinningCanvas = WinningCanvas;
                WinningCanvas.gameObject.SetActive(false);
                multiplayerCanvas = Instantiate(MultiplayerCanvas);
                multiplayerCanvas.gameObject.SetActive(true);
                gameController.RegularCanvas = multiplayerCanvas;
                gameController.PlayerNames.Add(brackets[0].Item1);
                gameController.PlayerNames.Add(brackets[0].Item2);
            }
        }
    }

    private void EndGameEvent()
    {
        SceneManager.LoadScene(0);
    }
}
