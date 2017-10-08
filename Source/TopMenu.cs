using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public static class TopMenu
    {
        private static CanvasPanel panel;

        public static bool visible;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.instance.images["ButtonsMenuBG"], new Vector2(1092f, 25f), Vector2.zero, new Rect(0f, 0f, GUIController.instance.images["ButtonsMenuBG"].width, GUIController.instance.images["ButtonsMenuBG"].height));

            Rect buttonRect = new Rect(0, 0, GUIController.instance.images["ButtonRect"].width, GUIController.instance.images["ButtonRect"].height);

            //Main buttons
            panel.AddButton("Hide Menu", GUIController.instance.images["ButtonRect"], new Vector2(46f, 28f), Vector2.zero, HideMenuClicked, buttonRect, GUIController.instance.trajanBold, "Hide Menu");
            panel.AddButton("Kill All", GUIController.instance.images["ButtonRect"], new Vector2(146f, 28f), Vector2.zero, KillAllClicked, buttonRect, GUIController.instance.trajanBold, "Kill All");
            panel.AddButton("Set Spawn", GUIController.instance.images["ButtonRect"], new Vector2(246f, 28f), Vector2.zero, SetSpawnClicked, buttonRect, GUIController.instance.trajanBold, "Set Spawn");
            panel.AddButton("Respawn", GUIController.instance.images["ButtonRect"], new Vector2(346f, 28f), Vector2.zero, RespawnClicked, buttonRect, GUIController.instance.trajanBold, "Respawn");
            panel.AddButton("Dump Log", GUIController.instance.images["ButtonRect"], new Vector2(446f, 28f), Vector2.zero, DumpLogClicked, buttonRect, GUIController.instance.trajanBold, "Dump Log");
            panel.AddButton("Cheats", GUIController.instance.images["ButtonRect"], new Vector2(46f, 68f), Vector2.zero, CheatsClicked, buttonRect, GUIController.instance.trajanBold, "Cheats");
            panel.AddButton("Charms", GUIController.instance.images["ButtonRect"], new Vector2(146f, 68f), Vector2.zero, CharmsClicked, buttonRect, GUIController.instance.trajanBold, "Charms");
            panel.AddButton("Skills", GUIController.instance.images["ButtonRect"], new Vector2(246f, 68f), Vector2.zero, SkillsClicked, buttonRect, GUIController.instance.trajanBold, "Skills");
            panel.AddButton("Items", GUIController.instance.images["ButtonRect"], new Vector2(346f, 68f), Vector2.zero, ItemsClicked, buttonRect, GUIController.instance.trajanBold, "Items");
            panel.AddButton("Bosses", GUIController.instance.images["ButtonRect"], new Vector2(446f, 68f), Vector2.zero, BossesClicked, buttonRect, GUIController.instance.trajanBold, "Bosses");
            panel.AddButton("DreamGate", GUIController.instance.images["ButtonRect"], new Vector2(546f, 68f), Vector2.zero, DreamGatePanelClicked, buttonRect, GUIController.instance.trajanBold, "DreamGate");

            //Dropdown panels
            panel.AddPanel("Cheats Panel", GUIController.instance.images["DropdownBG"], new Vector2(45f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, 150f));
            panel.AddPanel("Charms Panel", GUIController.instance.images["DropdownBG"], new Vector2(145f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, 180f));
            panel.AddPanel("Skills Panel", GUIController.instance.images["DropdownBG"], new Vector2(245f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, GUIController.instance.images["DropdownBG"].height));
            panel.AddPanel("Items Panel", GUIController.instance.images["DropdownBG"], new Vector2(345f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, GUIController.instance.images["DropdownBG"].height));
            panel.AddPanel("Bosses Panel", GUIController.instance.images["DropdownBG"], new Vector2(445f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, GUIController.instance.images["DropdownBG"].height));
            panel.AddPanel("DreamGate Panel", GUIController.instance.images["DreamGateDropdownBG"], new Vector2(545f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DreamGateDropdownBG"].width, GUIController.instance.images["DreamGateDropdownBG"].height));

            //Cheats panel
            panel.GetPanel("Cheats Panel").AddButton("Infinite Jump", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, InfiniteJumpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Infinite Jump", 10);
            panel.GetPanel("Cheats Panel").AddButton("Infinite Soul", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, InfiniteSoulClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Infinite Soul", 10);
            panel.GetPanel("Cheats Panel").AddButton("Infinite HP", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, InfiniteHPClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Infinite HP", 10);
            panel.GetPanel("Cheats Panel").AddButton("Invincibility", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, InvincibilityClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Invincibility", 10);

            //Charms panel
            panel.GetPanel("Charms Panel").AddButton("All Charms", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllCharmsClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "All Charms", 10);
            panel.GetPanel("Charms Panel").AddButton("Kingsoul", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, KingsoulClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Kingsoul: " + PlayerData.instance.royalCharmState, 10);
            panel.GetPanel("Charms Panel").AddButton("fHeart fix", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, FragileHeartFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "fHeart fix", 10);
            panel.GetPanel("Charms Panel").AddButton("fGreed fix", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, FragileGreedFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "fGreed fix", 10);
            panel.GetPanel("Charms Panel").AddButton("fStrength fix", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, FragileStrengthFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "fStrength fix", 10);

            //Skills panel buttons
            panel.GetPanel("Skills Panel").AddButton("All Skills", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllSkillsClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "All Skills", 10);
            panel.GetPanel("Skills Panel").AddButton("Scream", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, ScreamClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Scream: " + PlayerData.instance.screamLevel, 10);
            panel.GetPanel("Skills Panel").AddButton("Fireball", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, FireballClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Fireball: " + PlayerData.instance.fireballLevel, 10);
            panel.GetPanel("Skills Panel").AddButton("Quake", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, QuakeClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Quake: " + PlayerData.instance.quakeLevel, 10);
            panel.GetPanel("Skills Panel").AddButton("Mothwing Cloak", GUIController.instance.images["MothwingCloak"], new Vector2(5f, 150f), new Vector2(37f, 34f), MothwingCloakClicked, new Rect(0f, 0f, GUIController.instance.images["MothwingCloak"].width, GUIController.instance.images["MothwingCloak"].height));
            panel.GetPanel("Skills Panel").AddButton("Mantis Claw", GUIController.instance.images["MantisClaw"], new Vector2(43f, 150f), new Vector2(37f, 34f), MantisClawClicked, new Rect(0f, 0f, GUIController.instance.images["MantisClaw"].width, GUIController.instance.images["MantisClaw"].height));
            panel.GetPanel("Skills Panel").AddButton("Monarch Wings", GUIController.instance.images["MonarchWings"], new Vector2(5f, 194f), new Vector2(37f, 33f), MonarchWingsClicked, new Rect(0f, 0f, GUIController.instance.images["MonarchWings"].width, GUIController.instance.images["MonarchWings"].height));
            panel.GetPanel("Skills Panel").AddButton("Crystal Heart", GUIController.instance.images["CrystalHeart"], new Vector2(43f, 194f), new Vector2(37f, 34f), CrystalHeartClicked, new Rect(0f, 0f, GUIController.instance.images["CrystalHeart"].width, GUIController.instance.images["CrystalHeart"].height));
            panel.GetPanel("Skills Panel").AddButton("Isma's Tear", GUIController.instance.images["IsmasTear"], new Vector2(5f, 238f), new Vector2(37f, 40f), IsmasTearClicked, new Rect(0f, 0f, GUIController.instance.images["IsmasTear"].width, GUIController.instance.images["IsmasTear"].height));
            panel.GetPanel("Skills Panel").AddButton("Dream Nail", GUIController.instance.images["DreamNail1"], new Vector2(43f, 251f), new Vector2(37f, 59f), DreamNailClicked, new Rect(0f, 0f, GUIController.instance.images["DreamNail1"].width, GUIController.instance.images["DreamNail1"].height));
            panel.GetPanel("Skills Panel").AddButton("Dream Gate", GUIController.instance.images["DreamGate"], new Vector2(5f, 288f), new Vector2(37f, 36f), DreamGateClicked, new Rect(0f, 0f, GUIController.instance.images["DreamGate"].width, GUIController.instance.images["DreamGate"].height));
            panel.GetPanel("Skills Panel").AddButton("Great Slash", GUIController.instance.images["NailArt_GreatSlash"], new Vector2(5f, 329f), new Vector2(23f, 23f), GreatSlashClicked, new Rect(0f, 0f, GUIController.instance.images["NailArt_GreatSlash"].width, GUIController.instance.images["NailArt_GreatSlash"].height));
            panel.GetPanel("Skills Panel").AddButton("Dash Slash", GUIController.instance.images["NailArt_DashSlash"], new Vector2(33f, 329f), new Vector2(23f, 23f), DashSlashClicked, new Rect(0f, 0f, GUIController.instance.images["NailArt_DashSlash"].width, GUIController.instance.images["NailArt_DashSlash"].height));
            panel.GetPanel("Skills Panel").AddButton("Cyclone Slash", GUIController.instance.images["NailArt_CycloneSlash"], new Vector2(61f, 329f), new Vector2(23f, 23f), CycloneSlashClicked, new Rect(0f, 0f, GUIController.instance.images["NailArt_CycloneSlash"].width, GUIController.instance.images["NailArt_CycloneSlash"].height));

            //Skills panel button glow
            panel.GetPanel("Skills Panel").AddImage("Mothwing Cloak Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 145f), new Vector2(47f, 44f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Mantis Claw Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 145f), new Vector2(47f, 44f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Monarch Wings Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 189f), new Vector2(47f, 43f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Crystal Heart Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 189f), new Vector2(47f, 44f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Isma's Tear Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 233f), new Vector2(47f, 50f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Dream Nail Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 246f), new Vector2(47f, 69f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Dream Gate Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 283f), new Vector2(47f, 46f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Great Slash Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Dash Slash Glow", GUIController.instance.images["BlueGlow"], new Vector2(28f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Cyclone Slash Glow", GUIController.instance.images["BlueGlow"], new Vector2(56f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));

            panel.FixRenderOrder();
        }

        public static void Update()
        {
            if (panel == null)
            {
                return;
            }

            if (visible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!visible && panel.active)
            {
                panel.SetActive(false, true);
            }

            if (panel.GetPanel("Skills Panel").active) RefreshSkillsMenu();

            if (panel.GetPanel("Cheats Panel").active) panel.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(PlayerData.instance.infiniteAirJump ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);

        }

        private static void RefreshSkillsMenu()
        {
            if (PlayerData.instance.dreamNailUpgraded) panel.GetButton("Dream Nail", "Skills Panel").UpdateSprite(GUIController.instance.images["DreamNail2"], new Rect(0f, 0f, GUIController.instance.images["DreamNail2"].width, GUIController.instance.images["DreamNail2"].height));
            else panel.GetButton("Dream Nail", "Skills Panel").UpdateSprite(GUIController.instance.images["DreamNail1"], new Rect(0f, 0f, GUIController.instance.images["DreamNail1"].width, GUIController.instance.images["DreamNail1"].height));
            if (PlayerData.instance.hasShadowDash) panel.GetButton("Mothwing Cloak", "Skills Panel").UpdateSprite(GUIController.instance.images["ShadeCloak"], new Rect(0f, 0f, GUIController.instance.images["ShadeCloak"].width, GUIController.instance.images["ShadeCloak"].height));
            else panel.GetButton("Mothwing Cloak", "Skills Panel").UpdateSprite(GUIController.instance.images["MothwingCloak"], new Rect(0f, 0f, GUIController.instance.images["MothwingCloak"].width, GUIController.instance.images["MothwingCloak"].height));

            panel.GetImage("Mothwing Cloak Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Mantis Claw Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Monarch Wings Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Crystal Heart Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Isma's Tear Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Dream Gate Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Dream Nail Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Great Slash Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Dash Slash Glow", "Skills Panel").SetActive(true);
            panel.GetImage("Cyclone Slash Glow", "Skills Panel").SetActive(true);

            if (!PlayerData.instance.hasDash && !PlayerData.instance.hasShadowDash) panel.GetImage("Mothwing Cloak Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasWalljump) panel.GetImage("Mantis Claw Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDoubleJump) panel.GetImage("Monarch Wings Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasSuperDash) panel.GetImage("Crystal Heart Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasAcidArmour) panel.GetImage("Isma's Tear Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDreamGate) panel.GetImage("Dream Gate Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDreamNail && !PlayerData.instance.dreamNailUpgraded) panel.GetImage("Dream Nail Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasDashSlash) panel.GetImage("Great Slash Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasUpwardSlash) panel.GetImage("Dash Slash Glow", "Skills Panel").SetActive(false);
            if (!PlayerData.instance.hasCyclone) panel.GetImage("Cyclone Slash Glow", "Skills Panel").SetActive(false);

            panel.GetButton("Scream", "Skills Panel").UpdateText("Scream: " + PlayerData.instance.screamLevel);
            panel.GetButton("Fireball", "Skills Panel").UpdateText("Fireball: " + PlayerData.instance.fireballLevel);
            panel.GetButton("Quake", "Skills Panel").UpdateText("Quake: " + PlayerData.instance.quakeLevel);
        }

        private static void HideMenuClicked(string buttonName)
        {
            GUIController.instance.SetMenusActive(false);
        }

        private static void KillAllClicked(string buttonName)
        {
            PlayMakerFSM.BroadcastEvent("INSTA KILL");
            Console.AddLine("INSTA KILL broadcasted!");
        }

        private static void SetSpawnClicked(string buttonName)
        {
            HeroController.instance.SetHazardRespawn(DebugMod.refKnight.transform.position, false);
            Console.AddLine("Manual respawn point on this map set to" + DebugMod.refKnight.transform.position.ToString());
        }

        private static void RespawnClicked(string buttonName)
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

        private static void DumpLogClicked(string buttonName)
        {
            Console.AddLine("Saving console log...");
            Console.SaveHistory();
        }

        private static void CheatsClicked(string buttonName)
        {
            panel.TogglePanel("Cheats Panel");
        }

        private static void CharmsClicked(string buttonName)
        {
            panel.TogglePanel("Charms Panel");
        }

        private static void SkillsClicked(string buttonName)
        {
            panel.TogglePanel("Skills Panel");
            if (panel.GetPanel("Skills Panel").active) RefreshSkillsMenu();
        }

        private static void ItemsClicked(string buttonName)
        {
            panel.TogglePanel("Items Panel");
        }

        private static void BossesClicked(string buttonName)
        {
            panel.TogglePanel("Bosses Panel");
        }

        private static void DreamGatePanelClicked(string buttonName)
        {
            panel.TogglePanel("DreamGate Panel");
        }

        private static void InfiniteJumpClicked(string buttonName)
        {
            PlayerData.instance.infiniteAirJump = !PlayerData.instance.infiniteAirJump;
            Console.AddLine("Infinite Jump set to " + PlayerData.instance.infiniteAirJump.ToString().ToUpper());

            panel.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(PlayerData.instance.infiniteAirJump ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private static void InfiniteSoulClicked(string buttonName)
        {
            DebugMod.infiniteSoul = !DebugMod.infiniteSoul;
            Console.AddLine("Infinite SOUL set to " + DebugMod.infiniteSoul.ToString().ToUpper());

            panel.GetButton("Infinite Soul", "Cheats Panel").SetTextColor(DebugMod.infiniteSoul ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private static void InfiniteHPClicked(string buttonName)
        {
            DebugMod.infiniteHP = !DebugMod.infiniteHP;
            Console.AddLine("Infinite HP set to " + DebugMod.infiniteHP.ToString().ToUpper());

            panel.GetButton("Infinite HP", "Cheats Panel").SetTextColor(DebugMod.infiniteHP ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private static void InvincibilityClicked(string buttonName)
        {
            PlayerData.instance.isInvincible = !PlayerData.instance.isInvincible;
            Console.AddLine("Invincibility set to " + PlayerData.instance.isInvincible.ToString().ToUpper());

            panel.GetButton("Invincibility", "Cheats Panel").SetTextColor(PlayerData.instance.isInvincible ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);

            DebugMod.playerInvincible = PlayerData.instance.isInvincible;
        }

        private static void AllCharmsClicked(string buttonName)
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

            panel.GetButton("Kingsoul", "Charms Panel").UpdateText("Kingsoul: " + PlayerData.instance.royalCharmState);

            Console.AddLine("Added all charms to inventory");
        }

        private static void KingsoulClicked(string buttonName)
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

            panel.GetButton("Kingsoul", "Charms Panel").UpdateText("Kingsoul: " + PlayerData.instance.royalCharmState);
        }

        private static void FragileHeartFixClicked(string buttonName)
        {
            if (PlayerData.instance.brokenCharm_23)
            {
                PlayerData.instance.brokenCharm_23 = false;
                Console.AddLine("Fixed fragile heart");
            }
        }

        private static void FragileGreedFixClicked(string buttonName)
        {
            if (PlayerData.instance.brokenCharm_24)
            {
                PlayerData.instance.brokenCharm_24 = false;
                Console.AddLine("Fixed fragile greed");
            }
        }

        private static void FragileStrengthFixClicked(string buttonName)
        {
            if (PlayerData.instance.brokenCharm_25)
            {
                PlayerData.instance.brokenCharm_25 = false;
                Console.AddLine("Fixed fragile strength");
            }
        }

        private static void AllSkillsClicked(string buttonName)
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

        private static void ScreamClicked(string buttonName)
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

        private static void FireballClicked(string buttonName)
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

        private static void QuakeClicked(string buttonName)
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

        private static void MothwingCloakClicked(string buttonName)
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

        private static void MantisClawClicked(string buttonName)
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

        private static void MonarchWingsClicked(string buttonName)
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

        private static void CrystalHeartClicked(string buttonName)
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

        private static void IsmasTearClicked(string buttonName)
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

        private static void DreamNailClicked(string buttonName)
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

        private static void DreamGateClicked(string buttonName)
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

        private static void GreatSlashClicked(string buttonName)
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

        private static void DashSlashClicked(string buttonName)
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

        private static void CycloneSlashClicked(string buttonName)
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
    }
}
