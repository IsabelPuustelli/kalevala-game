using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    public string SavePath => $"{Application.persistentDataPath}/save.dat";

    [SerializeField] bool _useEncryption;

    GameData _data;
    List<ISaveable> _saveables;
    private readonly string _encryptionSecret = "ukko";

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        // Find all saveable game objects with ISaveable interface
        _saveables = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
    }

    void Start() => LoadGame();

    void NewGame()
    {
        _data = new GameData();
        SaveGame();
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
            NewGame();

        // load json file
        string json = File.ReadAllText(SavePath);
        json = _useEncryption ? EncryptDecrypt(json) : json;

        _data = JsonUtility.FromJson<GameData>(json);

        foreach (var saveable in _saveables)
            saveable.LoadData(_data);

    }

    public void SaveGame()
    {
        Debug.Log("Saving game");

        foreach (var obj in _saveables)
            obj.SaveData(ref _data);

        string data = JsonUtility.ToJson(_data, true);
        data = _useEncryption ? EncryptDecrypt(data) : data;
        File.WriteAllText(SavePath, data);
    }

    [ContextMenu("Delete Save")]
    public void DeleteSave() => File.Delete(SavePath);

    // Encrypt data using simple XOR
    string EncryptDecrypt(string toEncrypt)
    {
        string modifiedData = "";
        for (int i = 0; i < toEncrypt.Length; i++)
            modifiedData += (char)(toEncrypt[i] ^ _encryptionSecret[i % _encryptionSecret.Length]);

        return modifiedData;
    }
}
