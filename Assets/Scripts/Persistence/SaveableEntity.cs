/* using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    [SerializeField] string _id;
    public string Id => _id;

    [ContextMenu("Generate ID")]
    void GenerateId() => _id = Guid.NewGuid().ToString();

    void Awake()
    {
        // If no ID was set, generate one
        if (string.IsNullOrEmpty(_id))
        {
            GenerateId();
            Debug.LogWarning($"{name} has no ID, generated one: {_id}");
        }
    }


    // Iterate all components with ISaveable interface and save their state
    public object SaveState()
    {
        var state = new Dictionary<string, object>();
        foreach (var saveable in GetComponents<ISaveable>())
            state[saveable.GetType().ToString()] = saveable.SaveState();

        return state;
    }

    // Iterate all components with ISaveable interface and load their state
    public void LoadState(object data)
    {
        var state = (Dictionary<string, object>)data;
        foreach (var saveable in GetComponents<ISaveable>())
        {
            string type = saveable.GetType().ToString();
            if (state.TryGetValue(type, out object stateData))
                saveable.LoadState(stateData);
        }
    }
}
 */