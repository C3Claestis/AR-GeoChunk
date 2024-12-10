using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class LibraryPanelManager : MonoBehaviour
{
    [SerializeField] Image markerUI;
    [SerializeField] Text nameTxt;
    [SerializeField] GameObject downloadbtnRiver;
    [SerializeField] GameObject downloadbtnBeach;
    [SerializeField] GameObject downloadbtnIce;

    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite riverSprite;
    [SerializeField] Sprite beachSprite;
    [SerializeField] Sprite iceSprite;

    [SerializeField] GameObject downloadComplete;
    [SerializeField] Text coment;

    public void OpenItem(string name)
    {
        if (name == "River")
        {
            markerUI.sprite = riverSprite;
            nameTxt.text = "River";
            downloadbtnRiver.SetActive(true);
            downloadbtnBeach.SetActive(false);
            downloadbtnIce.SetActive(false);
        }
        else if (name == "Beach")
        {
            markerUI.sprite = beachSprite;
            nameTxt.text = "Beach";
            downloadbtnRiver.SetActive(false);
            downloadbtnBeach.SetActive(true);
            downloadbtnIce.SetActive(false);
        }
        else if (name == "Ice")
        {
            markerUI.sprite = iceSprite;
            nameTxt.text = "Ice Berg";
            downloadbtnRiver.SetActive(false);
            downloadbtnBeach.SetActive(false);
            downloadbtnIce.SetActive(true);
        }
        else
        {
            markerUI.sprite = defaultSprite;
            nameTxt.text = "???";
            downloadbtnRiver.SetActive(false);
            downloadbtnBeach.SetActive(false);
            downloadbtnIce.SetActive(false);
        }
    }

    void RequestPermissions()
    {
        NativeGallery.Permission permissionResult = NativeGallery.RequestPermission(
            NativeGallery.PermissionType.Write,
            NativeGallery.MediaType.Image
        );

        if (permissionResult != NativeGallery.Permission.Granted)
        {
            coment.text = "Permission not granted to access storage!";
        }
    }

    public void SaveImageToGallery(string fileName)
    {
        // Memastikan izin diberikan
        RequestPermissions();

        // Path ke file di StreamingAssets
        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (sourcePath.Contains("://"))
        {
            // File ada di dalam APK, gunakan UnityWebRequest untuk mengunduh file
            StartCoroutine(DownloadAndSaveImage(sourcePath, fileName));
        }
        else
        {
            // File dapat diakses langsung
            SaveImageDirectly(sourcePath, fileName);
        }
    }

    private IEnumerator DownloadAndSaveImage(string sourcePath, string fileName)
    {
        string tempPath = Path.Combine(Application.temporaryCachePath, fileName);

        using (UnityWebRequest uwr = UnityWebRequest.Get(sourcePath))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                File.WriteAllBytes(tempPath, uwr.downloadHandler.data);
                SaveToGallery(tempPath, fileName);
            }
            else
            {
                Debug.LogError($"Failed to download file: {uwr.error}");
                coment.text = "Failed to download file.";
            }
        }
    }

    private void SaveImageDirectly(string sourcePath, string fileName)
    {
        string tempPath = Path.Combine(Application.temporaryCachePath, fileName);
        File.Copy(sourcePath, tempPath, true);
        SaveToGallery(tempPath, fileName);
    }

    private void SaveToGallery(string tempPath, string fileName)
    {
        NativeGallery.SaveImageToGallery(tempPath, "MyGallery", $"Saved_{fileName}", (success, path) =>
        {
            if (success)
            {
                downloadComplete.SetActive(true);
                Debug.Log($"Image saved to Gallery: {path}");
            }
            else
            {
                coment.text = "Failed to save image to Gallery.";
                Debug.LogError("Failed to save image to Gallery.");
            }
        });
    }
}
