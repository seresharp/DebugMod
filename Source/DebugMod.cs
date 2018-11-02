using System;
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

        internal static GameManager GM => _gm != null ? _gm : (_gm = GameManager.instance);
        internal static InputHandler IH => _ih != null ? _ih : (_ih = GM.inputHandler);
        internal static HeroController HC => _hc != null ? _hc : (_hc = GM.hero_ctrl);
        internal static GameObject RefKnight => _refKnight != null ? _refKnight : (_refKnight = HC.gameObject);
        internal static PlayMakerFSM RefKnightSlash => _refKnightSlash != null ? _refKnightSlash : (_refKnightSlash = RefKnight.transform.Find("Attacks/Slash").GetComponent<PlayMakerFSM>());
        internal static CameraController RefCamera => _refCamera != null ? _refCamera : (_refCamera = GM.cameraCtrl);
        internal static PlayMakerFSM RefDreamNail => _refDreamNail != null ? _refDreamNail : (_refDreamNail = FSMUtility.LocateFSM(RefKnight, "Dream Nail"));

        internal static DebugMod instance;
        internal static GlobalSettings settings;

        private static float _loadTime;
        private static float _unloadTime;
        private static bool _loadingChar;

        internal static bool infiniteHP;
        internal static bool infiniteSoul;
        internal static bool playerInvincible;
        internal static bool noclip;
        internal static Vector3 noclipPos;
        internal static bool cameraFollow;
        internal static bool showingFgObjs = true;

        internal static Dictionary<string, Pair> bindMethods = new Dictionary<string, Pair>();
        internal static readonly Dictionary<int, float> PreviousAlphaValues = new Dictionary<int, float>();

        public override void Initialize()
        {
            instance = this;

            instance.Log("Initializing");

            float startTime = Time.realtimeSinceStartup;
            instance.Log("Building MethodInfo dict...");

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

            instance.Log("Done! Time taken: " + (Time.realtimeSinceStartup - startTime) + "s. Found " + bindMethods.Count + " methods");

            settings = GlobalSettings;

            if (settings.FirstRun)
            {
                instance.Log("First run detected, setting default binds");

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
                settings.binds.Add("Take Screenshot", (int)KeyCode.KeypadEnter);
                settings.binds.Add("Toggle Foreground Elements", (int)KeyCode.Keypad5);
                settings.binds.Add("Show More Foreground Elements", (int)KeyCode.Keypad2);
                settings.binds.Add("Show Fewer Foreground Elements", (int)KeyCode.Keypad8);
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

            Console.AddLine("New session started " + DateTime.Now);
        }
        
        public override string GetVersion()
        {
            return "1.3.3";
        }

        public override bool IsCurrent()
        {
            try
            {
                GithubVersionHelper helper = new GithubVersionHelper("seanpr96/DebugMod");
                Log("Github = " + helper.GetVersion());
                return helper.GetVersion() == GetVersion();
            }
            catch (Exception)
            {
                return true;
            }
        }

        private void SaveSettings()
        {
            SaveGlobalSettings();
            instance.Log("Saved");
        }

        private int PlayerDamaged(int damageAmount) => infiniteHP ? 0 : damageAmount;

        private void NewCharacter() => LoadCharacter(0);

        private void LoadCharacter(int saveId)
        {
            Console.Reset();
            EnemiesPanel.Reset();
            DreamGate.Reset();

            playerInvincible = false;
            infiniteHP = false;
            infiniteSoul = false;
            noclip = false;

            _loadingChar = true;
        }

        private void LevelActivated(Scene sceneFrom, Scene sceneTo)
        {
            string sceneName = sceneTo.name;
            
            if (_loadingChar)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(PlayerData.instance.playTime);
                string text = string.Format("{0:00}.{1:00}", Math.Floor(timeSpan.TotalHours), timeSpan.Minutes);
                int profileID = PlayerData.instance.profileID;
                string saveFilename = GameManager.instance.GetSaveFilename(profileID);
                DateTime lastWriteTime = File.GetLastWriteTime(Application.persistentDataPath + saveFilename);
                Console.AddLine("New savegame loaded. Profile playtime " + text + " Completion: " + PlayerData.instance.completionPercentage + " Save slot: " + profileID + " Game Version: " + PlayerData.instance.version + " Last Written: " + lastWriteTime);

                _loadingChar = false;
            }

            if (GM.IsGameplayScene())
            {
                _loadTime = Time.realtimeSinceStartup;
                Console.AddLine("New scene loaded: " + sceneName);
                EnemiesPanel.Reset();
                PlayerDeathWatcher.Reset();
                BossHandler.LookForBoss(sceneName);
            }

            showingFgObjs = true;
            PreviousAlphaValues.Clear();
        }

        private string OnLevelUnload(string toScene)
        {
            _unloadTime = Time.realtimeSinceStartup;

            return toScene;
        }

        public static bool GrimmTroupe()
        {
            return ModHooks.Instance.version.gameVersion.minor >= 2;
        }

        public static string GetSceneName()
        {
            if (GM == null)
            {
                instance.LogWarn("GameManager reference is null in GetSceneName");
                return "";
            }

            string sceneName = GM.GetSceneNameString();
            return sceneName;
        }

        public static float GetLoadTime()
        {
            return (float)Math.Round(_loadTime - _unloadTime, 2);
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
            RefCamera.FreezeInPlace();

            HC.ResetState();

            GM.LoadScene(scenename);
        }
    }
}