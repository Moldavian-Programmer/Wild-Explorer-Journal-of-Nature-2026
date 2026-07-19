using UnityEngine;

public class SavePointInteractable : MonoBehaviour
{
    [Header("Save Menu")]
    [SerializeField] private SaveLoadMenuController saveLoadMenuController;

    [Header("Player")]
    [SerializeField] private Transform playerRoot;

    public void OpenSaveMenu()
    {
        if (saveLoadMenuController == null)
        {
            Debug.LogError("SaveLoadMenuController is missing.");
            return;
        }

        saveLoadMenuController.OpenForSave(playerRoot);
    }
}