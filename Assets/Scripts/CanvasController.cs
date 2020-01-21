using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Canvas MainMenuCanvas;
    public Canvas MultiplayerCanvas;
    public Canvas TournamentCanvas;

    private void Start()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        MultiplayerCanvas.gameObject.SetActive(false);
        TournamentCanvas.gameObject.SetActive(false);
    }

    public void SingleplayerBtnClick()
    {
        // TODO: load singleplayer scene
    }

    public void MultiplayerBtnClick()
    {
        MultiplayerCanvas.gameObject.SetActive(true);
        MainMenuCanvas.gameObject.SetActive(false);
        TournamentCanvas.gameObject.SetActive(false);
    }

    public void ExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void TwoplayerBtnClick()
    {
        // TODO: load multiplayer scene with 2 players
    }

    public void ThreeplayerBtnClick()
    {
        // TODO: load multiplayer scene with 3 players
    }

    public void FourlayerBtnClick()
    {
        // TODO: load multiplayer scene with 4 players
    }

    public void TournamentplayerBtnClick()
    {
        // TODO: load tournament scene
        TournamentCanvas.gameObject.SetActive(true);
        MainMenuCanvas.gameObject.SetActive(false);
        MultiplayerCanvas.gameObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        MainMenuCanvas.gameObject.SetActive(true);
        MultiplayerCanvas.gameObject.SetActive(false);
        TournamentCanvas.gameObject.SetActive(false);
    }

    public void BackToMultiplayerCanvas()
    {
        MultiplayerCanvas.gameObject.SetActive(true);
        MainMenuCanvas.gameObject.SetActive(false);
        TournamentCanvas.gameObject.SetActive(false);
    }

    public void OnTournamentPlayerCountChanged(int optionId)
    {
        Debug.Log(optionId);
    }
}
