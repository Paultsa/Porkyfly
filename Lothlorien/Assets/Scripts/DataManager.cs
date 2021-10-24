using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DataManager : MonoBehaviour
{
    public static DataManager status;
    //[SerializeField] private PlayerData playerData;
    //[SerializeField] private GameManager gameManager;

    public int currency = 0;
    public float bestDistance = 0;
    public int[] boughtUpgrades;
    public int[] boughtUpgradeLevels;

    public bool enableMusic;
    public bool enableSounds;
    //public int highScores = 0;
    //public int scores = 0;

    //public static int endDistance;
    //bestDistance = DataManager.status.bestDistance;
    //DataManager.status.Load();

    void Awake()
    {
        if (status == null)
        {
            DontDestroyOnLoad(gameObject);
            status = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath + "/playerInfo.dat");
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        currency = PlayerData.playerData.currency;
        bestDistance = PlayerData.playerData.bestDistance;
        boughtUpgrades = PlayerData.playerData.boughtUpgrades;
        boughtUpgradeLevels = PlayerData.playerData.boughtUpgradeLevels;
        enableMusic = false;//AudioManager.audioManager.GetSoundState(); //KORJAA TÄHÄN MUSIIKEILLE OMA FUNKTIO
        enableSounds = AudioManager.audioManager.GetSoundState();

        //Debug.Log("Scores: " + bestDistance);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerInfo data = new PlayerInfo();

        data.bestDistance = bestDistance;
        data.currency = currency;
        data.boughtUpgrades = boughtUpgrades;
        data.boughtUpgradeLevels = boughtUpgradeLevels;
        data.enableMusic = enableMusic;
        data.enableSounds = enableSounds;

        Debug.Log("SOUNDS: " + enableSounds);

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerInfo data = (PlayerInfo)bf.Deserialize(file);

            bestDistance = data.bestDistance;
            currency = data.currency;
            boughtUpgrades = data.boughtUpgrades;
            boughtUpgradeLevels = data.boughtUpgradeLevels;
            enableMusic = data.enableMusic;
            enableSounds = data.enableSounds;

            Debug.Log("High scores: " + bestDistance);
            file.Close();
        }

        PlayerData.playerData.currency = currency;
        PlayerData.playerData.bestDistance = bestDistance;
        PlayerData.playerData.boughtUpgrades = boughtUpgrades;
        PlayerData.playerData.boughtUpgradeLevels = boughtUpgradeLevels;
        AudioManager.audioManager.EnableMusic(false);
        AudioManager.audioManager.EnableSound(enableSounds);

        Debug.Log("SOUNDS LOAD  : " + enableSounds);
    }

    [System.Serializable]
    class PlayerInfo
    {
        public int currency;
        public float bestDistance;
        public int[] boughtUpgrades;
        public int[] boughtUpgradeLevels;
        public bool enableMusic;
        public bool enableSounds;
    }
}
