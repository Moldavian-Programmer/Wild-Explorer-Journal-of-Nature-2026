using System;
using System.IO;
using UnityEngine;

public static class GameSaveSystem
{
    private static string SaveFolderPath
    {
        get
        {
            string path = Path.Combine(Application.persistentDataPath, "saves");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }

    public static string GetSlotPath(int slotIndex)
    {
        return Path.Combine(SaveFolderPath, "slot_" + slotIndex + ".json");
    }

    public static bool SlotExists(int slotIndex)
    {
        return File.Exists(GetSlotPath(slotIndex));
    }

    public static void SaveSlot(int slotIndex, Transform playerTransform, string sceneName)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Save failed: player transform is missing.");
            return;
        }

        GameSaveData data = new GameSaveData
        {
            slotIndex = slotIndex,
            savedAtText = DateTime.Now.ToString("dd/MM/yy HH/mm"),
            sceneName = sceneName,
            playerPosition = playerTransform.position,
            playerRotation = playerTransform.rotation
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSlotPath(slotIndex), json);

        Debug.Log("Game saved in slot " + slotIndex + " at " + data.savedAtText);
    }

    public static GameSaveData LoadSlot(int slotIndex)
    {
        string path = GetSlotPath(slotIndex);

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save found in slot " + slotIndex);
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public static string GetSlotLabel(int slotIndex)
    {
        GameSaveData data = LoadSlot(slotIndex);

        if (data == null || string.IsNullOrWhiteSpace(data.savedAtText))
        {
            return "Slot " + slotIndex;
        }

        return data.savedAtText;
    }
}