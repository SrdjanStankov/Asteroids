using System.Collections.Generic;
using UnityEngine;

public class TournamentController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;
    public GameObject GameControllerObj;
    public GameObject MultiplayerCanvas;
    [SerializeField] private List<string> playerNames = new List<string>();

    private List<(string, string)> brackets = new List<(string, string)>();
    private List<string> bracketsWinners = new List<string>();
    private GameController gameController;
    private GameObject multiplayerCanvasObj;

    // Start is called before the first frame update
    private void Start()
    {
        SetupBrackets();
        SpawnBracket();
    }

    // Update is called once per frame
    private void Update()
    {
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
        print($"bracket winner is {winnerName}");
        bracketsWinners.Add(winnerName.Item1);
        DestroyImmediate(gameController);
        DestroyImmediate(multiplayerCanvasObj);
        brackets.RemoveAt(0);

        foreach (var item in GameObject.FindGameObjectsWithTag("Asteroid"))
        {
            Destroy(item.gameObject);
        }

        SpawnBracket();
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
        var index = Random.Range(0, playerNames.Count);
        var player = playerNames[index];
        playerNames.RemoveAt(index);
        return player;
    }

    private void SpawnBracket()
    {
        if (bracketsWinners.Count >= 2)
        {
            brackets.Add((bracketsWinners[0], bracketsWinners[1]));
            bracketsWinners.Remove(bracketsWinners[0]);
            bracketsWinners.Remove(bracketsWinners[0]);
        }

        if (brackets.Count > 0)
        {
            // spawn game controller sa 2 igraca na brackets[0] i brackets[1]
            MultiplayerScenePlayerNumber.Number = 2;
            if (gameController == null)
            {
                gameController = GameControllerObj.AddComponent<GameController>();
                gameController.AsteroidPrefab = AsteroidPrefab;
                gameController.PlayerPrefab = PlayerPrefab;
                multiplayerCanvasObj = Instantiate(MultiplayerCanvas);
                multiplayerCanvasObj.SetActive(true);
            }
        }

        if (bracketsWinners.Count == 1 && brackets.Count <=0)
        {
            // logika za kraj igre
            print($"Winner {bracketsWinners[0]}");
        }
    }
}
