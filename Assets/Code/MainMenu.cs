using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject afterDownloadMarker;
    [SerializeField] GameObject marker;
    void Start()
    {
        if (PlayerPrefs.GetInt("Download") == 1)
        {
            afterDownloadMarker.SetActive(true);
            marker.SetActive(false);
        }
    }
    public void OnInGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SentChange()
    {
        PlayerPrefs.SetInt("Download", 1);
        marker.SetActive(false);
        afterDownloadMarker.SetActive(true);
    }
}
