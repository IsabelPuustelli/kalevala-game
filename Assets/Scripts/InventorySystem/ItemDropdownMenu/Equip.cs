using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Equip : MonoBehaviour
{
    public Button item;
    public Transform trans;

    public GameObject quickslotDisplay;
    private WeaponQuickslotDisplay script;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = item.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        script = quickslotDisplay.GetComponent<WeaponQuickslotDisplay>();
        OpenItemDropdown parentScript = transform.parent.GetComponentInParent<OpenItemDropdown>();
    }
    public void CancelPressed()
    {
        EventSystem.current.SetSelectedGameObject(transform.parent.gameObject);
        OpenItemDropdown parentScript = transform.parent.GetComponentInParent<OpenItemDropdown>();
        parentScript.Cancelled();
    }
    void TaskOnClick()
    {
        switch (trans.parent.tag)
        {
            case "weaponItem": script.ChangeItem(trans.parent.gameObject); break;
        }
    }
}