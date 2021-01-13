using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public enum SaveStateType {
        ToMemory,
        ToFile,
        CurrentMemoryToFile
    }

    internal class SaveStateHelper : MonoBehaviour
    {
        private static string _saveScene;
        private static PlayerData _savedPd;
        private static object _lockArea;
        private static SceneData _savedSd;
        private static Vector3 _savePos;
        private static FieldInfo cameraLockArea;
        public static int currentStateSlot = -1;
        public static bool preserveThroughStates = false;
        public static string[] saveStateFiles = new string[10];
        private static string _saveStateIdentifier = "";
        public static bool saveStateHUD = false;
        public static bool minimalInfoHUD = false;
        private static bool autoSlotSelect = true;

        #region saving
        public void SaveState(SaveStateType stateType)
        {
            switch (stateType)
            {
                case SaveStateType.ToMemory:
                    TempSaveState();
                    break;
                case SaveStateType.ToFile:
                    ConvertTempToFile();
                    break;
                case SaveStateType.CurrentMemoryToFile:
                    ConvertTempToFile();
                    break;
                default: break;
            }
        }

        private void TempSaveState()
        {
            _saveScene = GameManager.instance.GetSceneNameString();
            _saveStateIdentifier = "(tmp)_" + _saveScene + "-" + DateTime.Now.ToString("H:mm_d-MMM");
            _savedPd = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(PlayerData.instance));
            _savedSd = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(SceneData.instance));
            _savePos = HeroController.instance.gameObject.transform.position;
            cameraLockArea = (cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            _lockArea = cameraLockArea.GetValue(GameManager.instance.cameraCtrl);
        }

        private void ConvertTempToFile()
        {

        }

        private void SaveStateToFile()
        {

        }
        #endregion

        #region loading
        public void LoadState()
        {
            HeroController.instance.StartCoroutine(LoadStateCoro());
        }

        private void PrepareFileStateToMemory()
        {

        }

        private void LoadStateFromFile()
        {

        }

        private IEnumerator LoadStateCoro()
        {
            cameraLockArea = (cameraLockArea ?? typeof(CameraController).GetField("currentLockArea", BindingFlags.Instance | BindingFlags.NonPublic));
            GameManager.instance.ChangeToScene("Room_Sly_Storeroom", "", 0f);
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Room_Sly_Storeroom")
            {
                yield return null;
            }
            GameManager.instance.sceneData = (SceneData.instance = JsonUtility.FromJson<SceneData>(JsonUtility.ToJson(_savedSd)));
            //if (!BindableFunctions.preserveThroughStates)
            //{
            GameManager.instance.ResetSemiPersistentItems();
            //}
            yield return null;
            HeroController.instance.gameObject.transform.position = _savePos;
            PlayerData.instance = (GameManager.instance.playerData = (HeroController.instance.playerData = JsonUtility.FromJson<PlayerData>(JsonUtility.ToJson(_savedPd))));
            GameManager.instance.ChangeToScene(_saveScene, "", 0.4f);
            try
            {
                cameraLockArea.SetValue(GameManager.instance.cameraCtrl, _lockArea);
                GameManager.instance.cameraCtrl.LockToArea(_lockArea as CameraLockArea);
                BindableFunctions.cameraGameplayScene.SetValue(GameManager.instance.cameraCtrl, true);
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == _saveScene);
            HeroController.instance.playerData = PlayerData.instance;
            if (PlayerData.instance.MPCharge >= 99)
            {
                HeroController.instance.AddMPChargeSpa(1);
                HeroController.instance.TakeMP(1);
            }
            else if (PlayerData.instance.maxMP <= 99)
            {
                HeroController.instance.TakeMP(1);
                HeroController.instance.AddMPChargeSpa(1);
            }
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

        private void LoadFromFile()
        {

        }
        #endregion

        #region helper functionality
        public bool IsSet()
        {
            bool isSet = false;
            if (String.IsNullOrEmpty(_saveStateIdentifier))
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
