using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Range(1, 4)] public int PlayerCount = 1;
    
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;
    
    private Camera cameraMain;
    
    private int currentLvl = 0;
    private int asteroidsToDestroy = 0;
    private float currentAsteroidSpeed = 2.75f;
    private float cameraZOffset;

    public GameObject Winner { get; set; }
    public int AsteroidsToDestroy { get => asteroidsToDestroy; set => asteroidsToDestroy = value; }
    public List<GameObject> Players { get; set; }

    private void Start()
    {
        cameraMain = Camera.main;
        cameraZOffset = cameraMain.transform.position.z + 10;
        Players = new List<GameObject>(PlayerCount);
        for (int i = 0; i < PlayerCount; i++)
        {
            StupPlayer(i);
        }
    }

    private void StupPlayer(int i)
    {
        var gameObject = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        gameObject.transform.position += new Vector3(i, 0, cameraZOffset);
        Players.Add(gameObject);
        var script = gameObject.GetComponent<SpaceshipScript>();
        script.Forward = InputSchema.PlayersInputCombinations[i][Actions.Forward];
        script.Backwards = InputSchema.PlayersInputCombinations[i][Actions.Backwards];
        script.Left = InputSchema.PlayersInputCombinations[i][Actions.Left];
        script.Right = InputSchema.PlayersInputCombinations[i][Actions.Right];
        script.Fire = InputSchema.PlayersInputCombinations[i][Actions.Fire];
        switch (i)
        {
            case 0:
                script.Sprite = Resources.Load<Sprite>("Spaceship Blue");
                break;
            case 1:
                script.Sprite = Resources.Load<Sprite>("Spaceship Green");
                break;
            case 2:
                script.Sprite = Resources.Load<Sprite>("Spaceship Red");
                break;
            case 3:
                script.Sprite = Resources.Load<Sprite>("Spaceship Purple");
                break;
            default:
                break;
        }
    }

    private void StartLevel()
    {
        currentLvl++;
        AsteroidsToDestroy = (2 * currentLvl) + 1;
        currentAsteroidSpeed += 0.2f;

        if (currentLvl % 4 == 0)
        {
            foreach (var item in Players)
            {
                var spaceship = item.GetComponent<SpaceshipScript>();
                spaceship.FlightSpeed += 0.2f;
                spaceship.Points += 1000;
            }
        }

        for (int i = 0; i < 2 + (2 * currentLvl); i++)
        {
            CreateAsteroid(AsteroidType.Large, GetSpwnPositionAsteroid());
        }
    }

    internal void DestroyAsteroid(GameObject gameObject)
    {
        AsteroidsToDestroy--;
        Destroy(gameObject);
    }

    internal void CreateAsteroid(AsteroidType type, Vector3 position)
    {
        var asteroidObj = Instantiate(AsteroidPrefab, position, Quaternion.identity);
        var asteroid = asteroidObj.GetComponent<AsteroidScript>();
        asteroid.Angle = UnityEngine.Random.Range(0, 360);
        asteroid.Speed = currentAsteroidSpeed;
        asteroid.Type = type;
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
        asteroidsToDestroy++;
    }

    internal void RemovePlayer(GameObject gameObject)
    {
        Players.Remove(gameObject);
        Destroy(gameObject);
    }

    private Vector3 GetSpwnPositionAsteroid()
    {
        int Side = UnityEngine.Random.Range(1, 5);
        switch (Side)
        {
            case 1:
                return cameraMain.ScreenToWorldPoint(new Vector3(-100, UnityEngine.Random.Range(0, Screen.height), cameraZOffset)); // leva strana
            case 2:
                return cameraMain.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), -100, cameraZOffset)); // donja strana
            case 3:
                return cameraMain.ScreenToWorldPoint(new Vector3(Screen.width + 100, UnityEngine.Random.Range(0, Screen.height), cameraZOffset)); // desna strana
            case 4:
                return cameraMain.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), Screen.height + 100, cameraZOffset)); // gornja strana
            default:
                return Vector3.zero;
        }
    }

    private void Update()
    {
        if (Players.Count > 0)
        {
            // TODO: Spawn Power up-s

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
