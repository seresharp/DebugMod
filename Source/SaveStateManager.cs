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
        public static bool saveStateHUD = false;
        public static bool autoSlotSelect = true;
        //public static bool preserveThroughStates = false;

        internal SaveStateManager()
        {
            try
            {
                saveStateHUD = false;
                memoryState = new SaveState();
                if (Directory.Exists(SaveStateManager.path))
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
                        if (autoSlotSelect)
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
                            // slot selection algo:
                            //  ++current
                            //  if (open slot[current]) states[current] = new Obj() return? 
                            //  if oob current = 0
                            //  loop
                        }

                        if (!memoryState.IsSet()) memoryState.SaveTempState();
                        saveStateFiles[currentStateSlot].SaveStateToFile(memoryState, currentStateSlot);
                    }
                    break;
                case SaveStateType.SkipOne:
                    
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
                    {
                        // change currentStateSlot
                    }
                    saveStateFiles[currentStateSlot].PrepareFileStateToMemory(currentStateSlot);
                    break;
                case SaveStateType.SkipOne:
                    if (!saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        // change currentStateSlot
                    }
                    saveStateFiles[currentStateSlot].LoadStateFromFile();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region helper functionality
        private IEnumerator SelectSlot()
        {
            saveStateHUD = false;
            yield return null;

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
            // ui hud to select slot
            saveStateHUD = false;
            yield return null;
        }

        public void ToggleAutoSlot()
        {
            autoSlotSelect = !autoSlotSelect;
        }
        #endregion
    }
}
