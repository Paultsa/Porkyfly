using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class StartMenu : MonoBehaviour
{

    bool doneOnce = false;
    bool inOptions = false;
    [SerializeField] bool soundsEnabled;
    [SerializeField] bool musicEnabled;

    public RectTransform optionsFiller;

    public Sprite optionsUnselected;
    public Sprite optionsSelected;

    public Sprite soundEnabledSprite;
    public Sprite soundDisabledSprite;

    public Sprite musicEnabledSprite;
    public Sprite musicDisabledSprite;

    public Image optionsButtonImage;
    public Image soundsButtonImage;
    public Image pauseSoundsButtonImage;
    public Image musicButtonImage;
    public Image pauseMusicButtonImage;

    [SerializeField] private RectTransform menuContainer;
    public bool inMainMenu = true;
    [SerializeField] private bool smooth;
    [SerializeField] private float smoothSpeed = 0.1f;
    [SerializeField] private Vector3 desiredPosition;
    [SerializeField] private Vector3[] menuPositions;

    public Button shop1Button;
    public Button shop2Button;
    private void Start()
    {

        soundsEnabled = !DataManager.status.enableSounds;//!AudioManager.audioManager.GetSoundState();
        Sounds();
        /*
        float soundVolume;
        audioMixer.GetFloat("SoundVolume", out soundVolume);

        switch(soundVolume)
        {
            case -80:
                soundsEnabled = true;
                Sounds();
                break;
            case 0:
                soundsEnabled = false;
                Sounds();
                break;
        }
        */
        /*
        float musicVolume;
        audioMixer.GetFloat("MusicVolume", out musicVolume);

        switch (musicVolume)
        {
            case -80:
                musicEnabled = true;
                Music();
                break;
            case 0:
                musicEnabled = false;
                Music();
                break;
        }
        */

        menuPositions = new Vector3[menuContainer.childCount];
        Vector3 halfScreen = new Vector3(Screen.width/2, Screen.height / 2, 0);
        for(int i = 0; i < menuPositions.Length; i++)
        {
            menuPositions[i] = menuContainer.GetChild(i).localPosition;
        }
    }

    private void Update()
    {
        if(smooth)
        {
            menuContainer.anchoredPosition = Vector3.Lerp(menuContainer.anchoredPosition, desiredPosition, smoothSpeed);
        }
        else
        {
            menuContainer.anchoredPosition = desiredPosition;
        }

        if(inOptions && optionsFiller.offsetMax.y > 0)
        {
            
            optionsFiller.offsetMax = Vector2.Lerp(optionsFiller.offsetMax, new Vector2(optionsFiller.offsetMax.x, 0.001f), Time.unscaledDeltaTime * 5);
            optionsFiller.offsetMin = Vector2.Lerp(optionsFiller.offsetMin, new Vector2(optionsFiller.offsetMin.x, 0.001f), Time.unscaledDeltaTime * 5);
        }
        else if(optionsFiller.offsetMax.y < 330)
        {
            
            optionsFiller.offsetMax = Vector2.Lerp(optionsFiller.offsetMax, new Vector2(optionsFiller.offsetMax.x, 329.999f), Time.unscaledDeltaTime * 5);
            optionsFiller.offsetMin = Vector2.Lerp(optionsFiller.offsetMin, new Vector2(optionsFiller.offsetMin.x, 329.999f), Time.unscaledDeltaTime * 5);
        }
    }
    public void MoveMenu(int id)
    {
        Debug.Log("desiredPosition " + desiredPosition);
        Debug.Log("-menuPositions[id] " + -menuPositions[id]);

        desiredPosition = -menuPositions[id];
    }

    public void Options()
    {
        switch(inOptions)
        {
            case false:
                optionsButtonImage.sprite = optionsSelected;
                inOptions = true; break;
            case true:
                optionsButtonImage.sprite = optionsUnselected;
                inOptions = false; break;
        }
    }

    public void NoAds()
    {
        Debug.Log("Open menu for disabling ads");
        AudioManager.PlaySound("menu_button");
    }

    public void Sounds()
    {
        switch(soundsEnabled)
        {
            case true:
                soundsEnabled = false;
                //AudioManager.PlaySound("menu_toggle_off");
                AudioManager.audioManager.EnableSound(false);
                soundsButtonImage.sprite = soundDisabledSprite;
                pauseSoundsButtonImage.sprite = soundDisabledSprite;
                Debug.Log("Disable sounds");
                //audioMixer.SetFloat("SoundVolume", -80);
                break;

            case false:
                soundsEnabled = true;
                AudioManager.audioManager.EnableSound(true);
                AudioManager.PlaySound("menu_toggle_on");
                soundsButtonImage.sprite = soundEnabledSprite;
                pauseSoundsButtonImage.sprite = soundEnabledSprite;
                //audioMixer.SetFloat("SoundVolume", 0);
                break;
        }
    }

    public void Music()
    {
        switch (musicEnabled)
        {
            case true:
                AudioManager.PlaySound("menu_toggle_off");
                musicEnabled = false;
                Debug.Log("Disable music");
                //audioMixer.SetFloat("MusicVolume", -80);
                musicButtonImage.sprite = musicDisabledSprite;
                pauseMusicButtonImage.sprite = musicDisabledSprite;
                break;
            case false:
                AudioManager.PlaySound("menu_toggle_on");
                musicEnabled = true;
                Debug.Log("Enable music");
                //audioMixer.SetFloat("MusicVolume", 0);
                musicButtonImage.sprite = musicEnabledSprite;
                pauseMusicButtonImage.sprite = musicEnabledSprite;
                break;
        }
    }

    public void inMain(bool isIn)
    {
        if(isIn)
        {
            //AudioManager.PlaySound("shop_open");
            AudioManager.PlaySound("shop_next_page");
            inMainMenu = true;
        }
        else
        {
            //AudioManager.PlaySound("shop_close");
            AudioManager.PlaySound("shop_next_page");
            inMainMenu = false;
        }
        
    }

    public void selectFirstShop()
    {
        AudioManager.PlaySound("shop_next_page");
        shop2Button.transform.GetComponent<Image>().color = Color.white;
        shop1Button.transform.GetComponent<Image>().color = Color.grey;
    }
    public void selectSecondShop()
    {
        AudioManager.PlaySound("shop_next_page");
        shop1Button.transform.GetComponent<Image>().color = Color.white;
        shop2Button.transform.GetComponent<Image>().color = Color.grey;
    }
}
