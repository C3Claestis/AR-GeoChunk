using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LibraryPanelManager : MonoBehaviour
{
    [SerializeField] Image markerUI;
    [SerializeField] Text nameTxt;
    [SerializeField] GameObject downloadbtn;

    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite riverSprite;
    [SerializeField] Sprite beachSprite;
    [SerializeField] Sprite iceSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenItem(string name)
    {
        if (name == "River")
        {
            markerUI.sprite = riverSprite;
            nameTxt.text = "River";
            downloadbtn.SetActive(true);
        }
        else if (name == "Beach")
        {
            markerUI.sprite = beachSprite;
            nameTxt.text = "Beach";
            downloadbtn.SetActive(true);
        }
        else if (name == "Ice")
        {
            markerUI.sprite = iceSprite;
            nameTxt.text = "Ice Berg";
            downloadbtn.SetActive(true);
        }
        else
        {
            markerUI.sprite = defaultSprite;
            nameTxt.text = "???";
            downloadbtn.SetActive(false);
        }
    }
}
