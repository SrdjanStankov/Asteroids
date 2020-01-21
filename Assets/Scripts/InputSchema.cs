using System.Collections.Generic;
using UnityEngine;

public static class InputSchema
{
    public static Dictionary<int, Dictionary<Actions, KeyCode>> PlayersInputCombinations { get; private set; }

    static InputSchema()
    {
        PlayersInputCombinations = new Dictionary<int, Dictionary<Actions, KeyCode>>
        {
            { 0, new Dictionary<Actions, KeyCode>()
            {
                { Actions.Forward, KeyCode.UpArrow },
                { Actions.Backwards, KeyCode.DownArrow},
                { Actions.Left, KeyCode.LeftArrow },
                { Actions.Right, KeyCode.RightArrow },
                { Actions.Fire, KeyCode.RightControl }
            } } ,
            { 1, new Dictionary<Actions, KeyCode>()
            {
                { Actions.Forward, KeyCode.W },
                { Actions.Backwards, KeyCode.S},
                { Actions.Left, KeyCode.A },
                { Actions.Right, KeyCode.D },
                { Actions.Fire, KeyCode.LeftControl }
            } } ,
            { 2, new Dictionary<Actions, KeyCode>()
            {
                { Actions.Forward, KeyCode.I },
                { Actions.Backwards, KeyCode.K},
                { Actions.Left, KeyCode.J },
                { Actions.Right, KeyCode.L },
                { Actions.Fire, KeyCode.Comma }
            } } ,
            { 3, new Dictionary < Actions, KeyCode >()
            {
                { Actions.Forward, KeyCode.Keypad8 },
                { Actions.Backwards, KeyCode.Keypad5 },
                { Actions.Left, KeyCode.Keypad4 },
                { Actions.Right, KeyCode.Keypad6 },
                { Actions.Fire, KeyCode.KeypadPeriod }
            } }
        };
    }
}

public enum Actions
{
    Forward, Backwards, Left, Right, Fire
}

public enum AsteroidType
{
    Large, Medium, Small
}