
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private Font font;
        private GameObject canvas;
        private TopMenu topMenu;

        public Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

        public void Awake()
        {
            if (this.backgroundTexture == null)
            {
                this.backgroundTexture = new Texture2D(1, 1);
                this.backgroundTexture.SetPixel(0, 0, Color.white);
                this.backgroundTexture.Apply();
            }

            if (this.textureStyle == null)
            {
                this.textureStyle = new GUIStyle();
                this.textureStyle.normal.background = this.backgroundTexture;
            }
            
            this.modVersion = "1.2.0.2";
            this.showMenus = true;
            this.showHelp = true;
            this.showPanelState = 6;
            this.hazardLocation = PlayerData.instance.hazardRespawnLocation;
            this.respawnSceneWatch = PlayerData.instance.respawnScene;
            this.setMatrix();
        }

        public void Start()
        {
            GUI.depth = 2;
        }

        public void OnEnable()
        {
            GUI.depth = 2;
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

            topMenu = new TopMenu(canvas, font);

            GameObject.DontDestroyOnLoad(canvas);
        }

        public void SetMenusActive(bool active)
        {
            if (topMenu != null)
            {
                topMenu.SetActive(active);
            }
        }

        private void LoadResources()
        {
            foreach (Font f in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (f != null && f.name == "TrajanPro-Bold")
                {
                    font = f;
                    break;
                }
            }

            if (font == null) ModHooks.ModLog("[DEBUG MOD] Could not find game font");

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

        public void OnGUI()
        {
            Matrix4x4 matrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, this.scale);
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;
            GUI.color = Color.white;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            if (GameManager.instance.sceneName == "Menu_Title")
            {
                GUI.skin.label.fontStyle = FontStyle.Normal;
                GUI.skin.label.fontSize = 14;
                GUI.Label(new Rect(10f, 0f, 400f, 300f), "DebugMod\nby KZ | The Embraced One");
            }
            if (this.showMenus)
            {
                GUI.skin.button.fontStyle = FontStyle.Bold;
                if ((this.showPanel && GameManager.instance.IsGameplayScene() && UIManager.instance.uiState == UIState.PLAYING) || (GameManager.instance.IsGameplayScene() && !this.showHelp && UIManager.instance.uiState == UIState.PAUSED))
                {
                    GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.skin.label.fontSize = 18;

                    if (this.showPanelState >= 1)
                    {
                        int num = Mathf.FloorToInt(Time.realtimeSinceStartup / 60f);
                        int num2 = Mathf.FloorToInt(Time.realtimeSinceStartup - (float)(num * 60));
                        Vector3 position = DebugMod.refKnight.transform.position;
                        GUI.Label(new Rect(10f, 10f, 200f, 25f), "Session  time: " + string.Format("{0:00}:{1:00}", num, num2));
                        GUI.Label(new Rect(10f, 35f, 300f, 25f), "Hero position: " + position.ToString());
                        GUI.Label(new Rect(1720f, 10f, 300f, 120f), this.ReadInput());
                        GUI.Label(new Rect(910f, 5f, 100f, 25f), "Load: " + DebugMod.GetLoadTime() + "s");
                    }

                    GUILayout.BeginArea(new Rect(7f, 210f, 500f, 790f));
                    GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                    if (this.showPanelState >= 0 && this.showPanelState < 2)
                    {
                        GUILayout.Label("CTRL+F2 TO CYCLE BETWEEN TABS", new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    if (this.showPanelState >= 2)
                    {
                        ActorStates hero_state = HeroController.instance.hero_state;
                        int health = PlayerData.instance.health;
                        int mpcharge = PlayerData.instance.MPCharge;
                        int maxHealth = PlayerData.instance.maxHealth;
                        int maxHealthBase = PlayerData.instance.maxHealthBase;
                        int nailDamage = PlayerData.instance.nailDamage;
                        bool acceptingInput = HeroController.instance.acceptingInput;
                        Vector2 current_velocity = HeroController.instance.current_velocity;
                        bool controlReqlinquished = HeroController.instance.controlReqlinquished;
                        bool atBench = PlayerData.instance.atBench;
                        int value = DebugMod.refKnightSlash.FsmVariables.GetFsmInt("damageDealt").Value;
                        float value2 = DebugMod.refKnightSlash.FsmVariables.GetFsmFloat("Multiplier").Value;
                        GUILayout.Label(string.Concat(new object[]
                        {
                            "HERO STATE: ",
                            hero_state,
                            Environment.NewLine,
                            "HP: ",
                            health,
                            "  |  MP: ",
                            mpcharge,
                            "  |  Max HP: ",
                            maxHealth,
                            "  |  MAXBASE: ",
                            maxHealthBase,
                            Environment.NewLine,
                            "NAILDMG: ",
                            nailDamage,
                            "  |  OUTPUTFLAT : ",
                            value,
                            "  |  xMULT. : ",
                            value2,
                            Environment.NewLine,
                            "VELOCITY: ",
                            current_velocity,
                            Environment.NewLine,
                            "ACCEPT INPUT: ",
                            acceptingInput,
                            Environment.NewLine,
                            "RELINQUISHED: ",
                            controlReqlinquished,
                            "  |  atBENCH: ",
                            atBench
                        }), new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    if (this.showPanelState >= 3)
                    {
                        bool dashing = HeroController.instance.cState.dashing;
                        bool jumping = HeroController.instance.cState.jumping;
                        bool doubleJumping = HeroController.instance.cState.doubleJumping;
                        bool superDashing = HeroController.instance.cState.superDashing;
                        bool onGround = HeroController.instance.cState.onGround;
                        bool falling = HeroController.instance.cState.falling;
                        bool willHardLand = HeroController.instance.cState.willHardLand;
                        bool swimming = HeroController.instance.cState.swimming;
                        bool recoiling = HeroController.instance.cState.recoiling;
                        GUILayout.Label(string.Concat(new object[]
                        {
                            "DASHING: ",
                            dashing,
                            Environment.NewLine,
                            "JUMPING: ",
                            jumping,
                            Environment.NewLine,
                            "DOUBLE-JUMPING: ",
                            doubleJumping,
                            Environment.NewLine,
                            "SUPERDASHING: ",
                            superDashing,
                            Environment.NewLine,
                            "ON GROUND: ",
                            onGround,
                            Environment.NewLine,
                            "FALLING: ",
                            falling,
                            "  |   HARDLAND: ",
                            willHardLand,
                            Environment.NewLine,
                            "SWIMMING: ",
                            swimming,
                            Environment.NewLine,
                            "RECOILING: ",
                            recoiling
                        }), new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    if (this.showPanelState >= 4)
                    {
                        bool touchingWall = HeroController.instance.cState.touchingWall;
                        bool wallLocked = HeroController.instance.wallLocked;
                        bool wallJumping = HeroController.instance.cState.wallJumping;
                        bool wallSliding = HeroController.instance.cState.wallSliding;
                        bool touchingWallL = HeroController.instance.touchingWallL;
                        bool touchingWallR = HeroController.instance.touchingWallR;
                        bool attacking = HeroController.instance.cState.attacking;
                        bool flag = HeroController.instance.CanCast();
                        bool flag2 = HeroController.instance.CanSuperDash();
                        bool flag3 = HeroController.instance.CanOpenInventory();
                        bool flag4 = HeroController.instance.CanQuickMap();
                        bool value3 = DebugMod.refDreamNail.FsmVariables.GetFsmBool("Dream Warp Allowed").Value;
                        bool value4 = DebugMod.refDreamNail.FsmVariables.GetFsmBool("Can Dream Gate").Value;
                        bool value5 = DebugMod.refDreamNail.FsmVariables.GetFsmBool("Dream Gate Allowed").Value;
                        GUILayout.Label(string.Concat(new object[]
                        {
                            "WALL-LOCK: ",
                            wallLocked,
                            "  |  WALL-JUMPING: ",
                            wallJumping,
                            Environment.NewLine,
                            "WALL TOUCHING: ",
                            touchingWall,
                            Environment.NewLine,
                            "WALL SLIDING: ",
                            wallSliding,
                            Environment.NewLine,
                            "WALL_L: ",
                            touchingWallL,
                            "  |  WALL_R: ",
                            touchingWallR,
                            Environment.NewLine,
                            "ATTACKING: ",
                            attacking,
                            Environment.NewLine,
                            "canCAST: ",
                            flag,
                            "  |  canSUPERDASH: ",
                            flag2,
                            Environment.NewLine,
                            "canQUICKMAP: ",
                            flag3,
                            "  |  canINVENTR: ",
                            flag4,
                            Environment.NewLine,
                            "canWARP: ",
                            value3,
                            "  |  canGATE: ",
                            value4,
                            "  |  gateAllow: ",
                            value5
                        }), new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    if (this.showPanelState >= 5)
                    {
                        bool invulnerable = HeroController.instance.cState.invulnerable;
                        bool isInvincible = PlayerData.instance.isInvincible;
                        bool invinciTest = PlayerData.instance.invinciTest;
                        DamageMode damageMode = HeroController.instance.damageMode;
                        bool dead = HeroController.instance.cState.dead;
                        bool hazardDeath = HeroController.instance.cState.hazardDeath;
                        GUILayout.Label(string.Concat(new object[]
                        {
                            "isINVULNERABLE: ",
                            invulnerable,
                            Environment.NewLine,
                            "INVINCIBLE: ",
                            isInvincible,
                            "  |  INVINCITEST: ",
                            invinciTest,
                            Environment.NewLine,
                            "DAMAGE STATE: ",
                            damageMode,
                            Environment.NewLine,
                            "DEAD STATE: ",
                            dead,
                            Environment.NewLine,
                            "HAZARD DEATH: ",
                            hazardDeath
                        }), new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    if (this.showPanelState >= 6 && !BossHandler.bossSub)
                    {
                        string sceneName = DebugMod.GetSceneName();
                        bool flag5 = DebugMod.gm.IsGameplayScene();
                        bool transitioning = HeroController.instance.cState.transitioning;
                        HeroTransitionState transitionState = HeroController.instance.transitionState;
                        bool isPaused = HeroController.instance.cState.isPaused;
                        float completionPercentage = PlayerData.instance.completionPercentage;
                        int grubsCollected = PlayerData.instance.grubsCollected;
                        GameState gameState = GameManager.instance.gameState;
                        UIState uiState = UIManager.instance.uiState;
                        CameraController.CameraMode mode = DebugMod.refCamera.mode;
                        GUILayout.Label(string.Concat(new object[]
                        {
                            "SCENE NAME: ",
                            sceneName,
                            Environment.NewLine,
                            "TRANSITION: ",
                            transitioning,
                            "  |  isGAMEPLAY: ",
                            flag5,
                            Environment.NewLine,
                            "TRANSITION STATE: ",
                            transitionState,
                            Environment.NewLine,
                            "GAME STATE: ",
                            gameState,
                            Environment.NewLine,
                            "UI STATE: ",
                            uiState,
                            "  |  HERO PAUSED: ",
                            isPaused,
                            Environment.NewLine,
                            "CAMERA MODE: ",
                            mode,
                            Environment.NewLine,
                            "PERCENTAGE: ",
                            completionPercentage,
                            "  |  GRUBS: ",
                            grubsCollected
                        }), new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();
                }
                if ((this.showButtons && GameManager.instance.IsGameplayScene() && UIManager.instance.uiState == UIState.PLAYING) || (GameManager.instance.IsGameplayScene() && UIManager.instance.uiState == UIState.PAUSED))
                {
                    TextAnchor alignment = GUI.skin.button.alignment;
                    int fontSize = GUI.skin.toggle.fontSize;
                    FontStyle fontStyle = GUI.skin.toggle.fontStyle;
                    int fontSize2 = GUI.skin.button.fontSize;
                    FontStyle fontStyle2 = GUI.skin.button.fontStyle;
                    GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                    GUI.skin.button.fontSize = 18;
                    GUI.skin.button.fontStyle = FontStyle.Bold;
                    GUI.skin.toggle.fontSize = 18;
                    GUI.skin.toggle.fontStyle = FontStyle.Bold;
                    GUILayout.BeginArea(new Rect(10f, 1005f, 1090f, 70f));
                    GUILayout.BeginHorizontal("", new GUILayoutOption[0]);
                    if (GUILayout.Button("HIDE MENU", new GUILayoutOption[]
                {
                GUILayout.Height(30f),
                GUILayout.Width(150f)
                }))
                    {
                        this.showMenus = false;
                    }
                    if (GUILayout.Button("KILL ALL", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    }))
                    {
                        PlayMakerFSM.BroadcastEvent("INSTA KILL");
                        Console.AddLine("INSTA KILL broadcasted!");
                    }
                    Color backgroundColor = GUI.backgroundColor;
                    if (BossHandler.bossSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    BossHandler.bossSub = GUILayout.Toggle(BossHandler.bossSub, "BOSSES", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    if (this.charmSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    this.charmSub = GUILayout.Toggle(this.charmSub, "CHARMS", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    if (this.skillSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    this.skillSub = GUILayout.Toggle(this.skillSub, "SKILLS", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    if (this.itemSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    this.itemSub = GUILayout.Toggle(this.itemSub, "ITEMS", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    if (DreamGate.menuOpen)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    DreamGate.menuOpen = GUILayout.Toggle(DreamGate.menuOpen, "DREAMGATE", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal("", new GUILayoutOption[0]);
                    if (GUILayout.Button("INFINIJUMP", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    }))
                    {
                        PlayerData.instance.infiniteAirJump = !PlayerData.instance.infiniteAirJump;
                        Console.AddLine("Infinite Jump set to " + PlayerData.instance.infiniteAirJump.ToString().ToUpper());
                    }
                    if (GUILayout.Button("INFINISOUL", new GUILayoutOption[]
                {
                GUILayout.Height(30f),
                GUILayout.Width(150f)
                }))
                    {
                        this.infiniSOUL = !this.infiniSOUL;
                        Console.AddLine("Infinite SOUL set to " + this.infiniSOUL.ToString().ToUpper());
                    }
                    if (GUILayout.Button("INFINITE HP", new GUILayoutOption[]
                {
                GUILayout.Height(30f),
                GUILayout.Width(150f)
                }))
                    {
                        this.infiniteHP = !this.infiniteHP;
                        Console.AddLine("Infinite HP set to " + this.infiniteHP.ToString().ToUpper());
                    }
                    if (GUILayout.Button("INVINCIBLE", new GUILayoutOption[]
                {
                GUILayout.Height(30f),
                GUILayout.Width(150f)
                }))
                    {
                        PlayerData.instance.isInvincible = !PlayerData.instance.isInvincible;
                        Console.AddLine("Invincibility set to " + PlayerData.instance.isInvincible.ToString().ToUpper());
                    }
                    if (GUILayout.Button("SET RESPAWN", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    }))
                    {
                        this.manualRespawn = DebugMod.refKnight.transform.position;
                        HeroController.instance.SetHazardRespawn(this.manualRespawn, false);
                    }
                    if (GUILayout.Button("DUMP LOG", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    }))
                    {
                        Console.AddLine("Saving console log...");
                        Console.SaveHistory();
                    }
                    if (GUILayout.Button("RESPAWN", new GUILayoutOption[]
                {
                GUILayout.Height(30f),
                GUILayout.Width(150f)
                }))
                    {
                        Console.AddLine("Trying to respawn Hero...");
                        this.Respawn();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    TextAnchor alignment2 = GUI.skin.label.alignment;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    if (this.infiniSOUL)
                    {
                        GUI.Label(new Rect(167f, 1045f, 30f, 30f), "<color=lime>✔</color>");
                    }
                    if (PlayerData.instance.infiniteAirJump)
                    {
                        GUI.Label(new Rect(12f, 1045f, 30f, 30f), "<color=lime>✔</color>");
                    }
                    if (this.infiniteHP)
                    {
                        GUI.Label(new Rect(322f, 1045f, 30f, 30f), "<color=lime>✔</color>");
                    }
                    if (PlayerData.instance.isInvincible)
                    {
                        GUI.Label(new Rect(477f, 1045f, 30f, 30f), "<color=lime>✔</color>");
                    }
                    GUI.skin.button.alignment = alignment;
                    GUI.skin.label.alignment = alignment2;
                    GUI.skin.toggle.fontSize = fontSize;
                    GUI.skin.toggle.fontStyle = fontStyle;
                    GUI.skin.button.fontSize = fontSize2;
                    GUI.skin.button.fontStyle = fontStyle2;
                    int fontSize3 = GUI.skin.toggle.fontSize;
                    FontStyle fontStyle3 = GUI.skin.toggle.fontStyle;
                    GUI.skin.toggle.fontSize = 18;
                    GUI.skin.toggle.fontStyle = FontStyle.Bold;
                    if (this.skillSub)
                    {
                        TextAnchor alignment3 = GUI.skin.button.alignment;
                        int fontSize4 = GUI.skin.button.fontSize;
                        Color contentColor = GUI.contentColor;
                        Color backgroundColor2 = GUI.backgroundColor;
                        GUI.skin.button.fontSize = 18;
                        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                        GUI.contentColor = Color.white;
                        GUI.backgroundColor = Color.white;
                        GUILayout.BeginArea(new Rect(630f, 0f, 150f, 1005f));
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                        if (GUILayout.Button("ALL SKILLS", new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            PlayerData.instance.hasSpell = true;
                            PlayerData.instance.fireballLevel = 2;
                            PlayerData.instance.quakeLevel = 2;
                            PlayerData.instance.screamLevel = 2;
                            PlayerData.instance.hasNailArt = true;
                            PlayerData.instance.hasCyclone = true;
                            PlayerData.instance.hasDashSlash = true;
                            PlayerData.instance.hasUpwardSlash = true;
                            PlayerData.instance.hasAllNailArts = true;
                            PlayerData.instance.hasDreamNail = true;
                            PlayerData.instance.dreamNailUpgraded = true;
                            PlayerData.instance.hasDash = true;
                            PlayerData.instance.canDash = true;
                            PlayerData.instance.hasWalljump = true;
                            PlayerData.instance.hasSuperDash = true;
                            PlayerData.instance.hasShadowDash = true;
                            PlayerData.instance.hasAcidArmour = true;
                            PlayerData.instance.hasDoubleJump = true;
                            Console.AddLine("All SKILLs and SPELLs are activated");
                        }
                        if (GUILayout.Button("Scream lvl: " + PlayerData.instance.screamLevel.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.screamLevel >= 2)
                            {
                                PlayerData.instance.screamLevel = 0;
                            }
                            else
                            {
                                PlayerData.instance.screamLevel++;
                            }
                        }
                        if (GUILayout.Button("Firebll lvl: " + PlayerData.instance.fireballLevel.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.fireballLevel >= 2)
                            {
                                PlayerData.instance.fireballLevel = 0;
                            }
                            else
                            {
                                PlayerData.instance.fireballLevel++;
                            }
                        }
                        if (GUILayout.Button("Quake lvl: " + PlayerData.instance.quakeLevel.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.quakeLevel >= 2)
                            {
                                PlayerData.instance.quakeLevel = 0;
                            }
                            else
                            {
                                PlayerData.instance.quakeLevel++;
                            }
                        }
                        GUI.backgroundColor = Color.green;
                        PlayerData.instance.hasAcidArmour = GUILayout.Toggle(PlayerData.instance.hasAcidArmour, "Isma's Tear", new GUILayoutOption[0]);
                        PlayerData.instance.hasDoubleJump = GUILayout.Toggle(PlayerData.instance.hasDoubleJump, "Double Jump", new GUILayoutOption[0]);
                        PlayerData.instance.hasShadowDash = GUILayout.Toggle(PlayerData.instance.hasShadowDash, "Shadow Dash", new GUILayoutOption[0]);
                        PlayerData.instance.hasSuperDash = GUILayout.Toggle(PlayerData.instance.hasSuperDash, "Super  Dash", new GUILayoutOption[0]);
                        PlayerData.instance.hasWalljump = GUILayout.Toggle(PlayerData.instance.hasWalljump, "Wall  Jump", new GUILayoutOption[0]);
                        PlayerData.instance.hasDash = GUILayout.Toggle(PlayerData.instance.hasDash, "Has  Dash", new GUILayoutOption[0]);
                        PlayerData.instance.canDash = GUILayout.Toggle(PlayerData.instance.canDash, "Can  Dash", new GUILayoutOption[0]);
                        PlayerData.instance.hasDreamNail = GUILayout.Toggle(PlayerData.instance.hasDreamNail, "Dream Nail", new GUILayoutOption[0]);
                        PlayerData.instance.dreamNailUpgraded = GUILayout.Toggle(PlayerData.instance.dreamNailUpgraded, "Dream Nail2", new GUILayoutOption[0]);
                        PlayerData.instance.hasDreamGate = GUILayout.Toggle(PlayerData.instance.hasDreamGate, "Dream Gate", new GUILayoutOption[0]);
                        PlayerData.instance.hasNailArt = GUILayout.Toggle(PlayerData.instance.hasNailArt, "Enable Arts", new GUILayoutOption[0]);
                        PlayerData.instance.hasCyclone = GUILayout.Toggle(PlayerData.instance.hasCyclone, "Cyclone Slash", new GUILayoutOption[0]);
                        PlayerData.instance.hasDashSlash = GUILayout.Toggle(PlayerData.instance.hasDashSlash, "Charge Slash", new GUILayoutOption[0]);
                        PlayerData.instance.hasUpwardSlash = GUILayout.Toggle(PlayerData.instance.hasUpwardSlash, "Dash Slash", new GUILayoutOption[0]);
                        GUILayout.EndVertical();
                        GUILayout.EndArea();
                        GUI.skin.button.alignment = alignment3;
                        GUI.contentColor = contentColor;
                        GUI.backgroundColor = backgroundColor2;
                        GUI.skin.button.fontSize = fontSize4;
                    }
                    if (this.charmSub)
                    {
                        TextAnchor alignment4 = GUI.skin.button.alignment;
                        int fontSize5 = GUI.skin.button.fontSize;
                        Color contentColor2 = GUI.contentColor;
                        Color backgroundColor3 = GUI.backgroundColor;
                        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                        GUI.skin.button.fontSize = 18;
                        GUI.contentColor = Color.white;
                        GUI.backgroundColor = Color.white;
                        GUILayout.BeginArea(new Rect(475f, 0f, 150f, 1005f));
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                        if (GUILayout.Button("ALL CHARMS", new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            for (int i = 1; i < 37; i++)
                            {
                                PlayerData.instance.GetType().GetField("gotCharm_" + i.ToString()).SetValue(PlayerData.instance, true);
                            }
                            PlayerData.instance.charmSlots = 10;
                            PlayerData.instance.hasCharm = true;
                            PlayerData.instance.charmsOwned = 36;
                            PlayerData.instance.royalCharmState = 4;
                            PlayerData.instance.notchShroomOgres = true;
                            PlayerData.instance.notchFogCanyon = true;
                            PlayerData.instance.salubraNotch1 = true;
                            PlayerData.instance.salubraNotch2 = true;
                            PlayerData.instance.salubraNotch3 = true;
                            PlayerData.instance.salubraNotch4 = true;
                            Console.AddLine("Added all charms to inventory");
                        }
                        if (GUILayout.Button("Notches: " + PlayerData.instance.charmSlots.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.charmSlots >= 10)
                            {
                                PlayerData.instance.charmSlots = 1;
                            }
                            else
                            {
                                PlayerData.instance.charmSlots++;
                            }
                        }
                        if (GUILayout.Button("Kingsoul: " + PlayerData.instance.royalCharmState.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.royalCharmState >= 4)
                            {
                                PlayerData.instance.royalCharmState = 0;
                            }
                            else
                            {
                                PlayerData.instance.royalCharmState++;
                            }
                        }
                        GUI.backgroundColor = Color.green;
                        PlayerData.instance.brokenCharm_23 = GUILayout.Toggle(PlayerData.instance.brokenCharm_23, "fHrt broken?", new GUILayoutOption[0]);
                        PlayerData.instance.brokenCharm_24 = GUILayout.Toggle(PlayerData.instance.brokenCharm_24, "fGrd broken?", new GUILayoutOption[0]);
                        PlayerData.instance.brokenCharm_25 = GUILayout.Toggle(PlayerData.instance.brokenCharm_25, "fStr broken?", new GUILayoutOption[0]);
                        GUILayout.EndVertical();
                        GUILayout.EndArea();
                        GUI.skin.button.alignment = alignment4;
                        GUI.contentColor = contentColor2;
                        GUI.backgroundColor = backgroundColor3;
                        GUI.skin.button.fontSize = fontSize5;
                    }
                    if (this.itemSub)
                    {
                        TextAnchor alignment5 = GUI.skin.button.alignment;
                        int fontSize6 = GUI.skin.button.fontSize;
                        Color contentColor3 = GUI.contentColor;
                        Color backgroundColor4 = GUI.backgroundColor;
                        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                        GUI.skin.button.fontSize = 18;
                        GUI.contentColor = Color.white;
                        GUI.backgroundColor = Color.white;
                        GUILayout.BeginArea(new Rect(785f, 0f, 150f, 1005f));
                        GUILayout.FlexibleSpace();
                        GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                        if (GUILayout.Button("Pale Ore: " + PlayerData.instance.ore.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.ore >= 6)
                            {
                                PlayerData.instance.ore = 0;
                            }
                            else
                            {
                                PlayerData.instance.ore++;
                            }
                        }
                        if (GUILayout.Button("SimpleKeys: " + PlayerData.instance.simpleKeys.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.simpleKeys >= 3)
                            {
                                PlayerData.instance.simpleKeys = 0;
                            }
                            else
                            {
                                PlayerData.instance.simpleKeys++;
                            }
                        }
                        if (GUILayout.Button("Eggs: " + PlayerData.instance.rancidEggs.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.rancidEggs >= 25)
                            {
                                PlayerData.instance.rancidEggs = 0;
                            }
                            else
                            {
                                PlayerData.instance.rancidEggs++;
                            }
                        }
                        if (GUILayout.Button("Essence: " + PlayerData.instance.dreamOrbs.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }))
                        {
                            if (PlayerData.instance.dreamOrbs < 4000)
                            {
                                PlayerData.instance.dreamOrbs += 100;
                            }
                            else if (PlayerData.instance.dreamOrbs + 100 > 4000)
                            {
                                PlayerData.instance.dreamOrbs = 0;
                            }
                        }
                        if (GUILayout.Button("Geo: " + PlayerData.instance.geo.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.Height(20f)
                    }) && PlayerData.instance.geo + 500 <= 99999)
                        {
                            PlayerData.instance.AddGeo(500);
                            HeroController.instance.geoCounter.AddGeo(500);
                        }
                        GUI.backgroundColor = Color.green;
                        PlayerData.instance.hasLantern = GUILayout.Toggle(PlayerData.instance.hasLantern, "Lantern", new GUILayoutOption[0]);
                        PlayerData.instance.hasTramPass = GUILayout.Toggle(PlayerData.instance.hasTramPass, "Tram Pass", new GUILayoutOption[0]);
                        PlayerData.instance.hasQuill = GUILayout.Toggle(PlayerData.instance.hasQuill, "Quill&Ink", new GUILayoutOption[0]);
                        PlayerData.instance.hasCityKey = GUILayout.Toggle(PlayerData.instance.hasCityKey, "City Key", new GUILayoutOption[0]);
                        PlayerData.instance.hasSlykey = GUILayout.Toggle(PlayerData.instance.hasSlykey, "Sly Key", new GUILayoutOption[0]);
                        PlayerData.instance.hasWhiteKey = GUILayout.Toggle(PlayerData.instance.hasWhiteKey, "White Key", new GUILayoutOption[0]);
                        PlayerData.instance.hasLoveKey = GUILayout.Toggle(PlayerData.instance.hasLoveKey, "Love Key", new GUILayoutOption[0]);
                        PlayerData.instance.hasKingsBrand = GUILayout.Toggle(PlayerData.instance.hasKingsBrand, "King's Brand", new GUILayoutOption[0]);
                        PlayerData.instance.hasXunFlower = GUILayout.Toggle(PlayerData.instance.hasXunFlower, "Damn Flower", new GUILayoutOption[0]);
                        GUILayout.EndVertical();
                        GUILayout.EndArea();
                        GUI.skin.button.alignment = alignment5;
                        GUI.contentColor = contentColor3;
                        GUI.backgroundColor = backgroundColor4;
                        GUI.skin.button.fontSize = fontSize6;
                    }

                    BossHandler.UpdateGUI();

                    DreamGate.UpdateGUI();

                    GUI.skin.toggle.fontSize = fontSize3;
                    GUI.skin.toggle.fontStyle = fontStyle3;
                }

                Console.UpdateGUI(DreamGate.menuOpen);

                if (DebugMod.gm.IsGameplayScene() && this.showHelp)
                {
                    TextAnchor alignment10 = GUI.skin.label.alignment;
                    TextAnchor alignment11 = GUI.skin.label.alignment;
                    int fontSize9 = GUI.skin.label.fontSize;
                    GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.skin.label.fontSize = 18;
                    GUI.Label(new Rect(10f, 215f, 500f, 820f), string.Concat(new string[]
                {
                "~ ----- HIDE/SHOW HELP",
                Environment.NewLine,
                " ",
                Environment.NewLine,
                "F1 ---- HIDE/SHOW ALL GUI",
                Environment.NewLine,
                "F2 ---- SHOW/HIDE STATE PANEL",
                Environment.NewLine,
                "F3 ---- SHOW/HIDE BUTTONS",
                Environment.NewLine,
                "F4 ---- SHOW/HIDE CONSOLE",
                Environment.NewLine,
                "F5 ---- FORCE PAUSE MENU",
                Environment.NewLine,
                "F6 ---- HAZARD RESPAWN",
                Environment.NewLine,
                "F7 ---- SET HZRD RESPAWN",
                Environment.NewLine,
                "F8 ---- FORCE CAMERA TO FOLLOW",
                Environment.NewLine,
                "F9 ---- ENEMY PANEL",
                Environment.NewLine,
                "F10 --- HERO SELF-DAMAGE",
                Environment.NewLine,
                " ",
                Environment.NewLine,
                "PLUS ------ nailDamage +4",
                Environment.NewLine,
                "MINUS ----- nailDamage -4",
                Environment.NewLine,
                "NUMPAD+ --- TimeScale UP",
                Environment.NewLine,
                "NUMPAD- --- TimeScale DOWN",
                Environment.NewLine,
                "HOME ------ TOGGLE HeroLight",
                Environment.NewLine,
                " ",
                Environment.NewLine,
                "INSERT ---- TOGGLE Vignette",
                Environment.NewLine,
                "PageUP ---- CAMERA ZOOM+ ",
                Environment.NewLine,
                "PageDN ---- CAMERA ZOOM-",
                Environment.NewLine,
                "END ------- RESET ZOOM",
                Environment.NewLine,
                "DELETE ---- TOGGLE HUD",
                Environment.NewLine,
                "BACKSPC --- HIDE HERO",
                Environment.NewLine,
                "ESCAPE ---- CALL ALL PANELS",
                Environment.NewLine,
                Environment.NewLine,
                Environment.NewLine,
                Environment.NewLine,
                "Press [`] to hide Help"
                }));
                    GUI.skin.label.alignment = alignment10;
                    GUI.skin.label.alignment = alignment11;
                    GUI.skin.label.fontSize = fontSize9;
                }

                EnemyController.UpdateGUI(matrix, base.transform);
            }

            SaveManager.UpdateGUI();

            GUI.matrix = matrix;
        }

        public string ReadInput()
        {
            string empty = string.Empty;
            string format = "Move Vector: {0}, {1}";
            Vector2 vector = DebugMod.ih.inputActions.moveVector.Vector;
            object arg = vector.x.ToString();
            vector = DebugMod.ih.inputActions.moveVector.Vector;
            return string.Concat(new string[]
            {
                empty,
                string.Format(format, arg, vector.y.ToString()),
                string.Format("\nMove Pressed: {0}", DebugMod.ih.inputActions.left.IsPressed || DebugMod.ih.inputActions.right.IsPressed),
                string.Format("\nMove Raw L: {0} R: {1}", DebugMod.ih.inputActions.left.RawValue, DebugMod.ih.inputActions.right.RawValue),
                string.Format("\nInputX: " + DebugMod.ih.inputX, new object[0]),
                string.Format("\nAny Key Down: {0}", InputManager.AnyKeyIsPressed)
            });
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

        public void setMatrix()
        {
            this.originalWidth = (float)Screen.width;
            this.originalHeight = (float)Screen.height;
            this.scale.x = (float)Screen.width / 1920f;
            this.scale.y = (float)Screen.height / 1080f;
            this.scale.z = 1f;
        }

        public void Update()
        {
            if (this.originalHeight != Screen.height || this.originalWidth != Screen.width) this.setMatrix();

            if (DebugMod.GetSceneName() != "Menu_Title")
            {
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
                    if (this.showPanel)
                    {
                        this.showPanel = false;
                    }
                    this.showHelp = !this.showHelp;
                }
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    this.showMenus = !this.showMenus;
                }
                if (Input.GetKeyUp(KeyCode.F2) && !Input.GetKey(KeyCode.LeftControl))
                {
                    if (this.showHelp)
                    {
                        this.showHelp = false;
                    }
                    this.showPanel = !this.showPanel;
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F2))
                {
                    if (this.showPanelState >= 6)
                    {
                        this.showPanelState = 1;
                    }
                    else
                    {
                        this.showPanelState++;
                    }
                }
                if (Input.GetKeyUp(KeyCode.F3))
                {
                    this.showButtons = !this.showButtons;
                }
                if (Input.GetKeyUp(KeyCode.F4))
                {
                    Console.visible = !Console.visible;
                }
                if (Input.GetKeyUp(KeyCode.F5))
                {
                    if (PlayerData.instance.disablePause && DebugMod.GetSceneName() != "Menu_Title" && DebugMod.gm.IsGameplayScene() && !HeroController.instance.cState.recoiling)
                    {
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
                if (Input.GetKeyUp(KeyCode.F6))
                {
                    this.Respawn();
                }
                if (Input.GetKeyUp(KeyCode.F7))
                {
                    this.manualRespawn = DebugMod.refKnight.transform.position;
                    HeroController.instance.SetHazardRespawn(this.manualRespawn, false);
                    Console.AddLine("Manual respawn point on this map set to" + this.manualRespawn.ToString());
                }
                if (Input.GetKeyUp(KeyCode.F8))
                {
                    string text = DebugMod.refCamera.mode.ToString();
                    if (!this.cameraFollow && text != "FOLLOWING")
                    {
                        Console.AddLine("Setting Camera Mode to FOLLOW. Previous mode: " + text);
                        this.cameraFollow = true;
                    }
                    else if (this.cameraFollow)
                    {
                        this.cameraFollow = false;
                        Console.AddLine("Camera Mode is no longer forced");
                    }
                }
                if (Input.GetKeyUp(KeyCode.F9))
                {
                    EnemyController.enemyPanel = !EnemyController.enemyPanel;
                    if (EnemyController.enemyPool.Count < 1 && EnemyController.enemyPanel)
                    {
                        EnemyController.enemyCheck = false;
                        EnemyController.RefreshEnemyList(base.gameObject);
                    }
                }
                if (Input.GetKeyUp(KeyCode.F10))
                {
                    EnemyController.selfDamage();
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
                if (this.cameraFollow && DebugMod.refCamera.mode != CameraController.CameraMode.FOLLOWING)
                {
                    DebugMod.refCamera.SetMode(CameraController.CameraMode.FOLLOWING);
                }
                if (this.infiniteHP && !HeroController.instance.cState.dead && GameManager.instance.IsGameplayScene() && PlayerData.instance.health < PlayerData.instance.maxHealth)
                {
                    int amount = PlayerData.instance.maxHealth - PlayerData.instance.health;
                    PlayerData.instance.health = PlayerData.instance.maxHealth;
                    HeroController.instance.AddHealth(amount);
                }
                if (this.infiniSOUL && PlayerData.instance.MPCharge < 100 && PlayerData.instance.health > 0 && !HeroController.instance.cState.dead && GameManager.instance.IsGameplayScene())
                {
                    PlayerData.instance.MPCharge = 100;
                }

                if (PlayerDeathWatcher.PlayerDied())
                {
                    PlayerDeathWatcher.LogDeathDetails();
                }

                if (PlayerData.instance.hazardRespawnLocation != this.hazardLocation)
                {
                    this.hazardLocation = PlayerData.instance.hazardRespawnLocation;
                    Console.AddLine("Hazard Respawn location updated: " + this.hazardLocation.ToString());
                    if (this.showMenus && EnemyController.enemyPanel)
                    {
                        EnemyController.RefreshEnemyList(base.gameObject);
                    }
                }
                if (!string.IsNullOrEmpty(this.respawnSceneWatch) && this.respawnSceneWatch != PlayerData.instance.respawnScene)
                {
                    this.respawnSceneWatch = PlayerData.instance.respawnScene;
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

                EnemyController.CheckForAutoUpdate();
            }
        }

        public void DrawRect(Rect position, Color color)
        {
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position, GUIContent.none, this.textureStyle);
            GUI.backgroundColor = backgroundColor;
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

        public static GUIController _instance;

        public const float textWidth = 100f;

        public Vector3 hazardLocation;

        public Vector3 scale;

        public bool showMenus;
        
        public bool showPanel;

        public bool showButtons;

        public bool infiniSOUL;

        public Vector3 manualRespawn;

        public bool cameraFollow;

        public bool infiniteHP;

        public string modVersion;

        public float originalWidth;
        
        public float originalHeight;

        public string respawnSceneWatch;

        public Texture2D backgroundTexture;

        public GUIStyle textureStyle;

        public bool showHelp;

        public bool skillSub;

        public bool charmSub;

        public bool itemSub;

        public int showPanelState;
    }
}