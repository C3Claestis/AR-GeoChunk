using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("================== PANEL PROPERTY ==================")]
    [SerializeField] GameObject panelMain;
    [SerializeField] GameObject panelMenu;
    [SerializeField] GameObject panelScreening;
    [SerializeField] GameObject panelSave;
    [SerializeField] GameObject panelLibraryMarker;
    [SerializeField] GameObject panelManual;
    [SerializeField] GameObject panelShowMarker;
    [SerializeField] GameObject panelShowSaved;
    [SerializeField] Animator DisplayScreening;

    [Header("================== Display PROPERTY ==================")]
    [SerializeField] Image buttonDisplaySetting;
    [SerializeField] Image buttonDisplayDeskripsi;

    [Header("================== 3D PROPERTY ==================")]
    [SerializeField] Animator riverObj;
    [SerializeField] Animator beachObj;
    [SerializeField] Animator iceburgObj;

    [Header("================== SAVED PANEL PROPERTY ==================")]
    [SerializeField] Button beachSaveBtn;
    [SerializeField] Button riverSaveBtn;
    [SerializeField] Button iceSaveBtn;
    [SerializeField] Sprite riverSprite;
    [SerializeField] Sprite beachSprite;
    [SerializeField] Sprite iceSprite;

    [Header("================== DESKRIPSI SCREENING PROPERTY ==================")]
    [SerializeField] Text tittledeskripsiTxt;
    [SerializeField] Text deskripsiTxt;
    [SerializeField][TextArea(1, 10)] string riverDescID;
    [SerializeField][TextArea(1, 10)] string riverDescEN;

    [SerializeField][TextArea(1, 10)] string beachDescID;
    [SerializeField][TextArea(1, 10)] string beachDescEN;

    [SerializeField][TextArea(1, 10)] string iceDescID;
    [SerializeField][TextArea(1, 10)] string iceDescEN;

    [Header("================== GLOBAL PROPERTY ==================")]
    [SerializeField] GameObject cameraAr;
    [SerializeField] GameObject buttonHoverScreen;
    [SerializeField] UISwitcher.UISwitcher animSwitch;
    [SerializeField] UISwitcher.UISwitcher rotateSwitch;
    [SerializeField] private float rotationSpeed = 30f;

    // Dictionary untuk mapping nama panel ke GameObject
    private Dictionary<string, GameObject> panelDictionary;
    private string OpenDeskripsi;
    private bool displayScreen = false;
    private bool isAnimating = false;
    private bool isSwitchingPanel = false;
    private Coroutine rotationCoroutine;

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
            { "Manual", panelManual },
            { "ShowMarker", panelShowMarker },
            { "ShowSaved", panelShowSaved },
        };
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            PlayerPrefs.DeleteAll();
        }
    }
    #region Switch Display Screen
    public void SetSwitchAnim()
    {
        riverObj.SetBool("PlayAnim", animSwitch.isOn);
        beachObj.SetBool("PlayAnim", animSwitch.isOn);
        iceburgObj.SetBool("PlayAnim", animSwitch.isOn);
    }

    public void SetSwitchRotation()
    {
        if (rotateSwitch.isOn)
        {
            StartRotation();
        }
        else
        {
            StopRotation();
        }
    }
    #endregion

    #region Saving
    public void SetSave(string nameData)
    {
        PlayerPrefs.SetInt(nameData, 1);
    }

    public void OpenSavePanel()
    {
        if (PlayerPrefs.GetInt("River") == 1)
        {
            riverSaveBtn.gameObject.name = "River";
            riverSaveBtn.GetComponent<Transform>().GetChild(0).GetComponent<Image>().sprite = riverSprite;
        }

        if (PlayerPrefs.GetInt("Beach") == 1)
        {
            beachSaveBtn.gameObject.name = "Beach";
            beachSaveBtn.GetComponent<Transform>().GetChild(0).GetComponent<Image>().sprite = beachSprite;
        }

        if (PlayerPrefs.GetInt("Iceberg") == 1)
        {
            iceSaveBtn.gameObject.name = "Ice";
            iceSaveBtn.GetComponent<Transform>().GetChild(0).GetComponent<Image>().sprite = iceSprite;
        }
    }
    #endregion

    #region ScreenDisplay
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

    public void ButtonDisplayScreenDisplaySetting()
    {
        buttonDisplayDeskripsi.type = Image.Type.Sliced;
        buttonDisplaySetting.type = Image.Type.Simple;
    }

    public void ButtonDisplayScreenDisplayDeskripsi()
    {
        buttonDisplayDeskripsi.type = Image.Type.Simple;
        buttonDisplaySetting.type = Image.Type.Sliced;
    }

    //Overloading ke EventButton
    public void SetDeskripsiTxt(string name)
    {
        OpenDeskripsi = name;

        if (OpenDeskripsi == "River")
        {
            tittledeskripsiTxt.text = "River";
            deskripsiTxt.text = riverDescID;
        }
        else if (OpenDeskripsi == "Beach")
        {
            tittledeskripsiTxt.text = "Beach";
            deskripsiTxt.text = beachDescID;
        }
        else if (OpenDeskripsi == "Iceberg")
        {
            tittledeskripsiTxt.text = "IceBerg";
            deskripsiTxt.text = iceDescID;
        }
    }
    #endregion

    #region Rotate 3D
    // Coroutine untuk merotasi objek
    private IEnumerator RotateContinuously()
    {
        while (true)
        {
            riverObj.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            beachObj.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            iceburgObj.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            yield return null; // Tunggu frame berikutnya
        }
    }

    // Mulai rotasi
    private void StartRotation()
    {
        if (rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(RotateContinuously());
        }
    }

    // Berhenti rotasi
    private void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
    }
    #endregion

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
