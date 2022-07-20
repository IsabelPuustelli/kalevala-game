using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Examine : MonoBehaviour
{
    public void CancelPressed()
    {
        EventSystem.current.SetSelectedGameObject(transform.parent.gameObject);
        OpenItemDropdown parentScript = transform.parent.GetComponentInParent<OpenItemDropdown>();
        parentScript.Cancelled();
    }
}
