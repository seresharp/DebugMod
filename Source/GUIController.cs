
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;
using GlobalEnums;
using InControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Modding;

namespace DebugMod 
{
    public class GUIController : MonoBehaviour
    {
        public Font trajanBold;
        public Font trajanNormal;
        public Font arial;
        public Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();
        public Vector3 hazardLocation;
        public Vector3 manualRespawn;
        public bool cameraFollow;
        public string respawnSceneWatch;

        private GameObject canvas;
        private static GUIController _instance;

        public void Awake()
        {
            hazardLocation = PlayerData.instance.hazardRespawnLocation;
            respawnSceneWatch = PlayerData.instance.respawnScene;
        }

        public void BuildMenus()
        {
            LoadResources();

            canvas = new GameObject();
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvas.AddComponent<GraphicRaycaster>();

            InfoPanel.BuildMenu(canvas);
            TopMenu.BuildMenu(canvas);
            EnemiesPanel.BuildMenu(canvas);
            Console.BuildMenu(canvas);
            HelpPanel.BuildMenus(canvas);

            SetMenusActive(false);

            GameObject.DontDestroyOnLoad(canvas);
        }

        private void InputClicked(string input)
        {
            Modding.ModHooks.ModLog(input);
        }

        public void SetMenusActive(bool active)
        {
            TopMenu.visible = active;
            InfoPanel.visible = active;
            EnemiesPanel.visible = active;
            Console.visible = active;
            HelpPanel.visible = active;
        }

        private void LoadResources()
        {
            foreach (Font f in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (f != null && f.name == "TrajanPro-Bold")
                {
                    trajanBold = f;
                }

                if (f != null && f.name == "TrajanPro-Regular")
                {
                    trajanNormal = f;
                }

                //Just in case for some reason the computer doesn't have arial
                if (f != null && f.name == "Perpetua")
                {
                    arial = f;
                }

                foreach (string font in Font.GetOSInstalledFontNames())
                {
                    if (font.ToLower().Contains("arial"))
                    {
                        arial = Font.CreateDynamicFontFromOSFont(font, 13);
                        break;
                    }
                }
            }

            if (trajanBold == null || trajanNormal == null || arial == null) ModHooks.ModLog("[DEBUG MOD] Could not find game fonts");

            if (Directory.Exists("DebugMod"))
            {
                foreach (string fileName in Directory.GetFiles("DebugMod"))
                {
                    string extension = "";
                    if (fileName.Contains('.'))
                    {
                        extension = fileName.Substring(fileName.LastIndexOf('.'));
                    }

                    if (extension == ".png" || extension == ".jpg")
                    {
                        try
                        {
                            Texture2D tex = new Texture2D(1, 1);
                            tex.LoadImage(File.ReadAllBytes(fileName));

                            string[] split = fileName.Split(new char[] { '/', '\\' });
                            string internalName = split[split.Length - 1].Substring(0, split[split.Length - 1].LastIndexOf('.'));
                            images.Add(internalName, tex);

                            ModHooks.ModLog("[DEBUG MOD] Loaded image: " + internalName);
                        }
                        catch (Exception e)
                        {
                            ModHooks.ModLog("[DEBUG MOD] Failed to load image: " + fileName + "\n" + e.ToString());
                        }
                    }
                    else
                    {
                        ModHooks.ModLog("[DEBUG MOD] Non-image file in asset folder: " + fileName);
                    }
                }
            }
            else
            {
                ModHooks.ModLog("[DEBUG MOD] Could not find asset folder");
            }
        }

        public void Respawn()
        {
            if (GameManager.instance.IsGameplayScene() && !HeroController.instance.cState.dead && PlayerData.instance.health > 0)
            {
                if (UIManager.instance.uiState.ToString() == "PAUSED")
                {
                    UIManager.instance.TogglePauseGame();
                    GameManager.instance.HazardRespawn();
                    Console.AddLine("Closing Pause Menu and respawning...");
                    return;
                }
                if (UIManager.instance.uiState.ToString() == "PLAYING")
                {
                    HeroController.instance.RelinquishControl();
                    GameManager.instance.HazardRespawn();
                    HeroController.instance.RegainControl();
                    Console.AddLine("Respawn signal sent");
                    return;
                }
                Console.AddLine("Respawn requested in some weird conditions, abort, ABORT");
            }
        }

        public void Update()
        {
            InfoPanel.Update();
            TopMenu.Update();
            EnemiesPanel.Update();
            Console.Update();
            HelpPanel.Update();

            if (DebugMod.GetSceneName() != "Menu_Title")
            {
                if (DebugMod.infiniteSoul && PlayerData.instance.MPCharge < 100 && PlayerData.instance.health > 0 && !HeroController.instance.cState.dead && GameManager.instance.IsGameplayScene())
                {
                    PlayerData.instance.MPCharge = 100;
                }

                /*if (DebugMod.infiniteHP && !HeroController.instance.cState.dead && GameManager.instance.IsGameplayScene() && PlayerData.instance.health < PlayerData.instance.maxHealth)
                {
                    int amount = PlayerData.instance.maxHealth - PlayerData.instance.health;
                    PlayerData.instance.health = PlayerData.instance.maxHealth;
                    HeroController.instance.AddHealth(amount);
                }*/

                if (DebugMod.playerInvincible && PlayerData.instance != null)
                {
                    PlayerData.instance.isInvincible = true;
                }

                if (DebugMod.noclip)
                {
                    if (DebugMod.ih.inputActions.left.IsPressed)
                    {
                        DebugMod.noclipPos = new Vector3(DebugMod.noclipPos.x - Time.deltaTime * 20f, DebugMod.noclipPos.y, DebugMod.noclipPos.z);
                    }

                    if (DebugMod.ih.inputActions.right.IsPressed)
                    {
                        DebugMod.noclipPos = new Vector3(DebugMod.noclipPos.x + Time.deltaTime * 20f, DebugMod.noclipPos.y, DebugMod.noclipPos.z);
                    }

                    if (DebugMod.ih.inputActions.up.IsPressed)
                    {
                        DebugMod.noclipPos = new Vector3(DebugMod.noclipPos.x, DebugMod.noclipPos.y + Time.deltaTime * 20f, DebugMod.noclipPos.z);
                    }

                    if (DebugMod.ih.inputActions.down.IsPressed)
                    {
                        DebugMod.noclipPos = new Vector3(DebugMod.noclipPos.x, DebugMod.noclipPos.y - Time.deltaTime * 20f, DebugMod.noclipPos.z);
                    }

                    if (HeroController.instance.transitionState.ToString() == "WAITING_TO_TRANSITION")
                    {
                        DebugMod.refKnight.transform.position = DebugMod.noclipPos;
                    }
                    else
                    {
                        DebugMod.noclipPos = DebugMod.refKnight.transform.position;
                    }
                }

                if (Input.GetKeyUp(KeyCode.Escape) && DebugMod.gm.IsGamePaused())
                {
                    UIManager.instance.TogglePauseGame();
                }


                if (Input.GetKeyUp(KeyCode.Equals))
                {
                    int num = 4;
                    if (PlayerData.instance.nailDamage == 0)
                    {
                        num = 5;
                    }
                    PlayerData.instance.nailDamage = PlayerData.instance.nailDamage + num;
                    PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
                    Console.AddLine("Increased base nailDamage by " + num.ToString());
                }
                if (Input.GetKeyUp(KeyCode.Minus))
                {
                    int nailDamage = PlayerData.instance.nailDamage;
                    int num2 = PlayerData.instance.nailDamage - 4;
                    if (num2 >= 0)
                    {
                        PlayerData.instance.nailDamage = num2;
                        PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
                        Console.AddLine("Decreased base nailDamage by 4");
                    }
                    else
                    {
                        Console.AddLine("Cannot set base nailDamage less than 0 therefore forcing 0 value");
                        PlayerData.instance.nailDamage = 0;
                        PlayMakerFSM.BroadcastEvent("UPDATE NAIL DAMAGE");
                    }
                }
                if (Input.GetKeyUp(KeyCode.BackQuote))
                {
                    HelpPanel.visible = !HelpPanel.visible;
                }
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    SetMenusActive(!(HelpPanel.visible || InfoPanel.visible || EnemiesPanel.visible || TopMenu.visible || Console.visible));

                    if (EnemiesPanel.visible)
                    {
                        EnemiesPanel.RefreshEnemyList();
                    }
                }
                if (Input.GetKeyUp(KeyCode.F2))
                {
                    InfoPanel.visible = !InfoPanel.visible;
                }
                if (Input.GetKeyUp(KeyCode.F3))
                {
                    TopMenu.visible = !TopMenu.visible;
                }
                if (Input.GetKeyUp(KeyCode.F4))
                {
                    Console.visible = !Console.visible;
                }
                if (Input.GetKeyUp(KeyCode.F5))
                {
                    try
                    {
                        FieldInfo timeSlowed = typeof(GameManager).GetField("timeSlowed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                        FieldInfo ignoreUnpause = typeof(UIManager).GetField("ignoreUnpause", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

                        if ((PlayerData.instance.disablePause || (bool)timeSlowed.GetValue(GameManager.instance) || (bool)ignoreUnpause.GetValue(UIManager.instance)) && DebugMod.GetSceneName() != "Menu_Title" && DebugMod.gm.IsGameplayScene())
                        {
                            timeSlowed.SetValue(GameManager.instance, false);
                            ignoreUnpause.SetValue(UIManager.instance, false);
                            PlayerData.instance.disablePause = false;
                            UIManager.instance.TogglePauseGame();
                            Console.AddLine("Forcing Pause Menu because pause is disabled");
                        }
                        else
                        {
                            Console.AddLine("Game does not report that Pause is disabled, requesting it normally.");
                            UIManager.instance.TogglePauseGame();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.AddLine(e.ToString());
                    }
                }
                if (Input.GetKeyUp(KeyCode.F6))
                {
                    Respawn();
                }
                if (Input.GetKeyUp(KeyCode.F7))
                {
                    manualRespawn = DebugMod.refKnight.transform.position;
                    HeroController.instance.SetHazardRespawn(manualRespawn, false);
                    Console.AddLine("Manual respawn point on this map set to" + manualRespawn.ToString());
                }
                if (Input.GetKeyUp(KeyCode.F8))
                {
                    string text = DebugMod.refCamera.mode.ToString();
                    if (!cameraFollow && text != "FOLLOWING")
                    {
                        Console.AddLine("Setting Camera Mode to FOLLOW. Previous mode: " + text);
                        cameraFollow = true;
                    }
                    else if (cameraFollow)
                    {
                        cameraFollow = false;
                        Console.AddLine("Camera Mode is no longer forced");
                    }
                }
                if (Input.GetKeyUp(KeyCode.F9))
                {
                    EnemiesPanel.visible = !EnemiesPanel.visible;
                    if (EnemiesPanel.visible)
                    {
                        EnemiesPanel.RefreshEnemyList();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Insert))
                {
                    HeroController.instance.vignette.enabled = !HeroController.instance.vignette.enabled;
                }
                if (Input.GetKeyUp(KeyCode.Home))
                {
                    GameObject gameObject = DebugMod.refKnight.transform.Find("HeroLight").gameObject;
                    Color color = gameObject.GetComponent<SpriteRenderer>().color;
                    if (color.a != 0f)
                    {
                        color.a = 0f;
                        gameObject.GetComponent<SpriteRenderer>().color = color;
                        Console.AddLine("Rendering HeroLight invisible...");
                    }
                    else
                    {
                        color.a = 0.7f;
                        gameObject.GetComponent<SpriteRenderer>().color = color;
                        Console.AddLine("Rendering HeroLight visible...");
                    }
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.Home))
                {
                    DebugMod.refKnight.transform.Find("HeroLight").gameObject.SetActive(false);
                    Console.AddLine("Object HeroLight DISABLED until reload!");
                }
                if (Input.GetKeyUp(KeyCode.Delete))
                {
                    if (GameCameras.instance.hudCanvas.gameObject.activeInHierarchy)
                    {
                        GameCameras.instance.hudCanvas.gameObject.SetActive(false);
                        Console.AddLine("Disabling HUD...");
                    }
                    else
                    {
                        GameCameras.instance.hudCanvas.gameObject.SetActive(true);
                        Console.AddLine("Enabling HUD...");
                    }
                }
                if (Input.GetKeyUp(KeyCode.End))
                {
                    GameCameras.instance.tk2dCam.ZoomFactor = 1f;
                    Console.AddLine("Zoom factor was reset");
                }
                if (Input.GetKeyUp(KeyCode.PageUp))
                {
                    float zoomFactor = GameCameras.instance.tk2dCam.ZoomFactor;
                    GameCameras.instance.tk2dCam.ZoomFactor = zoomFactor + zoomFactor * 0.05f;
                    Console.AddLine("Zoom level increased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
                }
                if (Input.GetKeyUp(KeyCode.PageDown))
                {
                    float zoomFactor2 = GameCameras.instance.tk2dCam.ZoomFactor;
                    GameCameras.instance.tk2dCam.ZoomFactor = zoomFactor2 - zoomFactor2 * 0.05f;
                    Console.AddLine("Zoom level decreased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.PageUp))
                {
                    float zoomFactor3 = GameCameras.instance.tk2dCam.ZoomFactor;
                    GameCameras.instance.tk2dCam.ZoomFactor = zoomFactor3 + zoomFactor3 * 0.2f;
                    Console.AddLine("Zoom level increased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.PageDown))
                {
                    float zoomFactor4 = GameCameras.instance.tk2dCam.ZoomFactor;
                    GameCameras.instance.tk2dCam.ZoomFactor = zoomFactor4 - zoomFactor4 * 0.2f;
                    Console.AddLine("Zoom level decreased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.PageUp))
                {
                    GameCameras.instance.tk2dCam.ZoomFactor = GameCameras.instance.tk2dCam.ZoomFactor + 0.05f;
                    Console.AddLine("Zoom level increased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.PageDown))
                {
                    GameCameras.instance.tk2dCam.ZoomFactor = GameCameras.instance.tk2dCam.ZoomFactor - 0.05f;
                    Console.AddLine("Zoom level decreased to: " + GameCameras.instance.tk2dCam.ZoomFactor);
                }
                if (Input.GetKeyUp(KeyCode.Backspace))
                {
                    tk2dSprite component = DebugMod.refKnight.GetComponent<tk2dSprite>();
                    Color color2 = component.color;
                    if (color2.a != 0f)
                    {
                        color2.a = 0f;
                        component.color = color2;
                        Console.AddLine("Rendering Hero sprite invisible...");
                    }
                    else
                    {
                        color2.a = 1f;
                        component.color = color2;
                        Console.AddLine("Rendering Hero sprite visible...");
                    }
                }
                if (Input.GetKeyUp(KeyCode.KeypadMinus))
                {
                    float timeScale = Time.timeScale;
                    float num3 = timeScale - 0.1f;
                    if (num3 > 0f)
                    {
                        Time.timeScale = num3;
                        Console.AddLine(string.Concat(new object[]
                    {
                    "New TimeScale value: ",
                    num3,
                    " Old value: ",
                    timeScale
                    }));
                    }
                    else
                    {
                        Console.AddLine("Cannot set TimeScale equal or lower than 0");
                    }
                }
                if (Input.GetKeyUp(KeyCode.KeypadPlus))
                {
                    float timeScale2 = Time.timeScale;
                    float num4 = timeScale2 + 0.1f;
                    if (num4 < 2f)
                    {
                        Time.timeScale = num4;
                        Console.AddLine(string.Concat(new object[]
                    {
                    "New TimeScale value: ",
                    num4,
                    " Old value: ",
                    timeScale2
                    }));
                    }
                    else
                    {
                        Console.AddLine("Cannot set TimeScale greater than 2.0");
                    }
                }
                if (cameraFollow && DebugMod.refCamera.mode != CameraController.CameraMode.FOLLOWING)
                {
                    DebugMod.refCamera.SetMode(CameraController.CameraMode.FOLLOWING);
                }

                if (PlayerDeathWatcher.PlayerDied())
                {
                    PlayerDeathWatcher.LogDeathDetails();
                }

                if (PlayerData.instance.hazardRespawnLocation != hazardLocation)
                {
                    hazardLocation = PlayerData.instance.hazardRespawnLocation;
                    Console.AddLine("Hazard Respawn location updated: " + hazardLocation.ToString());

                    if (EnemiesPanel.visible)
                    {
                        EnemiesPanel.enemyUpdate(200f);
                    }
                }
                if (!string.IsNullOrEmpty(respawnSceneWatch) && respawnSceneWatch != PlayerData.instance.respawnScene)
                {
                    respawnSceneWatch = PlayerData.instance.respawnScene;
                    Console.AddLine(string.Concat(new string[]
                    {
                        "Save Respawn updated, new scene: ",
                        PlayerData.instance.respawnScene.ToString(),
                        ", Map Zone: ",
                        GameManager.instance.GetCurrentMapZone(),
                        ", Respawn Marker: ",
                        PlayerData.instance.respawnMarkerName.ToString()
                    }));
                }
            }
        }

        public static GUIController instance
        {
            get
            {
                if (GUIController._instance == null)
                {
                    GUIController._instance = UnityEngine.Object.FindObjectOfType<GUIController>();
                    if (GUIController._instance == null)
                    {
                        Modding.ModHooks.ModLog("[DEBUG MOD] Couldn't find GUIController");
                        GUIController._instance = null;
                    }
                }
                return GUIController._instance;
            }
            set
            {
            }
        }
    }
}