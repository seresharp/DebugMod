using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalEnums;

namespace DebugMod
{
    public class DebugMod : Mod<SaveSettings, GlobalSettings>
    {
        private static GameManager _gm;
        private static InputHandler _ih;
        private static HeroController _hc;
        private static GameObject _refKnight;
        private static PlayMakerFSM _refKnightSlash;
        private static CameraController _refCamera;
        private static PlayMakerFSM _refDreamNail;

        private static float loadTime;
        private static float unloadTime;
        private static bool loadingChar;

        public static GlobalSettings settings;

        public static bool infiniteHP;
        public static bool infiniteSoul;
        public static bool playerInvincible;
        public static bool noclip = false;
        public static Vector3 noclipPos;
        public static bool levelLoading;
        public static bool cameraFollow;

        public static Dictionary<string, Pair> bindMethods = new Dictionary<string, Pair>();

        public override void Initialize()
        {
            Instance.Log("Initializing");

            float startTime = Time.realtimeSinceStartup;
            Instance.Log("Building MethodInfo dict...");

            bindMethods.Clear();
            foreach (MethodInfo method in typeof(BindableFunctions).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                object[] attributes = method.GetCustomAttributes(typeof(BindableMethod), false);

                if (attributes.Any())
                {
                    BindableMethod attr = (BindableMethod)attributes[0];
                    string name = attr.name;
                    string cat = attr.category;

                    bindMethods.Add(name, new Pair(cat, method));
                }
            }

            Instance.Log("Done! Time taken: " + (Time.realtimeSinceStartup - startTime) + "s. Found " + bindMethods.Count + " methods");

            settings = GlobalSettings;

            if (settings.FirstRun)
            {
                Instance.Log("First run detected, setting default binds");

                settings.FirstRun = false;
                settings.binds.Clear();

                settings.binds.Add("Toggle All UI", (int)KeyCode.F1);
                settings.binds.Add("Toggle Info", (int)KeyCode.F2);
                settings.binds.Add("Toggle Menu", (int)KeyCode.F3);
                settings.binds.Add("Toggle Console", (int)KeyCode.F4);
                settings.binds.Add("Force Pause", (int)KeyCode.F5);
                settings.binds.Add("Hazard Respawn", (int)KeyCode.F6);
                settings.binds.Add("Set Respawn", (int)KeyCode.F7);
                settings.binds.Add("Force Camera Follow", (int)KeyCode.F8);
                settings.binds.Add("Toggle Enemy Panel", (int)KeyCode.F9);
                settings.binds.Add("Self Damage", (int)KeyCode.F10);
                settings.binds.Add("Toggle Binds", (int)KeyCode.BackQuote);
                settings.binds.Add("Nail Damage +4", (int)KeyCode.Equals);
                settings.binds.Add("Nail Damage -4", (int)KeyCode.Minus);
                settings.binds.Add("Increase Timescale", (int)KeyCode.KeypadPlus);
                settings.binds.Add("Decrease Timescale", (int)KeyCode.KeypadMinus);
                settings.binds.Add("Toggle Hero Light", (int)KeyCode.Home);
                settings.binds.Add("Toggle Vignette", (int)KeyCode.Insert);
                settings.binds.Add("Zoom In", (int)KeyCode.PageUp);
                settings.binds.Add("Zoom Out", (int)KeyCode.PageDown);
                settings.binds.Add("Reset Camera Zoom", (int)KeyCode.End);
                settings.binds.Add("Toggle HUD", (int)KeyCode.Delete);
                settings.binds.Add("Hide Hero", (int)KeyCode.Backspace);
            }

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += LevelActivated;
            GameObject UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.Instance.SavegameLoadHook += LoadCharacter;
            ModHooks.Instance.NewGameHook += NewCharacter;
            ModHooks.Instance.BeforeSceneLoadHook += OnLevelUnload;
            ModHooks.Instance.TakeHealthHook += PlayerDamaged;
            ModHooks.Instance.ApplicationQuitHook += SaveSettings;

            BossHandler.PopulateBossLists();
            GUIController.Instance.BuildMenus();

            Console.AddLine("New session started " + DateTime.Now.ToString());
        }
        
        public override string GetVersion()
        {
            return "1.3.0";
        }

        private void SaveSettings()
        {
            SaveGlobalSettings();
            Instance.Log("Saved");
        }

        private int PlayerDamaged(int damageAmount)
        {
            if (infiniteHP)
            {
                return 0;
            }

            return damageAmount;
        }

        private void NewCharacter()
        {
            LoadCharacter(0);
        }

        private void LoadCharacter(int saveId)
        {
            Console.Reset();
            EnemiesPanel.Reset();
            DreamGate.Reset();

            playerInvincible = false;
            infiniteHP = false;
            infiniteSoul = false;
            noclip = false;

            loadingChar = true;
        }

        private void LevelActivated(Scene sceneFrom, Scene sceneTo)
        {
            levelLoading = false;

            string sceneName = sceneTo.name;
            
            if (loadingChar)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds((double)PlayerData.instance.playTime);
                string text = string.Format("{0:00}.{1:00}", Math.Floor(timeSpan.TotalHours), timeSpan.Minutes);
                int profileID = PlayerData.instance.profileID;
                string saveFilename = GameManager.instance.GetSaveFilename(profileID);
                DateTime lastWriteTime = File.GetLastWriteTime(Application.persistentDataPath + saveFilename);
                Console.AddLine("New savegame loaded. Profile playtime " + text + " Completion: " + PlayerData.instance.completionPercentage + " Save slot: " + profileID + " Game Version: " + PlayerData.instance.version + " Last Written: " + lastWriteTime);

                loadingChar = false;
            }

            if (GM.IsGameplayScene())
            {
                loadTime = Time.realtimeSinceStartup;
                Console.AddLine("New scene loaded: " + sceneName);
                EnemiesPanel.Reset();
                PlayerDeathWatcher.Reset();
                BossHandler.LookForBoss(sceneName);
            }
        }

        private string OnLevelUnload(string toScene)
        {
            levelLoading = true;

            unloadTime = Time.realtimeSinceStartup;

            return toScene;
        }

        public static bool GrimmTroupe()
        {
            return ModHooks.Instance.version.gameVersion.minor >= 2;
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

        public static void Teleport(string scenename, Vector3 pos)
        {
            HC.transform.position = pos;

            HC.EnterWithoutInput(false);
            HC.proxyFSM.SendEvent("HeroCtrl-LeavingScene");
            HC.transform.SetParent(null);

            GM.NoLongerFirstGame();
            GM.SaveLevelState();
            GM.SetState(GameState.EXITING_LEVEL);
            GM.entryGateName = "dreamGate";
            RefCamera.FreezeInPlace(false);

            HC.ResetState();

            GM.LoadScene(scenename);
        }

        public static GameManager GM
        {
            get
            {
                if (_gm == null) _gm = GameManager.instance;
                return _gm;
            }
        }
        public static InputHandler IH
        {
            get
            {
                if (_ih == null) _ih = GM.inputHandler;
                return _ih;
            }
        }
        public static HeroController HC
        {
            get
            {
                if (_hc == null) _hc = GM.hero_ctrl;
                return _hc;
            }
        }
        public static GameObject RefKnight
        {
            get
            {
                if (_refKnight == null) _refKnight = HC.gameObject;
                return _refKnight;
            }
        }
        public static PlayMakerFSM RefKnightSlash
        {
            get
            {
                if (_refKnightSlash == null) _refKnightSlash = RefKnight.transform.Find("Attacks/Slash").GetComponent<PlayMakerFSM>();
                return _refKnightSlash;
            }
        }
        public static CameraController RefCamera
        {
            get
            {
                if (_refCamera == null) _refCamera = GM.cameraCtrl;
                return _refCamera;
            }
        }
        public static PlayMakerFSM RefDreamNail
        {
            get
            {
                if (_refDreamNail == null) _refDreamNail = FSMUtility.LocateFSM(RefKnight, "Dream Nail");
                return _refDreamNail;
            }
        }
    }
}
