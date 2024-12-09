using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SavePanelManager : MonoBehaviour
{
    [SerializeField] Image markerUI;
    [SerializeField] Text nameTxt;

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

    public void OpenItem(GameObject obj)
    {
        if (obj.name == "River")
        {
            markerUI.sprite = riverSprite;
            nameTxt.text = "River";
        }
        else if (obj.name == "Beach")
        {
            markerUI.sprite = beachSprite;
            nameTxt.text = "Beach";
        }
        else if (obj.name == "Ice")
        {
            markerUI.sprite = iceSprite;
            nameTxt.text = "Ice Berg";
        }
        else
        {
            markerUI.sprite = defaultSprite;
            nameTxt.text = "???";
        }
    }
}
