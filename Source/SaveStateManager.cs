using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DebugMod
{
    public enum SaveStateType
    {
        Memory,
        File,
        SkipOne
    }

    // TODO: Fix vessel count between savestates

    /// <summary>
    /// Handles organisation of SaveState-s
    /// quickState replicating legacy behaviour of only stored in RAM.
    /// Dictionary(int slot : file). Might change to HashMap(?) 
    ///  if: memory requirement too high: array for limiting savestates? hashmap as all states should logically be unique?
    /// HUD for viewing necessary info for UX.
    /// AutoSlotSelect to iterate over slots, eventually overwrite when circled and no free slots.
    /// </summary>
    internal class SaveStateManager
    {
        public static int maxSaveStates = DebugMod.settings.MaxSaveStates;
        public static int currentStateFolder = 0;
        public static SaveState quickState;
        public static bool inSelectSlotState = false;   // a mutex, in practice?
        public static int savePages;
        public static int currentStateSlot = -1;
        public static string path = Application.persistentDataPath + "/Savestates-1221/0/";
        public static string currentStateOperation = null;
        //public static bool hasFury = typeof(Modding.ReflectionHelper).GetField(, "fury");

        private static string[] stateStrings =
        {
            "Quickslot (save)",
            "Quickslot (load)",
            "Save quickslot to file",
            "Load quickslot from file",
            "Save new state to file",
            "Load new state from file"
        };
        private static Dictionary<int, SaveState> saveStateFiles = new Dictionary<int, SaveState>();

        //private static bool autoSlot;
        private DateTime timeoutHelper;
        private double timeoutAmount = 30;

        //public static bool preserveThroughStates = false;

        internal SaveStateManager()
        {
            try
            {
                inSelectSlotState = false;
                //autoSlot = false;
                DebugMod.settings.SaveStatePanelVisible = false;
                if (DebugMod.settings.SaveStatePages == -1) DebugMod.settings.SaveStatePages = 10;
                savePages = DebugMod.settings.SaveStatePages;
                quickState = new SaveState();
                for (int i = 0; i < savePages; i++)
                {
                    if (!Directory.Exists(Application.persistentDataPath + "/Savestates-1221/" + i.ToString()))
                    {
                        Directory.CreateDirectory(Application.persistentDataPath + "/Savestates-1221/" + i.ToString());
                    }
                    else
                    {
                        RefreshStateMenu();
                    }
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
                    quickState.SaveTempState();
                    break;
                case SaveStateType.File or SaveStateType.SkipOne:
                    if (!inSelectSlotState)
                    {
                        RefreshStateMenu();
                        GameManager.instance.StartCoroutine(SelectSlot(true, stateType));
                    }
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
                    if (quickState.IsSet())
                    {
                        quickState.LoadTempState();
                    }
                    else
                    {
                        Console.AddLine("No save state active");
                    }
                    break;
                case SaveStateType.File or SaveStateType.SkipOne:
                
                    if (!inSelectSlotState)
                    {
                        RefreshStateMenu();
                        GameManager.instance.StartCoroutine(SelectSlot(false, stateType));
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region helper functionality
        private IEnumerator SelectSlot(bool save, SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.Memory:
                    currentStateOperation = save ? "Quickslot (save)" : "Quickslot (load)";
                    break;
                case SaveStateType.File:
                    currentStateOperation = save ? "Quickslot save to file" : "Load file to quickslot";
                    break;
                case SaveStateType.SkipOne:
                    currentStateOperation = save ? "Save new state to file" : "Load new state from file";
                    break;
                default:
                    //DebugMod.instance.LogError("SelectSlot ended started");
                    throw new ArgumentException("Helper func SelectSlot requires `bool` and `SaveStateType` to proceed the savestate process");
            }

            if (DebugMod.settings.binds.TryGetValue(currentStateOperation, out int keycode))
            {
                DebugMod.alphaKeyDict.Add((KeyCode)keycode, keycode);
            }
            else
            {
                throw new Exception("Helper func SelectSlot could not find a binding for the `" + currentStateOperation + "` savestate process");
            }
            
            yield return null;
            timeoutHelper = DateTime.Now.AddSeconds(timeoutAmount);
            DebugMod.settings.SaveStatePanelVisible = inSelectSlotState = true;
            yield return new WaitUntil(DidInput);
            
            if (GUIController.didInput && !GUIController.inputEsc)
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
            }
            else
            {
                if (GUIController.didInput) Console.AddLine("Savestate action cancelled");
                else Console.AddLine("Timeout (" + timeoutAmount.ToString() + ")s was reached");
            }
            
            DebugMod.alphaKeyDict.Remove((KeyCode)keycode);
            currentStateOperation = null;
            GUIController.inputEsc = GUIController.didInput = false;
            DebugMod.settings.SaveStatePanelVisible = inSelectSlotState = false;
        }

        // Todo: cleanup Adds and Removes, because used to C++ :)
        private void SaveCoroHelper(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.File:
                    if (quickState == null || !quickState.IsSet())
                    {
                        quickState.SaveTempState();
                    }
                    if (saveStateFiles.ContainsKey(currentStateSlot))
                    {
                        saveStateFiles.Remove(currentStateSlot);
                    }
                    saveStateFiles.Add(currentStateSlot, new SaveState());
                    saveStateFiles[currentStateSlot].data = quickState.DeepCopy();
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
                    quickState.data = saveStateFiles[currentStateSlot].DeepCopy();
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

        /*
        public void ToggleAutoSlot()
        {
            autoSlot = !autoSlot;
        }

        public static bool GetAutoSlot()
        {
            return autoSlot;
        }
        */

        public static int GetCurrentSlot()
        {
            return currentStateSlot;
        }

        public string[] GetCurrentMemoryState()
        {
            if (quickState.IsSet())
            {
                return quickState.GetSaveStateInfo();
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
                int total = 0;
                foreach (KeyValuePair<int, SaveState> stateData in saveStateFiles)
                {
                    if (stateData.Value.IsSet()
                        && stateData.Key < DebugMod.settings.MaxSaveStates
                        && stateData.Key >= 0
                        && total < DebugMod.settings.MaxSaveStates)
                    {
                        returnData.Add(stateData.Key, stateData.Value.GetSaveStateInfo());
                        ++total;
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
                saveStateFiles.Clear();
                string shortFileName;
                string[] files = Directory.GetFiles(path);
                //DebugMod.instance.Log( 
                //    "path var: " + path +
                //    "\nSavestates: " + files.ToString()
                //    );

                foreach (string file in files)
                {
                    shortFileName = Path.GetFileName(file);
                    //DebugMod.instance.Log("file: " + shortFileName);
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

                        //DebugMod.instance.LogError(saveStateFiles[slot].GetSaveStateID());
                    }
                }
            }
            catch (Exception ex)
            {
                DebugMod.instance.LogError(string.Format(ex.Source, ex.Message));
                //throw ex;
            }
        }

        /*
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
                    if (quickState == null || !quickState.IsSet())
                    {
                        quickState.SaveTempState();
                    }
                    // May not need DeepCopy()
                    saveStateFiles[currentStateSlot].data = quickState.DeepCopy();
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
        */

        #endregion
    }
}
