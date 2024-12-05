using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("================== PANEL PROPERTY ==================")]
    [SerializeField] GameObject panelMain;
    [SerializeField] GameObject panelMenu;
    [SerializeField] GameObject panelScreening;
    [SerializeField] GameObject panelSave;
    [SerializeField] GameObject panelLibraryMarker;
    [SerializeField] GameObject panelManual;

    [Header("================== PANEL PROPERTY ==================")]
    [SerializeField] Animator DisplayScreening;

    [Header("================== GLOBAL PROPERTY ==================")]
    [SerializeField] GameObject cameraAr;
    [SerializeField] GameObject buttonHoverScreen;

    // Dictionary untuk mapping nama panel ke GameObject
    private Dictionary<string, GameObject> panelDictionary;
    private bool displayScreen = false;
    private bool isAnimating = false;
    private bool isSwitchingPanel = false; // Flag untuk mencegah pergantian panel selama jeda waktu

    // Start is called before the first frame update
    void Start()
    {
        // Inisialisasi dictionary
        panelDictionary = new Dictionary<string, GameObject>
        {
            { "Main", panelMain },
            { "Menu", panelMenu },
            { "Screening", panelScreening },
            { "Save", panelSave },
            { "LibraryMarker", panelLibraryMarker },
            { "Manual", panelManual }
        };
    }

    // Fungsi dipanggil melalui tombol
    public void OnButton(string panelName)
    {
        if (isSwitchingPanel) return; // Abaikan jika sedang dalam proses pergantian panel

        Debug.Log($"Button {panelName} ditekan");
        if (panelDictionary.ContainsKey(panelName))
        {
            StartCoroutine(SwitchPanel(panelDictionary[panelName], panelName == "Main"));
        }
        else
        {
            Debug.LogWarning($"Panel dengan nama {panelName} tidak ditemukan!");
        }
    }

    public void OnToogleScreen()
    {
        if (isAnimating) return; // Abaikan jika animasi sedang berlangsung

        if (displayScreen)
        {
            StartCoroutine(PlayAnimation("PopUp"));
            displayScreen = false;
            buttonHoverScreen.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            StartCoroutine(PlayAnimation("PopDown"));
            displayScreen = true;
            buttonHoverScreen.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    IEnumerator PlayAnimation(string triggerName)
    {
        isAnimating = true; // Set flag untuk mencegah input baru
        DisplayScreening.SetTrigger(triggerName);

        // Tunggu hingga animasi selesai
        AnimatorClipInfo[] clipInfo = DisplayScreening.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float animationDuration = clipInfo[0].clip.length;
            yield return new WaitForSeconds(animationDuration);
        }

        isAnimating = false; // Animasi selesai, reset flag
    }

    /// <summary>
    /// Fungsi untuk mengatur panel mana yang aktif setelah jeda waktu.
    /// </summary>
    IEnumerator SwitchPanel(GameObject activePanel, bool enableCamera)
    {
        // Pengecualian: Jika berpindah dari Main ke Menu, tidak ada jeda waktu
        if (panelMain.activeSelf && activePanel == panelMenu)
        {
            Debug.Log("Berpindah dari Main ke Menu tanpa jeda");
            foreach (var panel in panelDictionary.Values)
            {
                panel.SetActive(panel == activePanel);
            }
            cameraAr.SetActive(enableCamera);
            yield break;
        }

        isSwitchingPanel = true; // Set flag untuk mencegah pergantian panel lainnya
        yield return new WaitForSeconds(1f); // Jeda waktu sebelum panel diaktifkan

        foreach (var panel in panelDictionary.Values)
        {
            panel.SetActive(panel == activePanel);
        }

        cameraAr.SetActive(enableCamera); // Atur status kamera
        isSwitchingPanel = false; // Reset flag setelah pergantian selesai
    }
}
