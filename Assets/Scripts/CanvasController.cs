using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Canvas MainMenuCanvas;
    public Canvas MultiplayerCanvas;
    public Canvas TournamentCanvas;
    public Canvas TwoPlayerCanvas;
    public Canvas ThreePlayerCanvas;
    public Canvas FourPlayerCanvas;

    private void Start()
    {
        DisableAllCanvases();
        MainMenuCanvas.gameObject.SetActive(true);
    }

    public void SingleplayerBtnClick()
    {
        // TODO: load singleplayer scene
        MultiplayerScenePlayers.PlayerNumber = 1;
        SceneManager.LoadScene("SingleplayerScene");
    }

    public void MultiplayerBtnClick()
    {
        DisableAllCanvases();
        MultiplayerCanvas.gameObject.SetActive(true);
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
        MultiplayerScenePlayers.PlayerNumber = 2;
        DisableAllCanvases();
        TwoPlayerCanvas.gameObject.SetActive(true);
    }

    public void ThreeplayerBtnClick()
    {
        MultiplayerScenePlayers.PlayerNumber = 3;
        DisableAllCanvases();
        ThreePlayerCanvas.gameObject.SetActive(true);
    }

    public void FourPlayerBtnClick()
    {
        MultiplayerScenePlayers.PlayerNumber = 4;
        DisableAllCanvases();
        FourPlayerCanvas.gameObject.SetActive(true);
    }

    public void TournamentplayerBtnClick()
    {
        // TODO: load tournament scene
        DisableAllCanvases();
        TournamentCanvas.gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        DisableAllCanvases();
        MainMenuCanvas.gameObject.SetActive(true);
    }

    public void BackToMultiplayerCanvas()
    {
        DisableAllCanvases();
        MultiplayerCanvas.gameObject.SetActive(true);
    }

    public void StartButtonTournamentClick()
    {
        // TODO: collect player names and put them inside static class

        SceneManager.LoadScene("TournamentScene");
    }
    public void StartButton2PlayerClick()
    {
        if (IsInputFieldFromCanvasValid(TwoPlayerCanvas, 2))
        {
            SceneManager.LoadScene("multiplayerscene");
        }
    }

    public void StartButton3PlayerClick()
    {
        if (IsInputFieldFromCanvasValid(ThreePlayerCanvas, 3))
        {
            SceneManager.LoadScene("MultiplayerScene"); 
        }
    }

    public void StartButton4PlayerClick()
    {
        if (IsInputFieldFromCanvasValid(FourPlayerCanvas, 4))
        {
            SceneManager.LoadScene("MultiplayerScene");
        }
    }

    public void OnTournamentPlayerCountChanged(int optionId)
    {
        Debug.Log(optionId);
    }

    private bool IsInputFieldFromCanvasValid(Canvas canvas, int count)
    {
        var retVal = new List<TMP_InputField>(count);
        var valid = true;
        for (int i = 0; i < count; i++)
        {
            retVal.Add(canvas.transform.GetChild(i).GetChild(1).GetComponent<TMP_InputField>());
            MultiplayerScenePlayers.PlayerNames[i] = retVal[i].text;
        }

        foreach (var item in retVal)
        {
            if (string.IsNullOrEmpty(item.text) || string.IsNullOrWhiteSpace(item.text))
            {
                item.GetComponent<Image>().color = Color.red;
                valid = false;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
        }

        return valid;
    }

    private void DisableAllCanvases()
    {
        FourPlayerCanvas.gameObject.SetActive(false);
        MainMenuCanvas.gameObject.SetActive(false);
        MultiplayerCanvas.gameObject.SetActive(false);
        ThreePlayerCanvas.gameObject.SetActive(false);
        TournamentCanvas.gameObject.SetActive(false);
        TwoPlayerCanvas.gameObject.SetActive(false);
    }
}
