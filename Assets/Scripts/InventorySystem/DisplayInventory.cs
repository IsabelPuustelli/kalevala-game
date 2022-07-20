using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DisplayInventory : MonoBehaviour
{
    private InventoryObject inventory;
    private CanvasGroup canvasGroup;
    public CanvasGroup inventoryButtons;
    public CanvasGroup pauseMenuButtons;

    public InventoryObject defaultInventory;
    public InventoryObject foodInventory;
    public InventoryObject weaponsInventory;
    public GameObject dropdownOpener;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMNS;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    string buttonName;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
        inventory = defaultInventory;
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            switch (EventSystem.current.currentSelectedGameObject.name)
            {
                case "MenuSelection": canvasGroup.alpha = 0; break;
                default: canvasGroup.alpha = 1; break;
            }

            if (EventSystem.current.currentSelectedGameObject.name != buttonName && inventoryButtons.interactable == true)
            {
                foreach (Transform child in transform)
                    GameObject.Destroy(child.gameObject);

                switch (EventSystem.current.currentSelectedGameObject.name)
                {
                    case "Default": inventory = defaultInventory; break;
                    case "Food": inventory = foodInventory; break;
                    case "Weapons": inventory = weaponsInventory; ; break;
                }
                itemsDisplayed.Clear();
                CreateDisplay();
            }
            buttonName = EventSystem.current.currentSelectedGameObject.name;
        }
        //UpdateDisplay();
    }

    public void CreateDisplay()    // We create the display for the inventory by Instantiating an object of each item into the grid created in GetPosition, then setting a text for the amount of that object
    {
        for (int i = 0; i < inventory.Inventory.Count; i++)
        {
            var obj = Instantiate(inventory.Inventory[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Inventory[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Inventory[i], obj);
            var btn = Instantiate(dropdownOpener, GetPosition(i), Quaternion.identity, transform.GetChild(transform.childCount - 1));
            btn.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }
    /*
    public void UpdateDisplay()     // Updates the display in case any items have been added to the inventory
    {
        for (int i = 0; i < inventory.Inventory.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Inventory[i]))
                itemsDisplayed[inventory.Inventory[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Inventory[i].amount.ToString("n0");
            else
            {
                var obj = Instantiate(inventory.Inventory[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Inventory[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Inventory[i], obj);
            }
        }
    }*/
    public Vector3 GetPosition(int i)   //Function for easier setting of the size of the grid for the inventory
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }

    public void OpenInventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Open Inventory");

        inventoryButtons.interactable = false;
        pauseMenuButtons.interactable = false;
        GameObject firstChild = gameObject.transform.GetChild(0).GetChild(1).gameObject;
        EventSystem.current.SetSelectedGameObject(firstChild);
    }

    public void ShowInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (transform.parent.gameObject.activeSelf)
            {
                transform.parent.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                transform.parent.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
