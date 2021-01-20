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
        private DateTime timeoutHelper;
        private double timeoutAmount = 8;

        public static bool selectSlot = false;
        public static int currentStateSlot = -1;
        public static string path = Application.persistentDataPath + "/Savestates-1221";
        public const int maxSaveStates = 10;
        private static bool autoSlot;
        
        //public static bool preserveThroughStates = false;

        internal SaveStateManager()
        {
            try
            {
                selectSlot = false;
                DebugMod.settings.SaveStatePanelVisible = false;
                autoSlot = false;
                memoryState = new SaveState();
                if (!Directory.Exists(SaveStateManager.path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    RefreshStateMenu();
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
                            while (currentStateSlot + 1 != initSlot && ++i < maxSaveStates)
                            {
                                if (currentStateSlot + 1 >= maxSaveStates)
                                {
                                    currentStateSlot = 0;
                                }
                                else
                                {
                                    currentStateSlot++;
                                }

                                if (!saveStateFiles.ContainsKey(currentStateSlot))
                                {
                                    saveStateFiles.Add(currentStateSlot, new SaveState());
                                }
                            }
                            if (currentStateSlot == initSlot)
                            {
                                // TODO: Inquire if want to overwrite
                                currentStateSlot--;
                                if (currentStateSlot < 0) currentStateSlot = maxSaveStates - 1;
                                saveStateFiles.Remove(currentStateSlot);
                                saveStateFiles.Add(currentStateSlot, new SaveState());
                            }
                            if (!memoryState.IsSet()) memoryState.SaveTempState();
                            saveStateFiles[currentStateSlot].SaveStateToFile(currentStateSlot);
                        }
                        else
                        {
                            StartCoroutine(SelectSlot(true, stateType));
                        }
                    }
                    break;
                case SaveStateType.SkipOne:
                    StartCoroutine(SelectSlot(true, stateType));
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
                    StartCoroutine(SelectSlot(false, stateType));
                    saveStateFiles[currentStateSlot].PrepareFileStateToMemory(currentStateSlot);
                    break;
                case SaveStateType.SkipOne:
                    StartCoroutine(SelectSlot(false, stateType));
                    saveStateFiles[currentStateSlot].LoadStateFromFile();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region helper functionality
        private IEnumerator SelectSlot(bool save, SaveStateType stateType)
        {
            timeoutHelper = DateTime.Now.AddSeconds(timeoutAmount);
            DebugMod.settings.SaveStatePanelVisible = selectSlot = true;
           
            //Console.AddLine("coro test (pre if): " + tmp++);
            yield return new WaitUntil(DidInput);
            
            if (GUIController.didInput)
            {
                if (currentStateSlot >= 0 || currentStateSlot < maxSaveStates)
                {
                    if (save)
                    {
                        SaveCoroHelper(stateType);
                    }
                    else
                    {
                        LoadCoroHelper(stateType);
                    }
                    GUIController.didInput = false;
                }
            }
            else
            {
                Console.AddLine("Timeout (" + timeoutAmount + "s) reached");
            }

            yield return new WaitForSeconds(3);
            DebugMod.settings.SaveStatePanelVisible = selectSlot = false;
        }

        private void LoadCoroHelper(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.File:
                    //if (!memoryState.IsSet()) memoryState.SaveTempState();
                    saveStateFiles[currentStateSlot].PrepareFileStateToMemory(currentStateSlot);
                    break;
                case SaveStateType.SkipOne:
                    saveStateFiles[currentStateSlot].LoadStateFromFile();
                    break;
                default:
                    break;
            }
        }

        private void SaveCoroHelper(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.File:
                    if (!memoryState.IsSet()) memoryState.SaveTempState();
                    saveStateFiles[currentStateSlot] = memoryState;
                    saveStateFiles[currentStateSlot].SaveStateToFile(currentStateSlot);
                    break;
                case SaveStateType.SkipOne:
                    saveStateFiles[currentStateSlot] = new SaveState();
                    saveStateFiles[currentStateSlot].SaveTempState();
                    saveStateFiles[currentStateSlot].SaveStateToFile(currentStateSlot);
                    break;
                default:
                    break;
            }
        }

        private bool DidInput()
        {
            if (GUIController.didInput)
            {
                return true;
            }
            else if (timeoutHelper < DateTime.Now)
            {
                return true;
            }
            return false;
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

        public void RefreshStateMenu()
        {
            try
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    var digits = file.SkipWhile(c => !Char.IsDigit(c)).TakeWhile(Char.IsDigit).ToArray();
                    var str = new string(digits);
                    int slot = int.Parse(str);
                    // TODO: read savestate files enough to get summary
                    if (File.Exists(file) && (slot < maxSaveStates || slot >= 0))
                    {
                        SaveState tmpData = JsonUtility.FromJson<SaveState>(File.ReadAllText(file));
                        saveStateFiles.Remove(slot);
                        saveStateFiles.Add(slot, tmpData);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
