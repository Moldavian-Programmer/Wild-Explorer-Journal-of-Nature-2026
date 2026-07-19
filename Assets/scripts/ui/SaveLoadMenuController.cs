using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadMenuController : MonoBehaviour
{
    private enum MenuMode
    {
        Load,
        Save
    }

    [Header("Panels")]
    [SerializeField] private GameObject loadGamePanel;

    [Header("Slot Buttons")]
    [SerializeField] private Button slot1Button;
    [SerializeField] private Button slot2Button;
    [SerializeField] private Button slot3Button;

    [Header("Slot Texts")]
    [SerializeField] private TMP_Text slot1Text;
    [SerializeField] private TMP_Text slot2Text;
    [SerializeField] private TMP_Text slot3Text;

    [Header("Player")]
    [SerializeField] private Transform playerRoot;

    [Header("Scene")]
    [SerializeField] private string defaultGameplaySceneName = "mapa";

    private MenuMode currentMode = MenuMode.Load;

    private void Start()
    {
        RefreshSlotLabels();
    }

    public void OpenForLoad()
    {
        currentMode = MenuMode.Load;

        if (loadGamePanel != null)
        {
            loadGamePanel.SetActive(true);
        }

        RefreshSlotLabels();
        RefreshSlotButtonStates();
    }

    public void OpenForSave(Transform playerTransform)
    {
        currentMode = MenuMode.Save;

        if (playerTransform != null)
        {
            playerRoot = playerTransform;
        }

        if (loadGamePanel != null)
        {
            loadGamePanel.SetActive(true);
        }

        RefreshSlotLabels();
        SetAllSlotsInteractable(true);
    }

    public void ClosePanel()
    {
        if (loadGamePanel != null)
        {
            loadGamePanel.SetActive(false);
        }
    }

    public void UseSlot1()
    {
        UseSlot(1);
    }

    public void UseSlot2()
    {
        UseSlot(2);
    }

    public void UseSlot3()
    {
        UseSlot(3);
    }

    private void UseSlot(int slotIndex)
    {
        if (currentMode == MenuMode.Save)
        {
            SaveToSlot(slotIndex);
            return;
        }

        LoadFromSlot(slotIndex);
    }

    private void SaveToSlot(int slotIndex)
    {
        Transform targetPlayer = playerRoot;

        if (targetPlayer == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                targetPlayer = playerObject.transform;
            }
        }

        if (targetPlayer == null)
        {
            Debug.LogError("Cannot save: playerRoot is missing and no GameObject tagged Player was found.");
            return;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        GameSaveSystem.SaveSlot(slotIndex, targetPlayer, sceneName);

        RefreshSlotLabels();
    }

    private void LoadFromSlot(int slotIndex)
    {
        GameSaveData data = GameSaveSystem.LoadSlot(slotIndex);

        if (data == null)
        {
            Debug.LogWarning("Cannot load slot " + slotIndex + " because it is empty.");
            return;
        }

        StartCoroutine(LoadSavedScene(data));
    }

    private IEnumerator LoadSavedScene(GameSaveData data)
    {
        string sceneToLoad = string.IsNullOrWhiteSpace(data.sceneName)
            ? defaultGameplaySceneName
            : data.sceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return null;

        Transform targetPlayer = FindPlayerAfterSceneLoad();

        if (targetPlayer != null)
        {
            targetPlayer.position = data.playerPosition;
            targetPlayer.rotation = data.playerRotation;
        }
        else
        {
            Debug.LogWarning("Loaded scene, but player object was not found. Add Player tag to XR Origin or assign playerRoot.");
        }

        Time.timeScale = 1f;
    }

    private Transform FindPlayerAfterSceneLoad()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            return playerObject.transform;
        }

        if (Camera.main != null)
        {
            return Camera.main.transform.root;
        }

        return null;
    }

    private void RefreshSlotLabels()
    {
        if (slot1Text != null)
        {
            slot1Text.text = GameSaveSystem.GetSlotLabel(1);
        }

        if (slot2Text != null)
        {
            slot2Text.text = GameSaveSystem.GetSlotLabel(2);
        }

        if (slot3Text != null)
        {
            slot3Text.text = GameSaveSystem.GetSlotLabel(3);
        }
    }

    private void RefreshSlotButtonStates()
    {
        if (currentMode == MenuMode.Save)
        {
            SetAllSlotsInteractable(true);
            return;
        }

        if (slot1Button != null)
        {
            slot1Button.interactable = GameSaveSystem.SlotExists(1);
        }

        if (slot2Button != null)
        {
            slot2Button.interactable = GameSaveSystem.SlotExists(2);
        }

        if (slot3Button != null)
        {
            slot3Button.interactable = GameSaveSystem.SlotExists(3);
        }
    }

    private void SetAllSlotsInteractable(bool value)
    {
        if (slot1Button != null)
        {
            slot1Button.interactable = value;
        }

        if (slot2Button != null)
        {
            slot2Button.interactable = value;
        }

        if (slot3Button != null)
        {
            slot3Button.interactable = value;
        }
    }
}