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
    /// <summary>
    /// Saves and loads states from runtime data in Hollow Knight
    /// </summary>
    internal class SaveState : MonoBehaviour
    {
        private string saveStateIdentifier = "";
        private string saveScene;
        private PlayerData savedPd;
        private object lockArea;
        private SceneData savedSd;
        private Vector3 savePos;
        private FieldInfo cameraLockArea;
        private string filePath = "";
        

        internal SaveState()
        {
            
        }

        internal SaveState(SaveState saveState)
        {
            
        }

        private SaveState(string _scene, string _identifier, PlayerData _pd, SceneData _sd, Vector3 _pos, FieldInfo _cameraLockArea, object _paramLockArea) {
            saveScene = _scene;
            saveStateIdentifier = _identifier;
            savedPd = _pd;
            savedSd = _sd;
            savePos = _pos;
            cameraLockArea = _cameraLockArea;
            lockArea = _paramLockArea;
        }

        #region saving

        public void SaveTempState()
        {
            saveScene = GameManager.instance.GetSceneNameString();
            saveStateIdentifier = "(tmp)_" + saveScene + "-" + DateTime.Now.ToString("H:mm_d-MMM");
            savedPd = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(PlayerData.instance));
            savedSd = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(SceneData.instance));
            savePos = HeroController.instance.gameObject.transform.position;
            cameraLockArea = (cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            lockArea = cameraLockArea.GetValue(GameManager.instance.cameraCtrl);
        }

        public void SaveStateToFile(SaveState paramSave, int paramSlot)
        {
            try
            {
                if (saveStateIdentifier.StartsWith("(tmp)_"))
                {
                    saveStateIdentifier = saveStateIdentifier.Substring(6);
                }
                else if (String.IsNullOrEmpty(saveStateIdentifier))
                {
                    throw new Exception("No temp save state set");
                }

                filePath = SaveStateManager.path + paramSlot + ".json";
                File.WriteAllText(string.Concat(new object[]{filePath}), JsonUtility.ToJson(paramSave, prettyPrint: true));
            }
            catch (Exception ex)
            {
                Debug.Log("SaveStateToFile(): " + ex.Message);
            }
        }
        #endregion

        #region loading

        public void LoadTempState()
        {
            HeroController.instance.StartCoroutine(LoadStateCoro());
        }

        public void LoadStateFromFile()
        {
            PrepareFileStateToMemory(SaveStateManager.currentStateSlot);
            HeroController.instance.StartCoroutine(LoadStateCoro());
        }

        public void PrepareFileStateToMemory(int paramSlot)
        {
            try
            {
                SaveState tmpData = new SaveState();

                filePath = string.Concat(new object[]
                {
                    Application.persistentDataPath,
                    "/Savestates-1221/savestate",
                    paramSlot,
                    ".json"
                });

                if (File.Exists(filePath))
                {
                    tmpData = JsonUtility.FromJson<SaveState>(File.ReadAllText(filePath));
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

        private IEnumerator LoadStateCoro()
        {
            Console.AddLine("LoadStateCoro line1: " + savedPd.hazardRespawnLocation.ToString());
            cameraLockArea = (cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            GameManager.instance.ChangeToScene("Room_Sly_Storeroom", "", 0f);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Room_Sly_Storeroom")
            {
                yield return null;
            }
            GameManager.instance.sceneData = (SceneData.instance = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(savedSd)));
            //if (!BindableFunctions.preserveThroughStates)
            //{
            Console.AddLine("Before ResetSemiPersistentItems(): " + savedPd.hazardRespawnLocation.ToString());
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
            Console.AddLine("LoadStateCoro end of func: " + savedPd.hazardRespawnLocation.ToString());
            //HeroController.instance.SetHazardRespawn(savedPd.hazardRespawnLocation, savedPd.hazardRespawnFacingRight);
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
            if (!String.IsNullOrEmpty(saveStateIdentifier))
            {
                isSet = true;
            }
            return isSet;
        }

        public string GetSaveStateID()
        {
            return saveStateIdentifier;
        }

        public string[] GetSaveStateInfo()
        {
            return new string[]
            {
                filePath,
                saveScene,
                saveStateIdentifier,
                savedPd.ToString(),
                savedSd.ToString()
            };
        }
        
        #endregion
    }
}
