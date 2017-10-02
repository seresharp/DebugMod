
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
        private Font trajanBold;
        private Font trajanNormal;
        private GameObject canvas;
        private CanvasPanel topMenu;
        private CanvasPanel infoPanel;
        private Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();
        private bool playerInvincible;

        public void Awake()
        {
            if (backgroundTexture == null)
            {
                backgroundTexture = new Texture2D(1, 1);
                backgroundTexture.SetPixel(0, 0, Color.white);
                backgroundTexture.Apply();
            }

            if (textureStyle == null)
            {
                textureStyle = new GUIStyle();
                textureStyle.normal.background = backgroundTexture;
            }
            
            modVersion = "1.2.0.2";
            showMenus = true;
            showHelp = true;
            showPanelState = 6;
            hazardLocation = PlayerData.instance.hazardRespawnLocation;
            respawnSceneWatch = PlayerData.instance.respawnScene;
            setMatrix();
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

            infoPanel = new CanvasPanel(canvas, images["StatusPanelBG"], new Vector2(0f, 223f), Vector2.zero, new Rect(0f, 0f, images["StatusPanelBG"].width, images["StatusPanelBG"].height));

            //Labels
            infoPanel.AddText("Hero State Label", "Hero State", new Vector2(10f, 20f), trajanBold);
            infoPanel.AddText("Velocity Label", "Velocity", new Vector2(10f, 40f), trajanBold);
            infoPanel.AddText("Nail Damage Label", "Naildmg", new Vector2(10f, 60f), trajanBold);
            infoPanel.AddText("HP Label", "HP", new Vector2(10f, 80f), trajanBold);
            infoPanel.AddText("MP Label", "MP", new Vector2(10f, 100f), trajanBold);

            infoPanel.AddText("Completion Label", "Completion", new Vector2(10f, 178f), trajanBold);
            infoPanel.AddText("Grubs Label", "Grubs", new Vector2(10f, 198f), trajanBold);
            
            infoPanel.AddText("isInvuln Label", "isInvuln", new Vector2(10f, 276f), trajanBold);
            infoPanel.AddText("Invincible Label", "Invincible", new Vector2(10f, 296f), trajanBold);
            infoPanel.AddText("Invincitest Label", "invinciTest", new Vector2(10f, 316f), trajanBold);
            infoPanel.AddText("Damage State Label", "Damage State", new Vector2(10f, 336f), trajanBold);
            infoPanel.AddText("Dead State Label", "Dead State", new Vector2(10f, 356f), trajanBold);
            infoPanel.AddText("Hazard Death Label", "Hazard Death", new Vector2(10f, 376f), trajanBold);

            infoPanel.AddText("Scene Name Label", "Scene Name", new Vector2(10f, 454), trajanBold);
            infoPanel.AddText("Transition Label", "Transition", new Vector2(10f, 474f), trajanBold);
            infoPanel.AddText("Trans State Label", "Trans State", new Vector2(10f, 494f), trajanBold);
            infoPanel.AddText("is Gameplay Label", "Is Gameplay", new Vector2(10f, 514f), trajanBold);
            infoPanel.AddText("Game State Label", "Game State", new Vector2(10f, 534f), trajanBold);
            infoPanel.AddText("UI State Label", "UI State", new Vector2(10f, 554f), trajanBold);
            infoPanel.AddText("Hero Paused Label", "Hero Paused", new Vector2(10f, 574f), trajanBold);
            infoPanel.AddText("Camera Mode Label", "Camera Mode", new Vector2(10f, 594f), trajanBold);

            infoPanel.AddText("Accept Input Label", "Accept Input", new Vector2(300f, 30f), trajanBold);
            infoPanel.AddText("Relinquished Label", "Relinquished", new Vector2(300f, 50f), trajanBold);
            infoPanel.AddText("atBench Label", "atBench", new Vector2(300f, 70f), trajanBold);

            infoPanel.AddText("Dashing Label", "Dashing", new Vector2(300f, 120f), trajanBold);
            infoPanel.AddText("Jumping Label", "Jumping", new Vector2(300f, 140f), trajanBold);
            infoPanel.AddText("Superdashing Label", "Superdashing", new Vector2(300f, 160f), trajanBold);
            infoPanel.AddText("Falling Label", "Falling", new Vector2(300f, 180f), trajanBold);
            infoPanel.AddText("Hardland Label", "Hardland", new Vector2(300f, 200f), trajanBold);
            infoPanel.AddText("Swimming Label", "Swimming", new Vector2(300f, 220f), trajanBold);
            infoPanel.AddText("Recoiling Label", "Recoiling", new Vector2(300f, 240f), trajanBold);

            infoPanel.AddText("Wall lock Label", "Wall lock", new Vector2(300f, 290f), trajanBold);
            infoPanel.AddText("Wall jumping Label", "Wall jumping", new Vector2(300f, 310f), trajanBold);
            infoPanel.AddText("Wall touching Label", "Wall touching", new Vector2(300f, 330f), trajanBold);
            infoPanel.AddText("Wall sliding Label", "Wall sliding", new Vector2(300f, 350f), trajanBold);
            infoPanel.AddText("Wall left Label", "Wall left", new Vector2(300f, 370f), trajanBold);
            infoPanel.AddText("Wall right Label", "Wall right", new Vector2(300f, 390f), trajanBold);

            infoPanel.AddText("Attacking Label", "Attacking", new Vector2(300f, 440f), trajanBold);
            infoPanel.AddText("canCast Label", "canCast", new Vector2(300f, 460f), trajanBold);
            infoPanel.AddText("canSuperdash Label", "canSuperdash", new Vector2(300f, 480f), trajanBold);
            infoPanel.AddText("canQuickmap Label", "canQuickmap", new Vector2(300f, 500f), trajanBold);
            infoPanel.AddText("canInventory Label", "canInventory", new Vector2(300f, 520f), trajanBold);
            infoPanel.AddText("canWarp Label", "canWarp", new Vector2(300f, 540f), trajanBold);
            infoPanel.AddText("canDGate Label", "canDGate", new Vector2(300f, 560f), trajanBold);
            infoPanel.AddText("gateAllow Label", "gateAllow", new Vector2(300f, 580f), trajanBold);

            //Values
            infoPanel.AddText("Hero State", "", new Vector2(150f, 20f), trajanNormal);
            infoPanel.AddText("Velocity", "", new Vector2(150f, 40f), trajanNormal);
            infoPanel.AddText("Nail Damage", "", new Vector2(150f, 60f), trajanNormal);
            infoPanel.AddText("HP", "", new Vector2(150f, 80f), trajanNormal);
            infoPanel.AddText("MP", "", new Vector2(150f, 100f), trajanNormal);

            infoPanel.AddText("Completion", "", new Vector2(150f, 178f), trajanNormal);
            infoPanel.AddText("Grubs", "", new Vector2(150f, 198f), trajanNormal);

            infoPanel.AddText("isInvuln", "", new Vector2(150f, 276f), trajanNormal);
            infoPanel.AddText("Invincible", "", new Vector2(150f, 296f), trajanNormal);
            infoPanel.AddText("Invincitest", "", new Vector2(150f, 316f), trajanNormal);
            infoPanel.AddText("Damage State", "", new Vector2(150f, 336f), trajanNormal);
            infoPanel.AddText("Dead State", "", new Vector2(150f, 356f), trajanNormal);
            infoPanel.AddText("Hazard Death", "", new Vector2(150f, 376f), trajanNormal);

            infoPanel.AddText("Scene Name", "", new Vector2(150f, 454), trajanNormal);
            infoPanel.AddText("Transition", "", new Vector2(150f, 474f), trajanNormal);
            infoPanel.AddText("Trans State", "", new Vector2(150f, 494f), trajanNormal);
            infoPanel.AddText("is Gameplay", "", new Vector2(150f, 514f), trajanNormal);
            infoPanel.AddText("Game State", "", new Vector2(150f, 534f), trajanNormal);
            infoPanel.AddText("UI State", "", new Vector2(150f, 554f), trajanNormal);
            infoPanel.AddText("Hero Paused", "", new Vector2(150f, 574f), trajanNormal);
            infoPanel.AddText("Camera Mode", "", new Vector2(150f, 594f), trajanNormal);

            infoPanel.AddText("Accept Input", "", new Vector2(440f, 30f), trajanNormal);
            infoPanel.AddText("Relinquished", "", new Vector2(440f, 50f), trajanNormal);
            infoPanel.AddText("atBench", "", new Vector2(440f, 70f), trajanNormal);

            infoPanel.AddText("Dashing", "", new Vector2(440f, 120f), trajanNormal);
            infoPanel.AddText("Jumping", "", new Vector2(440f, 140f), trajanNormal);
            infoPanel.AddText("Superdashing", "", new Vector2(440f, 160f), trajanNormal);
            infoPanel.AddText("Falling", "", new Vector2(440f, 180f), trajanNormal);
            infoPanel.AddText("Hardland", "", new Vector2(440f, 200f), trajanNormal);
            infoPanel.AddText("Swimming", "", new Vector2(440f, 220f), trajanNormal);
            infoPanel.AddText("Recoiling", "", new Vector2(440f, 240f), trajanNormal);

            infoPanel.AddText("Wall lock", "", new Vector2(440f, 290f), trajanNormal);
            infoPanel.AddText("Wall jumping", "", new Vector2(440f, 310f), trajanNormal);
            infoPanel.AddText("Wall touching", "", new Vector2(440f, 330f), trajanNormal);
            infoPanel.AddText("Wall sliding", "", new Vector2(440f, 350f), trajanNormal);
            infoPanel.AddText("Wall left", "", new Vector2(440f, 370f), trajanNormal);
            infoPanel.AddText("Wall right", "", new Vector2(440f, 390f), trajanNormal);

            infoPanel.AddText("Attacking", "", new Vector2(440f, 440f), trajanNormal);
            infoPanel.AddText("canCast", "", new Vector2(440f, 460f), trajanNormal);
            infoPanel.AddText("canSuperdash", "", new Vector2(440f, 480f), trajanNormal);
            infoPanel.AddText("canQuickmap", "", new Vector2(440f, 500f), trajanNormal);
            infoPanel.AddText("canInventory", "", new Vector2(440f, 520f), trajanNormal);
            infoPanel.AddText("canWarp", "", new Vector2(440f, 540f), trajanNormal);
            infoPanel.AddText("canDGate", "", new Vector2(440f, 560f), trajanNormal);
            infoPanel.AddText("gateAllow", "", new Vector2(440f, 580f), trajanNormal);

            topMenu = new CanvasPanel(canvas, images["ButtonsMenuBG"], new Vector2(1092f, 25f), Vector2.zero, new Rect(0f, 0f, images["ButtonsMenuBG"].width, images["ButtonsMenuBG"].height));

            Rect buttonRect = new Rect(0, 0, images["ButtonRect"].width, images["ButtonRect"].height);

            //Main buttons
            topMenu.AddButton("Hide Menu", images["ButtonRect"], new Vector2(46f, 28f), Vector2.zero, HideMenuClicked, buttonRect, trajanBold, "Hide Menu");
            topMenu.AddButton("Kill All", images["ButtonRect"], new Vector2(146f, 28f), Vector2.zero, KillAllClicked, buttonRect, trajanBold, "Kill All");
            topMenu.AddButton("Set Spawn", images["ButtonRect"], new Vector2(246f, 28f), Vector2.zero, SetSpawnClicked, buttonRect, trajanBold, "Set Spawn");
            topMenu.AddButton("Respawn", images["ButtonRect"], new Vector2(346f, 28f), Vector2.zero, RespawnClicked, buttonRect, trajanBold, "Respawn");
            topMenu.AddButton("Dump Log", images["ButtonRect"], new Vector2(446f, 28f), Vector2.zero, DumpLogClicked, buttonRect, trajanBold, "Dump Log");
            topMenu.AddButton("Cheats", images["ButtonRect"], new Vector2(46f, 68f), Vector2.zero, CheatsClicked, buttonRect, trajanBold, "Cheats");
            topMenu.AddButton("Charms", images["ButtonRect"], new Vector2(146f, 68f), Vector2.zero, CharmsClicked, buttonRect, trajanBold, "Charms");
            topMenu.AddButton("Skills", images["ButtonRect"], new Vector2(246f, 68f), Vector2.zero, SkillsClicked, buttonRect, trajanBold, "Skills");
            topMenu.AddButton("Items", images["ButtonRect"], new Vector2(346f, 68f), Vector2.zero, ItemsClicked, buttonRect, trajanBold, "Items");
            topMenu.AddButton("Bosses", images["ButtonRect"], new Vector2(446f, 68f), Vector2.zero, BossesClicked, buttonRect, trajanBold, "Bosses");
            topMenu.AddButton("DreamGate", images["ButtonRect"], new Vector2(546f, 68f), Vector2.zero, DreamGatePanelClicked, buttonRect, trajanBold, "DreamGate");

            //Dropdown panels
            topMenu.AddPanel("Cheats Panel", images["DropdownBG"], new Vector2(45f, 75f), Vector2.zero, new Rect(0, 0, images["DropdownBG"].width, 150f));
            topMenu.AddPanel("Charms Panel", images["DropdownBG"], new Vector2(145f, 75f), Vector2.zero, new Rect(0, 0, images["DropdownBG"].width, 180f));
            topMenu.AddPanel("Skills Panel", images["DropdownBG"], new Vector2(245f, 75f), Vector2.zero, new Rect(0, 0, images["DropdownBG"].width, images["DropdownBG"].height));
            topMenu.AddPanel("Items Panel", images["DropdownBG"], new Vector2(345f, 75f), Vector2.zero, new Rect(0, 0, images["DropdownBG"].width, images["DropdownBG"].height));
            topMenu.AddPanel("Bosses Panel", images["DropdownBG"], new Vector2(445f, 75f), Vector2.zero, new Rect(0, 0, images["DropdownBG"].width, images["DropdownBG"].height));
            topMenu.AddPanel("DreamGate Panel", images["DreamGateDropdownBG"], new Vector2(545f, 75f), Vector2.zero, new Rect(0, 0, images["DreamGateDropdownBG"].width, images["DreamGateDropdownBG"].height));

            //Cheats panel
            topMenu.GetPanel("Cheats Panel").AddButton("Infinite Jump", images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, InfiniteJumpClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Infinite Jump", 10);
            topMenu.GetPanel("Cheats Panel").AddButton("Infinite Soul", images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, InfiniteSoulClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Infinite Soul", 10);
            topMenu.GetPanel("Cheats Panel").AddButton("Infinite HP", images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, InfiniteHPClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Infinite HP", 10);
            topMenu.GetPanel("Cheats Panel").AddButton("Invincibility", images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, InvincibilityClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Invincibility", 10);

            //Charms panel
            topMenu.GetPanel("Charms Panel").AddButton("All Charms", images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllCharmsClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "All Charms", 10);
            topMenu.GetPanel("Charms Panel").AddButton("Kingsoul", images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, KingsoulClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Kingsoul: " + PlayerData.instance.royalCharmState, 10);
            topMenu.GetPanel("Charms Panel").AddButton("fHeart fix", images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, FragileHeartFixClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "fHeart fix", 10);
            topMenu.GetPanel("Charms Panel").AddButton("fGreed fix", images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, FragileGreedFixClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "fGreed fix", 10);
            topMenu.GetPanel("Charms Panel").AddButton("fStrength fix", images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, FragileStrengthFixClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "fStrength fix", 10);

            //Skills panel buttons
            topMenu.GetPanel("Skills Panel").AddButton("All Skills", images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllSkillsClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "All Skills", 10);
            topMenu.GetPanel("Skills Panel").AddButton("Scream", images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, ScreamClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Scream: " + PlayerData.instance.screamLevel, 10);
            topMenu.GetPanel("Skills Panel").AddButton("Fireball", images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, FireballClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Fireball: " + PlayerData.instance.fireballLevel, 10);
            topMenu.GetPanel("Skills Panel").AddButton("Quake", images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, QuakeClicked, new Rect(0f, 0f, 80f, 20f), trajanNormal, "Quake: " + PlayerData.instance.quakeLevel, 10);
            topMenu.GetPanel("Skills Panel").AddButton("Mothwing Cloak", images["MothwingCloak"], new Vector2(5f, 150f), new Vector2(37f, 34f), MothwingCloakClicked, new Rect(0f, 0f, images["MothwingCloak"].width, images["MothwingCloak"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Mantis Claw", images["MantisClaw"], new Vector2(43f, 150f), new Vector2(37f, 34f), MantisClawClicked, new Rect(0f, 0f, images["MantisClaw"].width, images["MantisClaw"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Monarch Wings", images["MonarchWings"], new Vector2(5f, 194f), new Vector2(37f, 33f), MonarchWingsClicked, new Rect(0f, 0f, images["MonarchWings"].width, images["MonarchWings"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Crystal Heart", images["CrystalHeart"], new Vector2(43f, 194f), new Vector2(37f, 34f), CrystalHeartClicked, new Rect(0f, 0f, images["CrystalHeart"].width, images["CrystalHeart"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Isma's Tear", images["IsmasTear"], new Vector2(5f, 238f), new Vector2(37f, 40f), IsmasTearClicked, new Rect(0f, 0f, images["IsmasTear"].width, images["IsmasTear"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Dream Nail", images["DreamNail1"], new Vector2(43f, 251f), new Vector2(37f, 59f), DreamNailClicked, new Rect(0f, 0f, images["DreamNail1"].width, images["DreamNail1"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Dream Gate", images["DreamGate"], new Vector2(5f, 288f), new Vector2(37f, 36f), DreamGateClicked, new Rect(0f, 0f, images["DreamGate"].width, images["DreamGate"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Great Slash", images["NailArt_GreatSlash"], new Vector2(5f, 329f), new Vector2(23f, 23f), GreatSlashClicked, new Rect(0f, 0f, images["NailArt_GreatSlash"].width, images["NailArt_GreatSlash"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Dash Slash", images["NailArt_DashSlash"], new Vector2(33f, 329f), new Vector2(23f, 23f), DashSlashClicked, new Rect(0f, 0f, images["NailArt_DashSlash"].width, images["NailArt_DashSlash"].height));
            topMenu.GetPanel("Skills Panel").AddButton("Cyclone Slash", images["NailArt_CycloneSlash"], new Vector2(61f, 329f), new Vector2(23f, 23f), CycloneSlashClicked, new Rect(0f, 0f, images["NailArt_CycloneSlash"].width, images["NailArt_CycloneSlash"].height));

            //Skills panel button glow
            topMenu.GetPanel("Skills Panel").AddImage("Mothwing Cloak Glow", images["BlueGlow"], new Vector2(0f, 145f), new Vector2(47f, 44f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Mantis Claw Glow", images["BlueGlow"], new Vector2(38f, 145f), new Vector2(47f, 44f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Monarch Wings Glow", images["BlueGlow"], new Vector2(0f, 189f), new Vector2(47f, 43f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Crystal Heart Glow", images["BlueGlow"], new Vector2(38f, 189f), new Vector2(47f, 44f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Isma's Tear Glow", images["BlueGlow"], new Vector2(0f, 233f), new Vector2(47f, 50f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Dream Nail Glow", images["BlueGlow"], new Vector2(38f, 246f), new Vector2(47f, 69f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Dream Gate Glow", images["BlueGlow"], new Vector2(0f, 283f), new Vector2(47f, 46f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Great Slash Glow", images["BlueGlow"], new Vector2(0f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Dash Slash Glow", images["BlueGlow"], new Vector2(28f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));
            topMenu.GetPanel("Skills Panel").AddImage("Cyclone Slash Glow", images["BlueGlow"], new Vector2(56f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, images["BlueGlow"].width, images["BlueGlow"].height));

            SetMenusActive(false);

            topMenu.FixRenderOrder();
            RefreshMenus();

            GameObject.DontDestroyOnLoad(canvas);
        }

        public void SetMenusActive(bool active)
        {
            if (topMenu != null)
            {
                topMenu.SetActive(active, !active);
            }

            if (infoPanel != null)
            {
                infoPanel.SetActive(active, !active);
            }
        }

        public void CharacterLoaded()
        {
            RefreshMenus();
        }

        public void PlayerSetBool(string boolName, bool val)
        {
            RefreshMenus();
            PlayerData.instance.SetBoolInternal(boolName, val);
        }

        private void RefreshMenus()
        {
            if (topMenu.GetPanel("Skills Panel").active) RefreshSkillsMenu();

            topMenu.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(PlayerData.instance.infiniteAirJump ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private void RefreshSkillsMenu()
        {
            if (PlayerData.instance.dreamNailUpgraded) topMenu.GetButton("Dream Nail", "Skills Panel").UpdateSprite(images["DreamNail2"], new Rect(0f, 0f, images["DreamNail2"].width, images["DreamNail2"].height));
            else topMenu.GetButton("Dream Nail", "Skills Panel").UpdateSprite(images["DreamNail1"], new Rect(0f, 0f, images["DreamNail1"].width, images["DreamNail1"].height));
            if (PlayerData.instance.hasShadowDash) topMenu.GetButton("Mothwing Cloak", "Skills Panel").UpdateSprite(images["ShadeCloak"], new Rect(0f, 0f, images["ShadeCloak"].width, images["ShadeCloak"].height));
            else topMenu.GetButton("Mothwing Cloak", "Skills Panel").UpdateSprite(images["MothwingCloak"], new Rect(0f, 0f, images["MothwingCloak"].width, images["MothwingCloak"].height));

            topMenu.GetImage("Mothwing Cloak Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Mantis Claw Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Monarch Wings Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Crystal Heart Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Isma's Tear Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Dream Gate Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Dream Nail Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Great Slash Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Dash Slash Glow", "Skills Panel").SetActive(true);
            topMenu.GetImage("Cyclone Slash Glow", "Skills Panel").SetActive(true);

            if (!PlayerData.instance.hasDash && !PlayerData.instance.hasShadowDash) topMenu.GetImage("Mothwing Cloak Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasWalljump) topMenu.GetImage("Mantis Claw Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDoubleJump) topMenu.GetImage("Monarch Wings Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasSuperDash) topMenu.GetImage("Crystal Heart Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasAcidArmour) topMenu.GetImage("Isma's Tear Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDreamGate) topMenu.GetImage("Dream Gate Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDreamNail && !PlayerData.instance.dreamNailUpgraded) topMenu.GetImage("Dream Nail Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDashSlash) topMenu.GetImage("Great Slash Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasUpwardSlash) topMenu.GetImage("Dash Slash Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasCyclone) topMenu.GetImage("Cyclone Slash Glow", "Skills Panel").SetActive(false);

            topMenu.GetButton("Scream", "Skills Panel").UpdateText("Scream: " + PlayerData.instance.screamLevel);
            topMenu.GetButton("Fireball", "Skills Panel").UpdateText("Fireball: " + PlayerData.instance.fireballLevel);
            topMenu.GetButton("Quake", "Skills Panel").UpdateText("Quake: " + PlayerData.instance.quakeLevel);
        }

        private void RefreshInfoPanel()
        {
            PlayerData.instance.CountGameCompletion();

            infoPanel.GetText("Hero State").UpdateText(HeroController.instance.hero_state.ToString());
            infoPanel.GetText("Velocity").UpdateText(HeroController.instance.current_velocity.ToString());
            infoPanel.GetText("Nail Damage").UpdateText(DebugMod.refKnightSlash.FsmVariables.GetFsmInt("damageDealt").Value + " (Flat " + PlayerData.instance.nailDamage + ", x" + DebugMod.refKnightSlash.FsmVariables.GetFsmFloat("Multiplier").Value + ")");
            infoPanel.GetText("HP").UpdateText(PlayerData.instance.health + " / " + PlayerData.instance.maxHealth);
            infoPanel.GetText("MP").UpdateText((PlayerData.instance.MPCharge + PlayerData.instance.MPReserve).ToString());

            infoPanel.GetText("Completion").UpdateText(PlayerData.instance.completionPercentage.ToString());
            infoPanel.GetText("Grubs").UpdateText(PlayerData.instance.grubsCollected + " / 46");

            infoPanel.GetText("isInvuln").UpdateText(HeroController.instance.cState.invulnerable.ToString());
            infoPanel.GetText("Invincible").UpdateText(PlayerData.instance.isInvincible.ToString());
            infoPanel.GetText("Invincitest").UpdateText(PlayerData.instance.invinciTest.ToString());
            infoPanel.GetText("Damage State").UpdateText(HeroController.instance.damageMode.ToString());
            infoPanel.GetText("Dead State").UpdateText(HeroController.instance.cState.dead.ToString());
            infoPanel.GetText("Hazard Death").UpdateText(HeroController.instance.cState.hazardDeath.ToString());

            infoPanel.GetText("Scene Name").UpdateText(DebugMod.GetSceneName());
            infoPanel.GetText("Transition").UpdateText(HeroController.instance.cState.transitioning.ToString());

            string transState = HeroController.instance.transitionState.ToString();
            if (transState == "WAITING_TO_ENTER_LEVEL") transState = "LOADING";
            if (transState == "WAITING_TO_TRANSITION") transState = "WAITING";

            infoPanel.GetText("Trans State").UpdateText(transState);
            infoPanel.GetText("is Gameplay").UpdateText(DebugMod.gm.IsGameplayScene().ToString());
            infoPanel.GetText("Game State").UpdateText(GameManager.instance.gameState.ToString());
            infoPanel.GetText("UI State").UpdateText(UIManager.instance.uiState.ToString());
            infoPanel.GetText("Hero Paused").UpdateText(HeroController.instance.cState.isPaused.ToString());
            infoPanel.GetText("Camera Mode").UpdateText(DebugMod.refCamera.mode.ToString());

            infoPanel.GetText("Accept Input").UpdateText(HeroController.instance.acceptingInput.ToString());
            infoPanel.GetText("Relinquished").UpdateText(HeroController.instance.controlReqlinquished.ToString());
            infoPanel.GetText("atBench").UpdateText(PlayerData.instance.atBench.ToString());

            infoPanel.GetText("Dashing").UpdateText(HeroController.instance.cState.dashing.ToString());
            infoPanel.GetText("Jumping").UpdateText((HeroController.instance.cState.jumping || HeroController.instance.cState.doubleJumping).ToString());
            infoPanel.GetText("Superdashing").UpdateText(HeroController.instance.cState.superDashing.ToString());
            infoPanel.GetText("Falling").UpdateText(HeroController.instance.cState.falling.ToString());
            infoPanel.GetText("Hardland").UpdateText(HeroController.instance.cState.willHardLand.ToString());
            infoPanel.GetText("Swimming").UpdateText(HeroController.instance.cState.swimming.ToString());
            infoPanel.GetText("Recoiling").UpdateText(HeroController.instance.cState.recoiling.ToString());

            infoPanel.GetText("Wall lock").UpdateText(HeroController.instance.wallLocked.ToString());
            infoPanel.GetText("Wall jumping").UpdateText(HeroController.instance.cState.wallJumping.ToString());
            infoPanel.GetText("Wall touching").UpdateText(HeroController.instance.cState.touchingWall.ToString());
            infoPanel.GetText("Wall sliding").UpdateText(HeroController.instance.cState.wallSliding.ToString());
            infoPanel.GetText("Wall left").UpdateText(HeroController.instance.touchingWallL.ToString());
            infoPanel.GetText("Wall right").UpdateText(HeroController.instance.touchingWallR.ToString());

            infoPanel.GetText("Attacking").UpdateText(HeroController.instance.cState.attacking.ToString());
            infoPanel.GetText("canCast").UpdateText(HeroController.instance.CanCast().ToString());
            infoPanel.GetText("canSuperdash").UpdateText(HeroController.instance.CanSuperDash().ToString());
            infoPanel.GetText("canQuickmap").UpdateText(HeroController.instance.CanQuickMap().ToString());
            infoPanel.GetText("canInventory").UpdateText(HeroController.instance.CanOpenInventory().ToString());
            infoPanel.GetText("canWarp").UpdateText(DebugMod.refDreamNail.FsmVariables.GetFsmBool("Dream Warp Allowed").Value.ToString());
            infoPanel.GetText("canDGate").UpdateText(DebugMod.refDreamNail.FsmVariables.GetFsmBool("Can Dream Gate").Value.ToString());
            infoPanel.GetText("gateAllow").UpdateText(DebugMod.refDreamNail.FsmVariables.GetFsmBool("Dream Gate Allowed").Value.ToString());
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
            }

            if (trajanBold == null) ModHooks.ModLog("[DEBUG MOD] Could not find game font");

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

        private void HideMenuClicked()
        {
            SetMenusActive(false);
        }

        private void KillAllClicked()
        {
            PlayMakerFSM.BroadcastEvent("INSTA KILL");
            Console.AddLine("INSTA KILL broadcasted!");
        }

        private void SetSpawnClicked()
        {
            HeroController.instance.SetHazardRespawn(DebugMod.refKnight.transform.position, false);
            Console.AddLine("Manual respawn point on this map set to" + DebugMod.refKnight.transform.position.ToString());
        }

        private void RespawnClicked()
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

        private void DumpLogClicked()
        {
            Console.AddLine("Saving console log...");
            Console.SaveHistory();
        }

        private void CheatsClicked()
        {
            topMenu.TogglePanel("Cheats Panel");
        }

        private void CharmsClicked()
        {
            topMenu.TogglePanel("Charms Panel");
        }

        private void SkillsClicked()
        {
            topMenu.TogglePanel("Skills Panel");
            if (topMenu.GetPanel("Skills Panel").active) RefreshSkillsMenu();
        }

        private void ItemsClicked()
        {
            topMenu.TogglePanel("Items Panel");
        }

        private void BossesClicked()
        {
            topMenu.TogglePanel("Bosses Panel");
        }

        private void DreamGatePanelClicked()
        {
            topMenu.TogglePanel("DreamGate Panel");
        }

        private void InfiniteJumpClicked()
        {
            PlayerData.instance.infiniteAirJump = !PlayerData.instance.infiniteAirJump;
            Console.AddLine("Infinite Jump set to " + PlayerData.instance.infiniteAirJump.ToString().ToUpper());

            topMenu.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(PlayerData.instance.infiniteAirJump ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private void InfiniteSoulClicked()
        {
            infiniteSoul = !infiniteSoul;
            Console.AddLine("Infinite SOUL set to " + infiniteSoul.ToString().ToUpper());

            topMenu.GetButton("Infinite Soul", "Cheats Panel").SetTextColor(infiniteSoul ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private void InfiniteHPClicked()
        {
            infiniteHP = !infiniteHP;
            Console.AddLine("Infinite HP set to " + infiniteHP.ToString().ToUpper());

            topMenu.GetButton("Infinite Soul", "Cheats Panel").SetTextColor(infiniteHP ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private void InvincibilityClicked()
        {
            PlayerData.instance.isInvincible = !PlayerData.instance.isInvincible;
            Console.AddLine("Invincibility set to " + PlayerData.instance.isInvincible.ToString().ToUpper());

            topMenu.GetButton("Invincibility", "Cheats Panel").SetTextColor(PlayerData.instance.isInvincible ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);

            playerInvincible = PlayerData.instance.isInvincible;
        }

        private void AllCharmsClicked()
        {
            for (int i = 1; i < 37; i++)
            {
                PlayerData.instance.GetType().GetField("gotCharm_" + i.ToString()).SetValue(PlayerData.instance, true);
            }

            PlayerData.instance.charmSlots = 10;
            PlayerData.instance.hasCharm = true;
            PlayerData.instance.charmsOwned = 36;
            PlayerData.instance.royalCharmState = 4;
            PlayerData.instance.gotKingFragment = true;
            PlayerData.instance.gotQueenFragment = true;
            PlayerData.instance.notchShroomOgres = true;
            PlayerData.instance.notchFogCanyon = true;
            PlayerData.instance.salubraNotch1 = true;
            PlayerData.instance.salubraNotch2 = true;
            PlayerData.instance.salubraNotch3 = true;
            PlayerData.instance.salubraNotch4 = true;

            topMenu.GetButton("Kingsoul", "Charms Panel").UpdateText("Kingsoul: " + PlayerData.instance.royalCharmState);

            Console.AddLine("Added all charms to inventory");
        }

        private void KingsoulClicked()
        {
            if (!PlayerData.instance.gotCharm_36)
            {
                PlayerData.instance.gotCharm_36 = true;
            }

            PlayerData.instance.royalCharmState++;

            if (PlayerData.instance.royalCharmState >= 5)
            {
                PlayerData.instance.royalCharmState = 0;
            }

            topMenu.GetButton("Kingsoul", "Charms Panel").UpdateText("Kingsoul: " + PlayerData.instance.royalCharmState);
        }

        private void FragileHeartFixClicked()
        {
            if (PlayerData.instance.brokenCharm_23)
            {
                PlayerData.instance.brokenCharm_23 = false;
                Console.AddLine("Fixed fragile heart");
            }
        }

        private void FragileGreedFixClicked()
        {
            if (PlayerData.instance.brokenCharm_24)
            {
                PlayerData.instance.brokenCharm_24 = false;
                Console.AddLine("Fixed fragile greed");
            }
        }

        private void FragileStrengthFixClicked()
        {
            if (PlayerData.instance.brokenCharm_25)
            {
                PlayerData.instance.brokenCharm_25 = false;
                Console.AddLine("Fixed fragile strength");
            }
        }

        private void AllSkillsClicked()
        {
            PlayerData.instance.screamLevel = 2;
            PlayerData.instance.fireballLevel = 2;
            PlayerData.instance.quakeLevel = 2;

            PlayerData.instance.hasDash = true;
            PlayerData.instance.canDash = true;
            PlayerData.instance.hasShadowDash = true;
            PlayerData.instance.canShadowDash = true;
            PlayerData.instance.hasWalljump = true;
            PlayerData.instance.canWallJump = true;
            PlayerData.instance.hasDoubleJump = true;
            PlayerData.instance.hasSuperDash = true;
            PlayerData.instance.canSuperDash = true;
            PlayerData.instance.hasAcidArmour = true;

            PlayerData.instance.hasDreamNail = true;
            PlayerData.instance.dreamNailUpgraded = true;
            PlayerData.instance.hasDreamGate = true;

            PlayerData.instance.hasNailArt = true;
            PlayerData.instance.hasCyclone = true;
            PlayerData.instance.hasDashSlash = true;
            PlayerData.instance.hasUpwardSlash = true;

            Console.AddLine("Giving player all skills");

            RefreshSkillsMenu();
        }

        private void ScreamClicked()
        {
            if (PlayerData.instance.screamLevel >= 2)
            {
                PlayerData.instance.screamLevel = 0;
            }
            else
            {
                PlayerData.instance.screamLevel++;
            }

            RefreshSkillsMenu();
        }

        private void FireballClicked()
        {
            if (PlayerData.instance.fireballLevel >= 2)
            {
                PlayerData.instance.fireballLevel = 0;
            }
            else
            {
                PlayerData.instance.fireballLevel++;
            }

            RefreshSkillsMenu();
        }

        private void QuakeClicked()
        {
            if (PlayerData.instance.quakeLevel >= 2)
            {
                PlayerData.instance.quakeLevel = 0;
            }
            else
            {
                PlayerData.instance.quakeLevel++;
            }

            RefreshSkillsMenu();
        }

        private void MothwingCloakClicked()
        {
            if (!PlayerData.instance.hasDash && !PlayerData.instance.hasShadowDash)
            {
                PlayerData.instance.hasDash = true;
                PlayerData.instance.canDash = true;
                Console.AddLine("Giving player Mothwing Cloak");
            }
            else if (PlayerData.instance.hasDash && !PlayerData.instance.hasShadowDash)
            {
                PlayerData.instance.hasShadowDash = true;
                PlayerData.instance.canShadowDash = true;
                Console.AddLine("Giving player Shade Cloak");
            }
            else
            {
                PlayerData.instance.hasDash = false;
                PlayerData.instance.canDash = false;
                PlayerData.instance.hasShadowDash = false;
                PlayerData.instance.canShadowDash = false;
                Console.AddLine("Taking away both dash upgrades");
            }

            RefreshSkillsMenu();
        }

        private void MantisClawClicked()
        {
            if (!PlayerData.instance.hasWalljump)
            {
                PlayerData.instance.hasWalljump = true;
                PlayerData.instance.canWallJump = true;
                Console.AddLine("Giving player Mantis Claw");
            }
            else
            {
                PlayerData.instance.hasWalljump = false;
                PlayerData.instance.canWallJump = false;
                Console.AddLine("Taking away Mantis Claw");
            }

            RefreshSkillsMenu();
        }

        private void MonarchWingsClicked()
        {
            if (!PlayerData.instance.hasDoubleJump)
            {
                PlayerData.instance.hasDoubleJump = true;
                Console.AddLine("Giving player Monarch Wings");
            }
            else
            {
                PlayerData.instance.hasDoubleJump = false;
                Console.AddLine("Taking away Monarch Wings");
            }

            RefreshSkillsMenu();
        }

        private void CrystalHeartClicked()
        {
            if (!PlayerData.instance.hasSuperDash)
            {
                PlayerData.instance.hasSuperDash = true;
                PlayerData.instance.canSuperDash = true;
                Console.AddLine("Giving player Crystal Heart");
            }
            else
            {
                PlayerData.instance.hasSuperDash = false;
                PlayerData.instance.canSuperDash = false;
                Console.AddLine("Taking away Crystal Heart");
            }

            RefreshSkillsMenu();
        }

        private void IsmasTearClicked()
        {
            if (!PlayerData.instance.hasAcidArmour)
            {
                PlayerData.instance.hasAcidArmour = true;
                Console.AddLine("Giving player Isma's Tear");
            }
            else
            {
                PlayerData.instance.hasAcidArmour = false;
                Console.AddLine("Taking away Isma's Tear");
            }

            RefreshSkillsMenu();
        }

        private void DreamNailClicked()
        {
            if (!PlayerData.instance.hasDreamNail && !PlayerData.instance.dreamNailUpgraded)
            {
                PlayerData.instance.hasDreamNail = true;
                Console.AddLine("Giving player Dream Nail");
            }
            else if (PlayerData.instance.hasDreamNail && !PlayerData.instance.dreamNailUpgraded)
            {
                PlayerData.instance.dreamNailUpgraded = true;
                Console.AddLine("Giving player Awoken Dream Nail");
            }
            else
            {
                PlayerData.instance.hasDreamNail = false;
                PlayerData.instance.dreamNailUpgraded = false;
                Console.AddLine("Taking away both Dream Nail upgrades");
            }

            RefreshSkillsMenu();
        }

        private void DreamGateClicked()
        {
            if (!PlayerData.instance.hasDreamNail && !PlayerData.instance.hasDreamGate)
            {
                PlayerData.instance.hasDreamNail = true;
                PlayerData.instance.hasDreamGate = true;
                Console.AddLine("Giving player both Dream Nail and Dream Gate");
            }
            else if (PlayerData.instance.hasDreamNail && !PlayerData.instance.hasDreamGate)
            {
                PlayerData.instance.hasDreamGate = true;
                Console.AddLine("Giving player Dream Gate");
            }
            else
            {
                PlayerData.instance.hasDreamGate = false;
                Console.AddLine("Taking away Dream Gate");
            }

            RefreshSkillsMenu();
        }

        private void GreatSlashClicked()
        {
            if (!PlayerData.instance.hasDashSlash)
            {
                PlayerData.instance.hasDashSlash = true;
                PlayerData.instance.hasNailArt = true;
                Console.AddLine("Giving player Great Slash");
            }
            else
            {
                PlayerData.instance.hasDashSlash = false;
                Console.AddLine("Taking away Great Slash");
            }

            if (!PlayerData.instance.hasUpwardSlash && !PlayerData.instance.hasDashSlash && !PlayerData.instance.hasCyclone) PlayerData.instance.hasNailArt = false;

            RefreshSkillsMenu();
        }

        private void DashSlashClicked()
        {
            if (!PlayerData.instance.hasUpwardSlash)
            {
                PlayerData.instance.hasUpwardSlash = true;
                PlayerData.instance.hasNailArt = true;
                Console.AddLine("Giving player Dash Slash");
            }
            else
            {
                PlayerData.instance.hasUpwardSlash = false;
                Console.AddLine("Taking away Dash Slash");
            }

            if (!PlayerData.instance.hasUpwardSlash && !PlayerData.instance.hasDashSlash && !PlayerData.instance.hasCyclone) PlayerData.instance.hasNailArt = false;

            RefreshSkillsMenu();
        }

        private void CycloneSlashClicked()
        {
            if (!PlayerData.instance.hasCyclone)
            {
                PlayerData.instance.hasCyclone = true;
                PlayerData.instance.hasNailArt = true;
                Console.AddLine("Giving player Cyclone Slash");
            }
            else
            {
                PlayerData.instance.hasCyclone = false;
                Console.AddLine("Taking away Cyclone Slash");
            }

            if (!PlayerData.instance.hasUpwardSlash && !PlayerData.instance.hasDashSlash && !PlayerData.instance.hasCyclone) PlayerData.instance.hasNailArt = false;

            RefreshSkillsMenu();
        }

        public void OnGUI()
        {
            Matrix4x4 matrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
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
            if (showMenus)
            {
                GUI.skin.button.fontStyle = FontStyle.Bold;
                if ((showPanel && GameManager.instance.IsGameplayScene() && UIManager.instance.uiState == UIState.PLAYING) || (GameManager.instance.IsGameplayScene() && !showHelp && UIManager.instance.uiState == UIState.PAUSED))
                {
                    GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.skin.label.fontSize = 18;

                    if (showPanelState >= 1)
                    {
                        int num = Mathf.FloorToInt(Time.realtimeSinceStartup / 60f);
                        int num2 = Mathf.FloorToInt(Time.realtimeSinceStartup - (float)(num * 60));
                        Vector3 position = DebugMod.refKnight.transform.position;
                        GUI.Label(new Rect(10f, 10f, 200f, 25f), "Session  time: " + string.Format("{0:00}:{1:00}", num, num2));
                        GUI.Label(new Rect(10f, 35f, 300f, 25f), "Hero position: " + position.ToString());
                        GUI.Label(new Rect(1720f, 10f, 300f, 120f), ReadInput());
                        GUI.Label(new Rect(910f, 5f, 100f, 25f), "Load: " + DebugMod.GetLoadTime() + "s");
                    }

                    GUILayout.BeginArea(new Rect(7f, 210f, 500f, 790f));
                    GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                    if (showPanelState >= 0 && showPanelState < 2)
                    {
                        GUILayout.Label("CTRL+F2 TO CYCLE BETWEEN TABS", new GUILayoutOption[]
                        {
                            GUILayout.ExpandHeight(true)
                        });
                    }

                    if (showPanelState >= 2)
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

                    if (showPanelState >= 3)
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

                    if (showPanelState >= 4)
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

                    if (showPanelState >= 5)
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

                    if (showPanelState >= 6 && !BossHandler.bossSub)
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
                if ((showButtons && GameManager.instance.IsGameplayScene() && UIManager.instance.uiState == UIState.PLAYING) || (GameManager.instance.IsGameplayScene() && UIManager.instance.uiState == UIState.PAUSED))
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
                        showMenus = false;
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
                    if (charmSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    charmSub = GUILayout.Toggle(charmSub, "CHARMS", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    if (skillSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    skillSub = GUILayout.Toggle(skillSub, "SKILLS", "Button", new GUILayoutOption[]
                    {
                        GUILayout.Height(30f),
                        GUILayout.Width(150f)
                    });
                    GUI.backgroundColor = backgroundColor;
                    if (itemSub)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    itemSub = GUILayout.Toggle(itemSub, "ITEMS", "Button", new GUILayoutOption[]
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
                        infiniteSoul = !infiniteSoul;
                        Console.AddLine("Infinite SOUL set to " + infiniteSoul.ToString().ToUpper());
                    }
                    if (GUILayout.Button("INFINITE HP", new GUILayoutOption[]
                {
                GUILayout.Height(30f),
                GUILayout.Width(150f)
                }))
                    {
                        infiniteHP = !infiniteHP;
                        Console.AddLine("Infinite HP set to " + infiniteHP.ToString().ToUpper());
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
                        manualRespawn = DebugMod.refKnight.transform.position;
                        HeroController.instance.SetHazardRespawn(manualRespawn, false);
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
                        Respawn();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    TextAnchor alignment2 = GUI.skin.label.alignment;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    if (infiniteSoul)
                    {
                        GUI.Label(new Rect(167f, 1045f, 30f, 30f), "<color=lime>✔</color>");
                    }
                    if (PlayerData.instance.infiniteAirJump)
                    {
                        GUI.Label(new Rect(12f, 1045f, 30f, 30f), "<color=lime>✔</color>");
                    }
                    if (infiniteHP)
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
                    if (skillSub)
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
                    if (charmSub)
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
                    if (itemSub)
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

                if (DebugMod.gm.IsGameplayScene() && showHelp)
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
            originalWidth = (float)Screen.width;
            originalHeight = (float)Screen.height;
            scale.x = (float)Screen.width / 1920f;
            scale.y = (float)Screen.height / 1080f;
            scale.z = 1f;
        }

        public void Update()
        {
            if (originalHeight != Screen.height || originalWidth != Screen.width) setMatrix();

            if (DebugMod.GetSceneName() != "Menu_Title")
            {
                if (infiniteSoul && PlayerData.instance.MPCharge < 100 && PlayerData.instance.health > 0 && !HeroController.instance.cState.dead && GameManager.instance.IsGameplayScene())
                {
                    PlayerData.instance.MPCharge = 100;
                }

                if (infiniteHP && !HeroController.instance.cState.dead && GameManager.instance.IsGameplayScene() && PlayerData.instance.health < PlayerData.instance.maxHealth)
                {
                    int amount = PlayerData.instance.maxHealth - PlayerData.instance.health;
                    PlayerData.instance.health = PlayerData.instance.maxHealth;
                    HeroController.instance.AddHealth(amount);
                }

                if (playerInvincible && PlayerData.instance != null)
                {
                    PlayerData.instance.isInvincible = true;
                }

                if (Input.GetKeyUp(KeyCode.Escape) && DebugMod.gm.IsGamePaused())
                {
                    UIManager.instance.TogglePauseGame();
                }

                if (infoPanel.active)
                {
                    RefreshInfoPanel();
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
                    if (showPanel)
                    {
                        showPanel = false;
                    }
                    showHelp = !showHelp;
                }
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    showMenus = !showMenus;
                }
                if (Input.GetKeyUp(KeyCode.F2) && !Input.GetKey(KeyCode.LeftControl))
                {
                    if (showHelp)
                    {
                        showHelp = false;
                    }
                    showPanel = !showPanel;
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F2))
                {
                    if (showPanelState >= 6)
                    {
                        showPanelState = 1;
                    }
                    else
                    {
                        showPanelState++;
                    }
                }
                if (Input.GetKeyUp(KeyCode.F3))
                {
                    showButtons = !showButtons;
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

                    if (showMenus && EnemyController.enemyPanel)
                    {
                        EnemyController.RefreshEnemyList(base.gameObject);
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

                EnemyController.CheckForAutoUpdate();
            }
        }

        public void DrawRect(Rect position, Color color)
        {
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(position, GUIContent.none, textureStyle);
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

        public bool infiniteSoul;

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