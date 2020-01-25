using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;

    [SerializeField] private float currentAsteroidSpeed = 2.5f;
    [SerializeField] private float spawnPowerUpInterval = 30;
    [SerializeField] private List<GameObject> powerUps = new List<GameObject>();

    private int currentLvl = 0;

    private Camera cameraMain;

    private float cameraZOffset;
    private float nextPowerUpTime = 0;

    public GameObject Winner { get; set; }
    public GameObject[] Players { get; set; }
    public float SpawnPowerUpInterval { get { return spawnPowerUpInterval; } set { spawnPowerUpInterval = value; } }
    public int AsteroidsToDestroy { get; set; } = 0;
    public int CurrentLvl { get => currentLvl; private set => currentLvl = value; }

    private void Start()
    {
        if (MultiplayerScenePlayerNumber.Number == 0)
        {
            MultiplayerScenePlayerNumber.Number = 1;
        }
        cameraMain = Camera.main;
        cameraZOffset = cameraMain.transform.position.z + 10;
        Players = new GameObject[MultiplayerScenePlayerNumber.Number];
        for (int i = 0; i < MultiplayerScenePlayerNumber.Number; i++)
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

        switch (i)
        {
            case 0:
                spaceshipGO.GetComponent<SpaceshipAttribute>().Color = Color.blue;
                break;
            case 1:
                spaceshipGO.GetComponent<SpaceshipAttribute>().Color = Color.green;
                break;
            case 2:
                spaceshipGO.GetComponent<SpaceshipAttribute>().Color = Color.red;
                break;
            case 3:
                spaceshipGO.GetComponent<SpaceshipAttribute>().Color = Color.magenta;
                break;
            default:
                break;
        }
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
        for (int i = 0; i < MultiplayerScenePlayerNumber.Number; i++)
        {
            if (Players[i] == gameObject)
            {
                Players[i] = null;
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
        // choose powerup
        var powerUp = powerUps[Random.Range(0, powerUps.Count)];
        // choose location on screen
        var spawnLocation = cameraMain.ScreenToWorldPoint(new Vector3(Random.Range(25, Screen.width - 25), Random.Range(25, Screen.height - 25), cameraZOffset));
        // spawn powerup
        Instantiate(powerUp, spawnLocation, Quaternion.identity);
    }

    private void Update()
    {
        if (Players.Length > 0)
        {
            // TODO: Spawn Power up-s

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



        /*
         
        if len(mng.Managers.getInstance().objects.FindObjectsOfType("Spaceship")) > 0:
            self.nextPowerUp = (self.nextPowerUp + 4) % 40
            if(self.nextPowerUp == 0):
                self.spawnPowerUp()
            if self.asteroidsToDestroy <= 0:
                self.startLevel()
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
}
