using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public enum SaveStateType {
        Memory,
        File,
        SkipOne
    }

    /// <summary>
    /// Handles organisation of SaveState-s
    /// memoryState replicating legacy behaviour of only stored in RAM.
    /// Dictionary(int slot : file). Might change to HashMap(?) 
    ///  if: memory requirement too high: array for limiting savestates? hashmap as all states should logically be unique?
    /// HUD for viewing necessary info for UX.
    /// AutoSlotSelect to iterate over slots, eventually overwrite when circled and no free slots.
    /// </summary>
    internal class SaveStateManager : MonoBehaviour
    {
        public static SaveState memoryState;
        private static Dictionary<int, SaveState> saveStateFiles = new Dictionary<int, SaveState>();

        public static int currentStateSlot = -1;
        public static string path = Application.persistentDataPath + "/Savestates-1221";
        public const int maxSaveStates = 10;
        private static bool autoSlot;
        
        //public static bool preserveThroughStates = false;

        internal SaveStateManager()
        {
            try
            {
                DebugMod.settings.SaveStatePanelVisible = false;
                autoSlot = false;
                memoryState = new SaveState();
                if (!Directory.Exists(SaveStateManager.path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region saving
        public void SaveState(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.Memory:
                    if (!memoryState.IsSet()) memoryState.SaveTempState();
                    memoryState.SaveTempState();
                    break;
                case SaveStateType.File:
                    if (memoryState.IsSet())
                    {
                        if (autoSlot)
                        {
                            int i = 0;
                            int initSlot = currentStateSlot;

                            // Refactor using dict.keys()?
                            while (++currentStateSlot != initSlot && ++i < maxSaveStates)
                            {
                                if (currentStateSlot == maxSaveStates)
                                {
                                    currentStateSlot = 0;
                                }
                                if (!saveStateFiles.ContainsKey(currentStateSlot))
                                {
                                    saveStateFiles.Add(currentStateSlot, new SaveState());
                                }
                            }
                            if (currentStateSlot == initSlot)
                            {
                                // TODO: Inquire if want to overwrite
                                if (--currentStateSlot < 0) currentStateSlot = maxSaveStates - 1;
                                saveStateFiles.Remove(currentStateSlot);
                                saveStateFiles.Add(currentStateSlot, new SaveState());
                            }
                        }
                        else
                        {
                            
                            DebugMod.settings.SaveStatePanelVisible = true;
                            while (!saveStateFiles.ContainsKey(currentStateSlot))
                            {
                                SelectSlot();
                            }

                            DebugMod.settings.SaveStatePanelVisible = false;
                            saveStateFiles[currentStateSlot].LoadStateFromFile();
                            break;
                        }

                        if (!memoryState.IsSet()) memoryState.SaveTempState();
                        saveStateFiles[currentStateSlot].SaveStateToFile(memoryState, currentStateSlot);
                    }
                    break;
                case SaveStateType.SkipOne:

                    DebugMod.settings.SaveStatePanelVisible = true;
                    while (!saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        SelectSlot();
                    }

                    DebugMod.settings.SaveStatePanelVisible = false;
                    saveStateFiles[currentStateSlot].LoadStateFromFile();
                    break; 
                default: break;
            }
        }



        #endregion

        #region loading
        public void LoadState(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.Memory:
                    if (memoryState.IsSet())
                    {
                        memoryState.LoadTempState();
                    }
                    else
                    {
                        Console.AddLine("No save state active");
                    }
                    break;
                case SaveStateType.File:
                    if (!saveStateFiles.ContainsKey(currentStateSlot))
                        DebugMod.settings.SaveStatePanelVisible = true;
                    while (!saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        SelectSlot();
                    }

                    DebugMod.settings.SaveStatePanelVisible = false;
                    saveStateFiles[currentStateSlot].PrepareFileStateToMemory(currentStateSlot);
                    break;
                case SaveStateType.SkipOne:
                    DebugMod.settings.SaveStatePanelVisible = true;
                    while (!saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        SelectSlot();
                    }

                    DebugMod.settings.SaveStatePanelVisible = false;
                    saveStateFiles[currentStateSlot].LoadStateFromFile();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region helper functionality
        private void SelectSlot()
        {
            foreach (KeyValuePair<KeyCode, int> entry in DebugMod.alphaKeyDict)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    if (DebugMod.alphaKeyDict.TryGetValue(entry.Key, out int keyInt))
                    {
                        // keyInt should be between 0-9
                        currentStateSlot = keyInt;
                    }
                }
            }
        }

        public void ToggleAutoSlot()
        {
            autoSlot = !autoSlot;
        }

        public static bool GetAutoSlot()
        {
            return autoSlot;
        }

        public static int GetCurrentSlot()
        {
            return currentStateSlot;
        }

        public string[] GetCurrentMemoryState()
        {
            if (memoryState.IsSet())
            {
                return memoryState.GetSaveStateInfo();
            }
            return null;
        }

        public static bool HasFiles()
        {
            return (saveStateFiles.Count != 0);
        }

        public static Dictionary<int, string[]> GetSaveStatesInfo()
        {
            Dictionary<int, string[]> returnData = new Dictionary<int, string[]>();
            foreach (KeyValuePair<int, SaveState> stateData in saveStateFiles)
            {
                if (stateData.Value.IsSet())
                {
                    returnData.Add(stateData.Key, stateData.Value.GetSaveStateInfo());
                }
            }
            return returnData;
        }
            
        #endregion
    }
}
