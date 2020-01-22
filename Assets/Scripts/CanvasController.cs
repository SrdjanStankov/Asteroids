using UnityEngine;
using UnityEngine.SceneManagement;

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
        MultiplayerScenePlayerNumber.Number = 1;
        SceneManager.LoadScene("SingleplayerScene");
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
        MultiplayerScenePlayerNumber.Number = 2;
        SceneManager.LoadScene("MultiplayerScene");
    }

    public void ThreeplayerBtnClick()
    {
        // TODO: load multiplayer scene with 3 players
        MultiplayerScenePlayerNumber.Number = 3;
        SceneManager.LoadScene("MultiplayerScene");
    }

    public void FourlayerBtnClick()
    {
        // TODO: load multiplayer scene with 4 players
        MultiplayerScenePlayerNumber.Number = 4;
        SceneManager.LoadScene("MultiplayerScene");
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
