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
        CurrentMemoryToFile
    }

    internal class SaveStateHandler : MonoBehaviour
    {
        private string saveStateIdentifier = "";
        private string saveScene;
        private PlayerData savedPd;
        private object lockArea;
        private SceneData savedSd;
        private Vector3 savePos;
        private FieldInfo cameraLockArea;

        public static int currentStateSlot = -1;
        //public static bool preserveThroughStates = false;
        public static string[] saveStateFiles = new string[10];
        //private bool exists;

        public static bool saveStateHUD = false;
        public static bool minimalInfoHUD = false;
        private static bool autoSlotSelect = true;

        internal SaveStateHandler()
        {
            
        }

        private SaveStateHandler(string _scene, string _identifier, PlayerData _pd, SceneData _sd, Vector3 _pos, FieldInfo _cameraLockArea, object _paramLockArea) {
            saveScene = _scene;
            saveStateIdentifier = _identifier;
            savedPd = _pd;
            savedSd = _sd;
            savePos = _pos;
            cameraLockArea = _cameraLockArea;
            lockArea = _paramLockArea;
        }

        #region saving
        public void SaveState(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.Memory:
                    TempSaveState();
                    break;
                case SaveStateType.File:
                    ConvertTempSaveToFile();
                    break;
                case SaveStateType.CurrentMemoryToFile:
                    ConvertTempSaveToFile();
                    break;
                default: break;
            }
        }

        private void TempSaveState()
        {
            saveScene = GameManager.instance.GetSceneNameString();
            saveStateIdentifier = "(tmp)_" + saveScene + "-" + DateTime.Now.ToString("H:mm_d-MMM");
            savedPd = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(PlayerData.instance));
            savedSd = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(SceneData.instance));
            savePos = HeroController.instance.gameObject.transform.position;
            cameraLockArea = (cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            lockArea = cameraLockArea.GetValue(GameManager.instance.cameraCtrl);
        }

        private void ConvertTempSaveToFile()
        {
            if (autoSlotSelect)
            
                switch (autoSlotSelect)
                {
                    case true:
                        int i = 0;
                        int slotAtEntry = currentStateSlot;
                        while (++currentStateSlot != slotAtEntry || i++ < 10)
                        {
                            if (currentStateSlot < 10)
                            {
                                if (String.IsNullOrEmpty(saveStateFiles[currentStateSlot]))
                                {
                                    SaveStateToFile();
                                }
                            }
                            else
                            {
                                currentStateSlot = -1;
                            }
                        }
                        break;
                    case false:
                        saveStateHUD = true;
                        break;
                    default:
                        break;
                }
        }

        private void SaveStateToFile()
        {
            try
            {
                int slotAtEntry = currentStateSlot;
                if (slotAtEntry < 0 || slotAtEntry >= 10)
                {
                    throw new Exception("No savestate slot index set, abort making file");
                }
                Directory.CreateDirectory(Application.persistentDataPath + "/Savestates-1221");


                if (saveStateIdentifier.StartsWith("(tmp)_"))
                {
                    saveStateIdentifier = saveStateIdentifier.Substring(6);
                }
                else if (String.IsNullOrEmpty(saveStateIdentifier))
                {
                    saveStateFiles[currentStateSlot] = saveScene + "-" + DateTime.Now.ToString("H:mm_d-MMM");
                }
                saveStateFiles[currentStateSlot] = saveStateIdentifier;

                File.WriteAllText(string.Concat(new object[]
                {
                    Application.persistentDataPath,
                    "/Savestates-1221/savestate",
                    currentStateSlot,
                    ".json"
                }), JsonUtility.ToJson(obj: new SaveStateHandler
                (
                    saveScene,
                    saveStateIdentifier,
                    savedPd,
                    savedSd,
                    savePos,
                    cameraLockArea,
                    lockArea
                ), prettyPrint: true));
            }
            catch (Exception ex)
            {
                Debug.Log("SaveStateToFile(): " + ex.Message);
            }
        }
        #endregion

        #region loading
        public void LoadState(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.Memory:
                    if (!String.IsNullOrEmpty(saveStateIdentifier))
                    {
                        HeroController.instance.StartCoroutine(LoadStateCoro());
                    }
                    else
                    {
                        Console.AddLine("No save state active");
                    }
                    break;
                case SaveStateType.File:
                    PrepareFileStateToMemory();
                    // Implement slot controls
                    break;
                case SaveStateType.CurrentMemoryToFile:
                    LoadStateFromFile();
                    // Implement slot controls
                    break;
                default:
                    break;
            }


            //HeroController.instance.StartCoroutine(LoadStateCoro());

        }

        private void PrepareFileStateToMemory()
        {
            try
            {

                int slot = currentStateSlot;
                SaveStateHandler tmpData = new SaveStateHandler();
                if (slot < 0 || slot >= 10)
                {
                    Console.AddLine("No slot selected, aborting prep");
                    return;
                }
                string path = string.Concat(new object[]
                {
                    Application.persistentDataPath,
                    "/Savestates-1221/savestate",
                    slot,
                    ".json"
                });

                if (File.Exists(path))
                {
                    tmpData = JsonUtility.FromJson<SaveStateHandler>(File.ReadAllText(path));
                    try
                    {
                        saveStateIdentifier = tmpData.saveStateIdentifier;
                        cameraLockArea = tmpData.cameraLockArea;
                        savedPd = tmpData.savedPd;
                        savedSd = tmpData.savedSd;
                        savePos = tmpData.savePos;
                        saveScene = tmpData.saveScene;
                        lockArea = tmpData.lockArea;
                        Console.AddLine("Load SaveState ready:" + saveStateIdentifier);
                    }
                    catch (Exception)
                    {
                        Console.AddLine("Save prep failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("SaveStateToFile() error: " + ex.Message);
            }
        }

        private void LoadStateFromFile()
        {
            PrepareFileStateToMemory();
            HeroController.instance.StartCoroutine(LoadStateCoro());
        }

        private IEnumerator LoadStateCoro()
        {
            cameraLockArea = (cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            GameManager.instance.ChangeToScene("Room_Sly_Storeroom", "", 0f);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Room_Sly_Storeroom")
            {
                yield return null;
            }
            GameManager.instance.sceneData = (SceneData.instance = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(savedSd)));
            //if (!BindableFunctions.preserveThroughStates)
            //{
            GameManager.instance.ResetSemiPersistentItems();
            //}
            yield return null;
            HeroController.instance.gameObject.transform.position = savePos;
            PlayerData.instance = (GameManager.instance.playerData = (HeroController.instance.playerData = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(savedPd))));
            GameManager.instance.ChangeToScene(saveScene, "", 0.4f);
            try
            {
                cameraLockArea.SetValue(GameManager.instance.cameraCtrl, lockArea);
                GameManager.instance.cameraCtrl.LockToArea(lockArea as CameraLockArea);
                BindableFunctions.cameraGameplayScene.SetValue(GameManager.instance.cameraCtrl, true);
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == saveScene);
            HeroController.instance.playerData = PlayerData.instance;
            if (PlayerData.instance.MPCharge >= 99)
            {
                if (PlayerData.instance.MPReserve > 0)
                {
                    HeroController.instance.TakeReserveMP(1);
                    HeroController.instance.AddMPChargeSpa(1);
                }
                HeroController.instance.TakeMP(1);
                HeroController.instance.AddMPChargeSpa(1);
            }
            else 
            {
                HeroController.instance.TakeMP(1);
                HeroController.instance.AddMPChargeSpa(1);
            }
            HeroController.instance.SetHazardRespawn(savedPd.hazardRespawnLocation, savedPd.hazardRespawnFacingRight);
            HeroController.instance.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
            HeroController.instance.geoCounter.playerData = PlayerData.instance;
            HeroController.instance.geoCounter.TakeGeo(0);
            HeroAnimationController component = HeroController.instance.GetComponent<HeroAnimationController>();
            typeof(HeroAnimationController).GetField("pd", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(component, PlayerData.instance);
            HeroController.instance.TakeHealth(1);
            HeroController.instance.AddHealth(1);
            GameCameras.instance.hudCanvas.gameObject.SetActive(true);
            HeroController.instance.TakeHealth(1);
            HeroController.instance.AddHealth(1);
            yield break;
        }
        #endregion

        #region helper functionality
        public bool IsSet()
        {
            bool isSet = false;
            if (String.IsNullOrEmpty(saveStateIdentifier))
            {
                isSet = true;
            }
            return isSet;
        }

        public void ToggleAutoSlot()
        {
            autoSlotSelect = !autoSlotSelect;
        }
        #endregion
    }
}
