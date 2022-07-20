using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponQuickslotDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeItem(GameObject item)
    {
        Transform parent = GameObject.Find("WeaponQuickslot").transform;
        Instantiate(item, new Vector3(parent.position.x, parent.position.y, parent.position.z), Quaternion.identity, parent);
        Transform weapon = parent.transform.GetChild(0);
        foreach (Transform child in weapon)
            GameObject.Destroy(child.gameObject);
    }
}