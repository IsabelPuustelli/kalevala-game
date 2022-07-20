using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Start()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void RestAtCheckpoint()
    {
        Debug.Log("Resting at checkpoint");
        SaveSystem.Instance.SaveGame();
        UserInterface.Instance.LargePrompt("Saved!", 1.5f);
    }
}
