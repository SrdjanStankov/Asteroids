using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;

    [SerializeField] private float currentAsteroidSpeed = 2.5f;
    [SerializeField] private float spawnPowerUpInterval = 30;
    [SerializeField] private List<GameObject> powerUps = new List<GameObject>();

    private int currentLvl = 0;

    private Camera cameraMain;
    private List<(string, float)> destroyedShips = new List<(string, float)>();
    private (string, float) winner;

    private float cameraZOffset;
    private float nextPowerUpTime = 0;

    public GameObject[] Players { get; set; }
    public float SpawnPowerUpInterval { get => spawnPowerUpInterval; set => spawnPowerUpInterval = value; }
    public int AsteroidsToDestroy { get; set; } = 0;
    public int CurrentLvl { get => currentLvl; private set => currentLvl = value; }
    public (string, float) Winner { get => winner; set => winner = value; }

    private void Start()
    {
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

        Attribute.PlayerName = MultiplayerScenePlayers.PlayerNames[i];
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
                Debug.Log(winner);
                // show winning screen with winner and score
            }
        }



        /*
         
        else:
            if self.winner is None:
                self.winner = max(self.scores, key = lambda x: x[2])
                print(f"Winner is {self.winner[0]}: {self.winner[2]}")
            if not self.isTournament:
                if not self.isOver: 
                    for item in self.destroyedShipAttribute:
                        mng.Managers.getInstance().scene.removeItem(item)
                    for item in mng.Managers.getInstance().objects.FindObjectsOfType("Asteroid"):
                        mng.Managers.getInstance().objects.Destroy(item.Id)
                    self.isOver = True
                else:
                    self.noti.update.disconnect(self.update)
                    mng.Managers.getInstance().scene.backFromMultiplayer()
                    mng.Managers.getInstance().input.stopListening()
         
         */
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
}
