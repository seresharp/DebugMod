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
    /// Handles struct SaveStateData and individual SaveState operations
    /// </summary>
    internal class SaveState
    {
        [Serializable]
        public class SaveStateData
        {
            public string saveStateIdentifier;
            public string saveScene;
            public PlayerData savedPd;
            public object lockArea;
            public SceneData savedSd;
            public Vector3 savePos;
            public FieldInfo cameraLockArea;
            public string filePath;
        }

        [SerializeField]
        public SaveStateData data;

        internal SaveState()
        {
            data = new SaveStateData();
        }

        #region saving

        public void SaveTempState()
        {
            data.saveScene = GameManager.instance.GetSceneNameString();
            data.saveStateIdentifier = "(tmp)_" + data.saveScene + "-" + DateTime.Now.ToString("H:mm_d-MMM");
            data.savedPd = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(PlayerData.instance));
            data.savedSd = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(SceneData.instance));
            data.savePos = HeroController.instance.gameObject.transform.position;
            data.cameraLockArea = (data.cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            data.lockArea = data.cameraLockArea.GetValue(GameManager.instance.cameraCtrl);
        }

        public void NewSaveStateToFile(int paramSlot)
        {
            SaveTempState();
            SaveStateToFile(paramSlot);
        }

        public void SaveStateToFile(int paramSlot)
        {
            try
            {
                if (data.saveStateIdentifier.StartsWith("(tmp)_"))
                {
                    data.saveStateIdentifier = data.saveStateIdentifier.Substring(6);
                }
                else if (String.IsNullOrEmpty(data.saveStateIdentifier))
                {
                    throw new Exception("No temp save state set");
                }
                
                File.WriteAllText (
                    string.Concat(new object[] {
                        SaveStateManager.path,
                        "/savestate",
                        paramSlot,
                        ".json"
                    }),
                    JsonUtility.ToJson( data, 
                        prettyPrint: true 
                    )
                );

                
                /*
                DebugMod.instance.Log(string.Concat(new object[] {
                    "SaveStateToFile (this): \n - ", data.saveStateIdentifier,
                    "\n - ", data.saveScene,
                    "\n - ", (JsonUtility.ToJson(data.savedPd)),
                    "\n - ", (JsonUtility.ToJson(data.savedSd)),
                    "\n - ", data.savePos.ToString(),
                    "\n - ", data.cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic),
                    "\n - ", data.lockArea.ToString(), " ========= ", data.cameraLockArea.GetValue(GameManager.instance.cameraCtrl)
                }));
                DebugMod.instance.Log("SaveStateToFile (data): " + data);
                */
            }
            catch (Exception ex)
            {
                DebugMod.instance.LogDebug(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region loading

        public void LoadTempState()
        {
            HeroController.instance.StartCoroutine(LoadStateCoro());
        }

        public void NewLoadStateFromFile()
        {
            LoadStateFromFile(SaveStateManager.currentStateSlot);
            LoadTempState();
        }

        public void LoadStateFromFile(int paramSlot)
        {
            try
            {
                data.filePath = string.Concat(
                new object[]
                {
                    SaveStateManager.path,
                    "savestate",
                    paramSlot,
                    ".json"
                });
                DebugMod.instance.Log("prep filepath: " + data.filePath);

                if (File.Exists(data.filePath))
                {
                    //DebugMod.instance.Log("checked filepath: " + data.filePath);
                    SaveStateData tmpData = JsonUtility.FromJson<SaveStateData>(File.ReadAllText(data.filePath));
                    try
                    {
                        data.saveStateIdentifier = tmpData.saveStateIdentifier;
                        data.cameraLockArea = tmpData.cameraLockArea;
                        data.savedPd = tmpData.savedPd;
                        data.savedSd = tmpData.savedSd;
                        data.savePos = tmpData.savePos;
                        data.saveScene = tmpData.saveScene;
                        data.lockArea = tmpData.lockArea;
                        DebugMod.instance.Log("Load SaveState ready: " + data.saveStateIdentifier);
                    }
                    catch (Exception ex)
                    {
                        DebugMod.instance.Log(string.Format(ex.Source, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                DebugMod.instance.LogDebug(ex.Message);
                throw ex;
            }
        }

        private IEnumerator LoadStateCoro()
        {
            Console.AddLine("LoadStateCoro line1: " + data.savedPd.hazardRespawnLocation.ToString());
            data.cameraLockArea = (data.cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            GameManager.instance.ChangeToScene("Room_Sly_Storeroom", "", 0f);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Room_Sly_Storeroom")
            {
                yield return null;
            }
            GameManager.instance.sceneData = (SceneData.instance = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(data.savedSd)));
            //if (!BindableFunctions.preserveThroughStates)
            //{
            Console.AddLine("Before ResetSemiPersistentItems(): " + data.savedPd.hazardRespawnLocation.ToString());
            GameManager.instance.ResetSemiPersistentItems();
            //}
            yield return null;
            HeroController.instance.gameObject.transform.position = data.savePos;
            PlayerData.instance = (GameManager.instance.playerData = (HeroController.instance.playerData = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(data.savedPd))));
            GameManager.instance.ChangeToScene(data.saveScene, "", 0.4f);
            try
            {
                data.cameraLockArea.SetValue(GameManager.instance.cameraCtrl, data.lockArea);
                GameManager.instance.cameraCtrl.LockToArea(data.lockArea as CameraLockArea);
                BindableFunctions.cameraGameplayScene.SetValue(GameManager.instance.cameraCtrl, true);
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == data.saveScene);
            HeroController.instance.playerData = PlayerData.instance;
            HeroController.instance.geoCounter.playerData = PlayerData.instance;
            HeroController.instance.geoCounter.TakeGeo(0);
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
            //Console.AddLine("LoadStateCoro end of func: " + data.savedPd.hazardRespawnLocation.ToString());
            //HeroController.instance.SetHazardRespawn(savedPd.hazardRespawnLocation, savedPd.hazardRespawnFacingRight);
            HeroController.instance.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");
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
            if (!String.IsNullOrEmpty(data.saveStateIdentifier))
            {
                isSet = true;
            }
            return isSet;
        }

        public string GetSaveStateID()
        {
            return data.saveStateIdentifier;
        }

        public string[] GetSaveStateInfo()
        {
            return new string[]
            {
                data.saveStateIdentifier,
                data.saveScene
            };
        }
        
        #endregion
    }
}
