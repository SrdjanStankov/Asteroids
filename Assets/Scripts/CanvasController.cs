using System.Collections.Generic;
using System.Linq;
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

    public RectTransform TournamentPlayerNamesPanel;

    public GameObject NameInputFieldPrefab;

    private List<TMP_InputField> nameInputFields = new List<TMP_InputField>();

    private void Start()
    {
        DisableAllCanvases();
        MainMenuCanvas.gameObject.SetActive(true);
        MultiplayerScenePlayers.PlayerNames = new List<string>();
        MultiplayerScenePlayers.PlayerNumber = 0;
        OnTournamentPlayerCountChanged(0);
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
        bool inputFieldValid = ColorFieldsBasedOnCorrection();
        if (inputFieldValid)
        {
            foreach (var item in nameInputFields)
            {
                MultiplayerScenePlayers.PlayerNames.Add(item.text);
            }
            MultiplayerScenePlayers.PlayerNumber = nameInputFields.Count;
            SceneManager.LoadScene("TournamentScene");
        }
    }

    private bool ColorFieldsBasedOnCorrection()
    {
        bool inputFieldValid = true;
        foreach (var item in nameInputFields)
        {
            if (string.IsNullOrEmpty(item.text) || string.IsNullOrWhiteSpace(item.text))
            {
                item.GetComponent<Image>().color = Color.red;
                inputFieldValid = false;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
        }

        return inputFieldValid;
    }

    public void StartButton2PlayerClick()
    {
        if (IsInputFieldFromCanvasValid(TwoPlayerCanvas, 2))
        {
            SceneManager.LoadScene("MultiplayerScene");
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
        DestroyInputFields();
        for (int i = 0; i < 2 + (2 * optionId); i++)
        {
            nameInputFields.Add(Instantiate(NameInputFieldPrefab, TournamentPlayerNamesPanel).GetComponent<TMP_InputField>());
            nameInputFields.LastOrDefault().onDeselect.AddListener((_) => ColorFieldsBasedOnCorrection());
        }
    }

    private void DestroyInputFields()
    {
        foreach (var item in nameInputFields)
        {
            Destroy(item.gameObject);
        }
        nameInputFields.Clear();
    }

    private bool IsInputFieldFromCanvasValid(Canvas canvas, int count)
    {
        var inputFields = new List<TMP_InputField>(count);
        bool inputFieldValid = true;

        for (int i = 0; i < count; i++)
        {
            inputFields.Add(canvas.transform.GetChild(i).GetChild(1).GetComponent<TMP_InputField>());
        }

        foreach (var item in inputFields)
        {
            if (string.IsNullOrEmpty(item.text) || string.IsNullOrWhiteSpace(item.text))
            {
                item.GetComponent<Image>().color = Color.red;
                inputFieldValid = false;
            }
            else
            {
                item.GetComponent<Image>().color = Color.white;
            }
        }

        if (inputFieldValid)
        {
            foreach (var item in inputFields)
            {
                MultiplayerScenePlayers.PlayerNames.Add(item.text);
            }
        }

        return inputFieldValid;
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
