using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //ESIMERKKI - How to use
    //AudioManager.PlaySound("music_gameplay");
    //AudioManager.EnableSound(true);
    //AudioManager.GetSoundState(); (bool)

    public static AudioManager audioManager;

    public static AudioClip music_gameplay;

    public static AudioClip
        menu_button,
        menu_pause,
        menu_unpause,
        menu_toggle_off,
        menu_toggle_on,

        shop_unlock_charaster,
        shop_upgrade_1_star,
        shop_upgrade_2_star,
        shop_upgrade_3_star,
        shop_buy_no_money,
        shop_open,
        shop_close,
        shop_next_page,

        gameover_menu_open,

        porky_boing_bird,
        porky_boing_owl,

        porky_bounce,
        porky_sling,
        porky_boing,
        porky_boing_bear,
        porky_boing_bear_sleeping,
        porky_boing_slowdown,
        porky_airboost,
        porky_thunder,

        //music_gameplay,
    music_stop;

    static AudioSource audioSource;

    private void Awake()
    {
        if (audioManager == null)
        {
            DontDestroyOnLoad(gameObject);
            audioManager = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);

        }
 
    }

    void Start()
    {
        
        menu_button = Resources.Load<AudioClip>("menu_button");
        menu_pause = Resources.Load<AudioClip>("menu_pause");
        menu_unpause = Resources.Load<AudioClip>("menu_unpause");
        menu_toggle_off = Resources.Load<AudioClip>("menu_toggle_off");
        menu_toggle_on = Resources.Load<AudioClip>("menu_toggle_on");

        shop_unlock_charaster = Resources.Load<AudioClip>("shop_unlock_charaster");
        shop_upgrade_1_star = Resources.Load<AudioClip>("shop_upgrade_1_star");
        shop_upgrade_2_star = Resources.Load<AudioClip>("shop_upgrade_2_star");
        shop_upgrade_3_star = Resources.Load<AudioClip>("shop_upgrade_3_star");
        shop_buy_no_money = Resources.Load<AudioClip>("shop_buy_no_money");
        shop_open = Resources.Load<AudioClip>("shop_open");
        shop_close = Resources.Load<AudioClip>("shop_close");
        shop_next_page = Resources.Load<AudioClip>("shop_next_page");

        gameover_menu_open = Resources.Load<AudioClip>("gameover_menu_open");

        porky_boing_bird = Resources.Load<AudioClip>("porky_boing_bird");
        porky_boing_owl = Resources.Load<AudioClip>("porky_boing_owl");

        porky_thunder = Resources.Load<AudioClip>("porky_thunder");
        porky_bounce = Resources.Load<AudioClip>("porky_bounce");
        porky_sling = Resources.Load<AudioClip>("porky_sling");
        porky_boing = Resources.Load<AudioClip>("porky_boing");
        porky_boing_bear = Resources.Load<AudioClip>("porky_boing_bear");
        porky_boing_bear_sleeping = Resources.Load<AudioClip>("porky_boing_bear_sleeping");
        porky_boing_slowdown = Resources.Load<AudioClip>("porky_boing_slowdown");
        porky_airboost = Resources.Load<AudioClip>("porky_airboost");
        

        music_gameplay = Resources.Load<AudioClip>("music_gameplay");
        
    }

    // Update is called once per frame
    void Update()
    {


    }


    public static void PlaySound(string clip)
    {
        
        switch (clip)
        {
            case "menu_button": audioSource.PlayOneShot(menu_button); break;
            case "menu_pause": audioSource.PlayOneShot(menu_pause); break;
            case "menu_unpause": audioSource.PlayOneShot(menu_unpause); break;
            case "menu_toggle_off": audioSource.PlayOneShot(menu_toggle_off); break;
            case "menu_toggle_on": audioSource.PlayOneShot(menu_toggle_on); break;

            case "shop_unlock_charaster": audioSource.PlayOneShot(shop_unlock_charaster); break;
            case "shop_upgrade_1_star": audioSource.PlayOneShot(shop_upgrade_1_star); break;
            case "shop_upgrade_2_star": audioSource.PlayOneShot(shop_upgrade_2_star); break;
            case "shop_upgrade_3_star": audioSource.PlayOneShot(shop_upgrade_3_star); break;
            case "shop_buy_no_money": audioSource.PlayOneShot(shop_buy_no_money); break;
            case "shop_open": audioSource.PlayOneShot(shop_open); break;
            case "shop_close": audioSource.PlayOneShot(shop_close); break;
            case "shop_next_page": audioSource.PlayOneShot(shop_next_page); break;

            case "gameover_menu_open": audioSource.PlayOneShot(gameover_menu_open); break;

                
            case "porky_thunder": audioSource.PlayOneShot(porky_thunder); break;
            case "porky_boing_bird": audioSource.PlayOneShot(porky_boing_bird); break;
            case "porky_boing_owl": audioSource.PlayOneShot(porky_boing_owl); break;

            case "porky_bounce": audioSource.PlayOneShot(porky_bounce); break;
            case "porky_sling": audioSource.PlayOneShot(porky_sling); break;
            case "porky_boing": audioSource.PlayOneShot(porky_boing); break;
            case "porky_boing_bear": audioSource.PlayOneShot(porky_boing_bear); break;
            case "porky_boing_bear_sleeping": audioSource.PlayOneShot(porky_boing_bear_sleeping); break;
            case "porky_boing_slowdown": audioSource.PlayOneShot(porky_boing_slowdown); break;
            case "porky_airboost": audioSource.PlayOneShot(porky_airboost); break;

            case "music_gameplay": audioSource.PlayOneShot(music_gameplay); break;
            case "music_stop": audioSource.Stop(); break;
                
        }
    }

    public void EnableSound(bool state)
    {
        if (audioSource != null)
        {
            switch (state)
            {
                case false:
                    audioSource.volume = 0;
                    break;
                case true:
                    audioSource.volume = 80;
                    break;
            }
        }
        DataManager.status.Save();
    }

    public void EnableMusic(bool state)
    {
        /*
        if (audioSource != null)
        {
            switch (state)
            {
                case false:
                    audioSource.volume = 0;
                    break;
                case true:
                    audioSource.volume = 80;
                    break;
            }
        }
        */
    }

    public bool GetSoundState()
    {
        if (audioSource != null)
        {
            if (audioSource.volume > 0)
            {
                return true;
            }
        }

        return false;
    }

}
