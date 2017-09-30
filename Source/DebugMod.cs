using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Modding;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DebugMod
{
    public class DebugMod : Mod
    {
        private static GameManager _gm;
        private static InputHandler _ih;
        private static GameObject _refKnight;
        private static PlayMakerFSM _refKnightSlash;
        private static CameraController _refCamera;
        private static PlayMakerFSM _refDreamNail;

        private static float loadTime;
        private static float unloadTime;
        private static bool loadingChar;

        public override void Initialize()
        {
            ModHooks.ModLog("Initializing debug mod");
            ModHooks.Instance.SavegameLoadHook += LoadCharacter;
            ModHooks.Instance.BeforeSceneLoadHook += OnLevelUnload;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += LevelActivated;
            UnityEngine.GameObject UIObj = new UnityEngine.GameObject();
            UIObj.AddComponent<GUIController>();
            UnityEngine.GameObject.DontDestroyOnLoad(UIObj);

            BossHandler.PopulateBossLists();
            GUIController.instance.BuildMenus();
        }

        public void LoadCharacter(int saveId)
        {
            Console.Reset();
            EnemyController.Reset();
            DreamGate.Reset();

            loadingChar = true;
        }

        public void LevelActivated(Scene sceneFrom, Scene sceneTo)
        {
            string sceneName = sceneTo.name;

            if (loadingChar)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds((double)PlayerData.instance.playTime);
                string text = string.Format("{0:00}.{1:00}", Math.Floor(timeSpan.TotalHours), timeSpan.Minutes);
                int profileID = PlayerData.instance.profileID;
                string saveFilename = GameManager.instance.GetSaveFilename(profileID);
                DateTime lastWriteTime = File.GetLastWriteTime(Application.persistentDataPath + saveFilename);
                Console.AddLine("New savegame loaded. Profile playtime " + text + " Completion: " + PlayerData.instance.completionPercentage + " Save slot: " + profileID + " Game Version: " + PlayerData.instance.version + " Last Written: " + lastWriteTime);

                GUIController.instance.SetMenusActive(true);

                loadingChar = false;
            }

            if (gm.IsGameplayScene())
            {
                loadTime = Time.realtimeSinceStartup;
                Console.AddLine("New scene loaded: " + sceneName);
                EnemyController.Reset();
                PlayerDeathWatcher.Reset();
                BossHandler.LookForBoss(sceneName);
            }

            if (sceneName == "Menu_Title")
            {
                GUIController.instance.SetMenusActive(false);
            }
        }

        public string OnLevelUnload(string toScene)
        {
            unloadTime = Time.realtimeSinceStartup;

            return toScene;
        }

        public static string GetSceneName()
        {
            GameManager gm = GameManager.instance;

            if (gm != null)
            {
                return gm.GetSceneNameString();
            }

            return "";
        }

        public static float GetLoadTime()
        {
            return (float)Math.Round((double)(loadTime - unloadTime), 2);
        }

        public static GameManager gm
        {
            get
            {
                if (DebugMod._gm == null) DebugMod._gm = GameManager.instance;
                return DebugMod._gm;
            }
            set
            {
            }
        }
        public static InputHandler ih
        {
            get
            {
                if (DebugMod._ih == null) DebugMod._ih = DebugMod.gm.inputHandler;
                return DebugMod._ih;
            }
            set
            {
            }
        }
        public static GameObject refKnight
        {
            get
            {
                if (DebugMod._refKnight == null) DebugMod._refKnight = HeroController.instance.gameObject;
                return DebugMod._refKnight;
            }
            set
            {
            }
        }
        public static PlayMakerFSM refKnightSlash
        {
            get
            {
                if (DebugMod._refKnightSlash == null) DebugMod._refKnightSlash = DebugMod.refKnight.transform.Find("Attacks/Slash").GetComponent<PlayMakerFSM>();
                return DebugMod._refKnightSlash;
            }
            set
            {
            }
        }
        public static CameraController refCamera
        {
            get
            {
                if (DebugMod._refCamera == null) DebugMod._refCamera = GameObject.Find("tk2dCamera").GetComponent<CameraController>();
                return DebugMod._refCamera;
            }
            set
            {
            }
        }
        public static PlayMakerFSM refDreamNail
        {
            get
            {
                if (DebugMod._refDreamNail == null) DebugMod._refDreamNail = FSMUtility.LocateFSM(refKnight, "Dream Nail");
                return DebugMod._refDreamNail;
            }
            set
            {
            }
        }
    }
}
