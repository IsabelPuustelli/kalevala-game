using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

public class OpenItemDropdown : MonoBehaviour
{
    // Start is called before the first frame update
	public Button item;
	public Button use;
	public Button examine;
	public Button drop;

	void Start()
	{
		Button btn = item.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

    void TaskOnClick()
	{
		Instantiate(use, new Vector2(transform.position.x + -75f, transform.position.y), Quaternion.identity, transform.parent);
		Instantiate(examine, new Vector2(transform.position.x + -75f, transform.position.y - 40f), Quaternion.identity, transform.parent);
		Instantiate(drop, new Vector2(transform.position.x + -75f, transform.position.y - 80f), Quaternion.identity, transform.parent);

		GameObject firstChild = gameObject.transform.parent.GetChild(1).gameObject;
		EventSystem.current.SetSelectedGameObject(firstChild);
	}
	
	public void Cancelled()
    {
		GameObject.Destroy(transform.GetChild(1).gameObject);
		GameObject.Destroy(transform.GetChild(2).gameObject);
		GameObject.Destroy(transform.GetChild(3).gameObject);
	}
	
}