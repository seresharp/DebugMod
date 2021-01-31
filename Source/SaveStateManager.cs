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
    internal class SaveStateManager
    {
        public const int maxSaveStates = 5;

        public static SaveState memoryState;
        public static bool inSelectSlotState = false;
        public static int currentStateSlot = -1;
        public static string path = Application.persistentDataPath + "/Savestates-1221/";

        private static Dictionary<int, SaveState> saveStateFiles = new Dictionary<int, SaveState>();
        private static bool autoSlot;
        private DateTime timeoutHelper;
        private double timeoutAmount = 8;

        //public static bool preserveThroughStates = false;

        internal SaveStateManager()
        {
            try
            {
                inSelectSlotState = false;
                autoSlot = false;
                DebugMod.settings.SaveStatePanelVisible = false;
                memoryState = new SaveState();

                if (!Directory.Exists(path))
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
                    memoryState.SaveTempState();
                    break;
                case SaveStateType.File or SaveStateType.SkipOne:
                    AutoSlotSelect(stateType);
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
                case SaveStateType.File or SaveStateType.SkipOne:
                    GameManager.instance.StartCoroutine(SelectSlot(false, stateType));
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
            DebugMod.settings.SaveStatePanelVisible = inSelectSlotState = true;

            //Console.AddLine("didInput bool, pre-WaitUntil(DidInput): " + GUIController.didInput.ToString());

            //Console.AddLine("coro test (pre if): " + tmp++);
            yield return new WaitUntil(DidInput);

            //Console.AddLine("didInput bool, post-WaitUntil(DidInput): " + GUIController.didInput.ToString());


            if (GUIController.didInput)
            {
                if (currentStateSlot >= 0 && currentStateSlot < maxSaveStates)
                {
                    if (save)
                    {
                        SaveCoroHelper(stateType);
                    }
                    else
                    {
                        LoadCoroHelper(stateType);
                    }
                }
                GUIController.didInput = false;
            }
            else
            {
                Console.AddLine("Timeout (" + timeoutAmount + "s) reached");
            }

            //yield return new WaitForSeconds(2);
            DebugMod.settings.SaveStatePanelVisible = inSelectSlotState = false;
        }

        private void SaveCoroHelper(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.File:
                    if (memoryState == null || !memoryState.IsSet())
                    {
                        memoryState.SaveTempState();
                    }
                    if (saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        saveStateFiles.Remove(currentStateSlot);
                    }
                    saveStateFiles.Add(currentStateSlot, new SaveState());
                    saveStateFiles[currentStateSlot].data = memoryState.data;
                    saveStateFiles[currentStateSlot].SaveStateToFile(currentStateSlot);
                    break;
                case SaveStateType.SkipOne:
                    if (saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        saveStateFiles.Remove(currentStateSlot);
                    }
                    saveStateFiles.Add(currentStateSlot, new SaveState());
                    saveStateFiles[currentStateSlot].NewSaveStateToFile(currentStateSlot);
                    break;
                default:
                    break;
            }
        }

        private void LoadCoroHelper(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.File:
                    if (saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        saveStateFiles.Remove(currentStateSlot);
                    }
                    saveStateFiles.Add(currentStateSlot, new SaveState());
                    saveStateFiles[currentStateSlot].LoadStateFromFile(currentStateSlot);
                    memoryState = saveStateFiles[currentStateSlot];
                    break;
                case SaveStateType.SkipOne:
                    if (saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        saveStateFiles.Remove(currentStateSlot);
                    }
                    saveStateFiles.Add(currentStateSlot, new SaveState());
                    saveStateFiles[currentStateSlot].NewLoadStateFromFile();
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
            if (HasFiles())
            {
                foreach (KeyValuePair<int, SaveState> stateData in saveStateFiles)
                {
                    if (stateData.Value.IsSet())
                    {
                        returnData.Add(stateData.Key, stateData.Value.GetSaveStateInfo());
                    }
                }
            }
            return returnData;
        }

        public void RefreshStateMenu()
        {
            try
            {
                //SaveState tempSave = new SaveState();
                string shortFileName;
                string[] files = Directory.GetFiles(path);
                DebugMod.instance.Log( 
                    "path var: " + path +
                    "\nSavestates: " + files.ToString());

                foreach (string file in files)
                {
                    shortFileName = Path.GetFileName(file);
                    DebugMod.instance.Log("file: " + shortFileName);
                    var digits = shortFileName.SkipWhile(c => !Char.IsDigit(c)).TakeWhile(Char.IsDigit).ToArray();
                    int slot = int.Parse(new string(digits));
                    
                    if (File.Exists(file) && (slot >= 0 || slot < maxSaveStates))
                    {
                        if (saveStateFiles.ContainsKey(slot))
                        {
                            saveStateFiles.Remove(slot);
                        }
                        saveStateFiles.Add(slot, new SaveState());
                        saveStateFiles[slot].LoadStateFromFile(slot);
                        
                        DebugMod.instance.Log(saveStateFiles[slot].GetSaveStateID());
                    }
                }
            }
            catch (Exception ex)
            {
                DebugMod.instance.Log(string.Format(ex.Source, ex.Message));
                throw ex;
            }
        }

        private void AutoSlotSelect(SaveStateType stateType)
        {
            if (autoSlot)
            {
                int i = 0;
                int initSlot = currentStateSlot;

                // saveStateFiles.Keys;
                // Refactor using dict.keys()?
                while (++currentStateSlot != initSlot && ++i < maxSaveStates && saveStateFiles.ContainsKey(currentStateSlot))
                {
                    if (currentStateSlot + 1 > maxSaveStates)
                    {
                        currentStateSlot = 0;
                    }
                }

                if (currentStateSlot == initSlot)
                {
                    // TODO: Inquire if want to overwrite
                    currentStateSlot--;
                    if (currentStateSlot < 0)
                    {
                        currentStateSlot = maxSaveStates - 1;
                    }
                    saveStateFiles.Remove(currentStateSlot);
                }

                saveStateFiles.Add(currentStateSlot, new SaveState());
                if (stateType == SaveStateType.Memory)
                {
                    if (memoryState == null || !memoryState.IsSet())
                    {
                        memoryState.SaveTempState();
                    }
                    saveStateFiles[currentStateSlot].data = memoryState.data;
                    saveStateFiles[currentStateSlot].SaveStateToFile(currentStateSlot);
                } 
                else if (stateType == SaveStateType.SkipOne)
                {
                    saveStateFiles[currentStateSlot].NewSaveStateToFile(currentStateSlot);
                }
            }
            else
            {
                GameManager.instance.StartCoroutine(SelectSlot(true, stateType));
            }
        }
        #endregion
    }
}
