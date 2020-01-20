using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int PlayerCount = 1;
    public GameObject PlayerPrefab;
    public GameObject AsteroidPrefab;

    public List<GameObject> Players;

    private int currentLvl = 0;
    private int asteroidsToDestroy = 0;
    private float currentAsteroidSpeed = 2.75f;

    public GameObject Winner { get; set; }

    void Awake()
    {
    }

    void Start()
    {
        Players = new List<GameObject>(PlayerCount);
        for (int i = 0; i < PlayerCount; i++)
        {
            var gameObject = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
            gameObject.transform.position += new Vector3(i, 0, 0);
            Players.Add(gameObject);
            var script = gameObject.GetComponent<SpaceshipScript>();
            script.Forward = InputSchema.PlayersInputCombinations[i][Actions.Forward];
            script.Backwards = InputSchema.PlayersInputCombinations[i][Actions.Backwards];
            script.Left = InputSchema.PlayersInputCombinations[i][Actions.Left];
            script.Right = InputSchema.PlayersInputCombinations[i][Actions.Right];
            script.Fire = InputSchema.PlayersInputCombinations[i][Actions.Fire];
        }
    }

    void StartLevel()
    {
        currentLvl++;
        asteroidsToDestroy = (2 * currentLvl) + 1;
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
            var go = Instantiate(AsteroidPrefab, GetSpwnPositionAsteroid(), Quaternion.identity);
            go.GetComponent<AsteroidScript>().Speed = currentAsteroidSpeed;
        }
    }

    private Vector3 GetSpwnPositionAsteroid()
    {
        var Side = UnityEngine.Random.Range(1, 5);
        switch (Side)
        {
            case 1:
                return new Vector3(-(Screen.width / 2) - 50, UnityEngine.Random.Range(-Screen.height, Screen.height) / 2, 0); // leva strana
            case 2:
                return new Vector3(UnityEngine.Random.Range(-Screen.width, Screen.width) / 2, -(Screen.height / 2) - 50,  0); // donja strana
            case 3:
                return new Vector3((Screen.width / 2) + 50, -UnityEngine.Random.Range(-Screen.height, Screen.height)/2, 0); // desna strana
            case 4:
                return new Vector3(-(UnityEngine.Random.Range(-Screen.width, Screen.width) / 2), (Screen.height / 2) + 50, 0); // gornja strana
            default:
                return Vector3.zero;
        }
    }

    void Update()
    {
        if (Players.Count > 0)
        {
            // TODO: Spawn Power up-s

            if (asteroidsToDestroy <= 0)
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
