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
            this.data = new SaveStateData();
        }

        private SaveState(string _scene, string _identifier, PlayerData _pd, SceneData _sd, Vector3 _pos, FieldInfo _cameraLockArea, object _paramLockArea) {
            data.saveScene = _scene;
            data.saveStateIdentifier = _identifier;
            data.savedPd = _pd;
            data.savedSd = _sd;
            data.savePos = _pos;
            data.cameraLockArea = _cameraLockArea;
            data.lockArea = _paramLockArea;
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

                //filePath = SaveStateManager.path + paramSlot + ".json";

                data.filePath = string.Concat(new object[] {
                    Application.persistentDataPath,
                    "/Savestates-1221/savestate",
                    paramSlot,
                    ".json"
                });
                
                File.WriteAllText(
                    data.filePath,
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
            catch (Exception)
            {
                throw;
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
            LoadTempState();
        }

        public void PrepareFileStateToMemory(int paramSlot)
        {
            try
            {
                SaveStateData tmpData = new SaveStateData();
                
                data.filePath = string.Concat(new object[]
                {
                    Application.persistentDataPath,
                    "/Savestates-1221/savestate",
                    paramSlot,
                    ".json"
                });

                if (File.Exists(data.filePath))
                {
                    tmpData = JsonUtility.FromJson<SaveStateData>(File.ReadAllText(data.filePath));
                    try
                    {
                        data.saveStateIdentifier = tmpData.saveStateIdentifier;
                        data.cameraLockArea = tmpData.cameraLockArea;
                        data.savedPd = tmpData.savedPd;
                        data.savedSd = tmpData.savedSd;
                        data.savePos = tmpData.savePos;
                        data.saveScene = tmpData.saveScene;
                        data.lockArea = tmpData.lockArea;
                        Console.AddLine("Load SaveState ready:" + data.saveStateIdentifier);
                    }
                    catch (Exception)
                    {
                        Console.AddLine("Save prep failed");
                    }
                }
            }
            catch (Exception)
            {
                throw;
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
            Console.AddLine("LoadStateCoro end of func: " + data.savedPd.hazardRespawnLocation.ToString());
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
                data.filePath,
                data.saveScene,
                data.saveStateIdentifier
            };
        }
        
        #endregion
    }
}
