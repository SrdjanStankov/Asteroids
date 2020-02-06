using System.Collections.Generic;
using UnityEngine;

public static class MultiplayerScenePlayers
{
    public static int PlayerNumber { get; set; }
    public static List<string> PlayerNames { get; set; } = new List<string>();

    private static List<int> randomNumbers = new List<int>();

    public static int GetRandomNonRepeatingNumberBasedOnPlayerCount()
    {
        if (randomNumbers.Count == 0)
        {
            CreateRandomNumbers();
        }
        int retVal = randomNumbers[0];
        randomNumbers.RemoveAt(0);
        return retVal;
    }

    private static void CreateRandomNumbers()
    {
        while (PlayerNumber > randomNumbers.Count)
        {
            int rand;
            do
            {
                rand = Random.Range(0, PlayerNumber);
            } while (randomNumbers.Contains(rand));
            randomNumbers.Add(rand);
        }
    }
}
