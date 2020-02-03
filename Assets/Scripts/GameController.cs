using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;
    public Canvas WinningCanvas;
    public Canvas RegularCanvas;

    [SerializeField] private float currentAsteroidSpeed = 2.5f;
    [SerializeField] private float spawnPowerUpInterval = 30;
    [SerializeField] private List<GameObject> powerUps = new List<GameObject>();

    private int currentLvl = 0;

    private Camera cameraMain;
    private List<(string, float)> destroyedShips = new List<(string, float)>();
    private (string, float) winner;

    private float cameraZOffset;
    private float nextPowerUpTime = 0;
    private bool end = false;

    public GameObject[] Players { get; set; }
    public float SpawnPowerUpInterval { get => spawnPowerUpInterval; set => spawnPowerUpInterval = value; }
    public int AsteroidsToDestroy { get; set; } = 0;
    public int CurrentLvl { get => currentLvl; private set => currentLvl = value; }
    public (string, float) Winner { get => winner; set => winner = value; }
    public List<string> PlayerNames { get; set; } = new List<string>();

    private void Start()
    {
        end = false;
        if (PlayerNames.Count <= 0)
        {
            MultiplayerScenePlayers.PlayerNames.ForEach((item) => PlayerNames.Add(item));
        }
        WinningCanvas.gameObject.SetActive(false);
        RegularCanvas.gameObject.SetActive(true);
        if (MultiplayerScenePlayers.PlayerNumber == 0)
        {
            MultiplayerScenePlayers.PlayerNumber = 1;
        }
        cameraMain = Camera.main;
        cameraZOffset = cameraMain.transform.position.z + 10;
        Players = new GameObject[MultiplayerScenePlayers.PlayerNumber];
        for (int i = 0; i < MultiplayerScenePlayers.PlayerNumber; i++)
        {
            StupPlayer(i);
        }
    }

    private void StupPlayer(int i)
    {
        nextPowerUpTime = Time.time + SpawnPowerUpInterval;
        var spaceshipGO = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        spaceshipGO.transform.position += new Vector3(i, 0, cameraZOffset);
        Players[i] = spaceshipGO;

        var script = spaceshipGO.GetComponent<SpaceshipMovement>();
        script.Forward = InputSchema.PlayersInputCombinations[i][Actions.Forward];
        script.Backwards = InputSchema.PlayersInputCombinations[i][Actions.Backwards];
        script.Left = InputSchema.PlayersInputCombinations[i][Actions.Left];
        script.Right = InputSchema.PlayersInputCombinations[i][Actions.Right];
        spaceshipGO.GetComponent<SpaceshipFiring>().Fire = InputSchema.PlayersInputCombinations[i][Actions.Fire];

        var Attribute = spaceshipGO.GetComponent<SpaceshipAttribute>();
        switch (i)
        {
            case 0:
                Attribute.Color = Color.blue;
                break;
            case 1:
                Attribute.Color = Color.green;
                break;
            case 2:
                Attribute.Color = Color.red;
                break;
            case 3:
                Attribute.Color = Color.magenta;
                break;
            default:
                break;
        }

        Attribute.PlayerName = PlayerNames[i];
    }

    private void StartLevel()
    {
        CurrentLvl++;
        int maxAsteroidsToDestroy = (2 * CurrentLvl) + 1;
        currentAsteroidSpeed += 0.2f;

        nextPowerUpTime = Time.time + SpawnPowerUpInterval;

        if (CurrentLvl % 4 == 0)
        {
            foreach (var item in Players)
            {
                var spaceshipMovement = item.GetComponent<SpaceshipAttribute>();
                spaceshipMovement.FlightSpeed += 0.2f;
                spaceshipMovement.RotationSpeed += 20f;
                item.GetComponent<SpaceshipAttribute>().AddPoints(1000);
            }
        }

        for (int i = 0; i < maxAsteroidsToDestroy; i++)
        {
            CreateAsteroid(AsteroidType.Large, GetSpwnPositionAsteroid());
        }
    }

    internal void CreateAsteroid(AsteroidType type, Vector3 position)
    {
        var asteroidObj = Instantiate(AsteroidPrefab, position, Quaternion.identity);
        var asteroid = asteroidObj.GetComponent<StraightLineMovement>();
        asteroid.Angle = Random.Range(0, 360);
        asteroid.Speed = currentAsteroidSpeed;
        asteroidObj.GetComponent<AsteroidsStats>().Type = type;
        switch (type)
        {
            case AsteroidType.Large:
                asteroid.Speed = currentAsteroidSpeed;
                break;
            case AsteroidType.Medium:
                asteroid.Speed *= 1.5f;
                asteroidObj.transform.localScale /= 2;
                break;
            case AsteroidType.Small:
                asteroid.Speed *= 2.25f;
                asteroidObj.transform.localScale /= 4;
                break;
            default:
                break;
        }
        AsteroidsToDestroy++;
    }

    internal void RemovePlayer(GameObject gameObject)
    {
        for (int i = 0; i < MultiplayerScenePlayers.PlayerNumber; i++)
        {
            if (Players[i] == gameObject)
            {
                Players[i] = null;
                var attribute = gameObject.GetComponent<SpaceshipAttribute>();
                destroyedShips.Add((attribute.PlayerName, attribute.Score));
            }
        }
        Destroy(gameObject);
    }

    private Vector3 GetSpwnPositionAsteroid()
    {
        int Side = Random.Range(1, 5);
        switch (Side)
        {
            case 1:
                return cameraMain.ScreenToWorldPoint(new Vector3(-100, Random.Range(0, Screen.height), cameraZOffset)); // leva strana
            case 2:
                return cameraMain.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), -100, cameraZOffset)); // donja strana
            case 3:
                return cameraMain.ScreenToWorldPoint(new Vector3(Screen.width + 100, Random.Range(0, Screen.height), cameraZOffset)); // desna strana
            case 4:
                return cameraMain.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height + 100, cameraZOffset)); // gornja strana
            default:
                return Vector3.zero;
        }
    }

    internal void DestroyAsteroid(AsteroidsStats asteroid)
    {
        AsteroidsToDestroy--;
        switch (asteroid.Type)
        {
            case AsteroidType.Large:
                CreateAsteroid(AsteroidType.Medium, asteroid.gameObject.transform.position);
                CreateAsteroid(AsteroidType.Medium, asteroid.gameObject.transform.position);
                Destroy(asteroid.gameObject);
                break;
            case AsteroidType.Medium:
                CreateAsteroid(AsteroidType.Small, asteroid.gameObject.transform.position);
                CreateAsteroid(AsteroidType.Small, asteroid.gameObject.transform.position);
                Destroy(asteroid.gameObject);
                break;
            case AsteroidType.Small:
                Destroy(asteroid.gameObject);
                break;
            default:
                break;
        }
    }

    private void SpawnPowerUp()
    {
        var powerUp = powerUps[Random.Range(0, powerUps.Count)];
        var spawnLocation = cameraMain.ScreenToWorldPoint(new Vector3(Random.Range(25, Screen.width - 25), Random.Range(25, Screen.height - 25), cameraZOffset));
        Instantiate(powerUp, spawnLocation, Quaternion.identity);
    }

    private void Update()
    {
        if (end)
        {
            return;
        }

        if (ArePlayersAlive())
        {
            if (nextPowerUpTime <= Time.time)
            {
                SpawnPowerUp();
                nextPowerUpTime += SpawnPowerUpInterval;
            }

            if (AsteroidsToDestroy <= 0)
            {
                StartLevel();
            }
        }
        else
        {
            if (string.IsNullOrEmpty(winner.Item1))
            {
                winner = destroyedShips.OrderByDescending(x => x.Item2).FirstOrDefault();
                //Debug.Log(winner);
                ShowWinningCanvas();
            }
        }
    }

    private void ShowWinningCanvas()
    {
        WinningCanvas.transform.GetChild(0).GetComponent<TMP_Text>().text = Winner.Item1;
        WinningCanvas.transform.GetChild(1).GetComponent<TMP_Text>().text = Winner.Item2.ToString();
        WinningCanvas.gameObject.SetActive(true);
        RegularCanvas.gameObject.SetActive(false);
        end = true;
    }

    private bool ArePlayersAlive()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i] != null)
            {
                return true;
            }
        }
        return false;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
