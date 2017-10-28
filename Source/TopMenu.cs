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
            panel.AddPanel("Cheats Panel", GUIController.instance.images["DropdownBG"], new Vector2(45f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, 210f));
            panel.AddPanel("Charms Panel", GUIController.instance.images["DropdownBG"], new Vector2(145f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, 240f));
            panel.AddPanel("Skills Panel", GUIController.instance.images["DropdownBG"], new Vector2(245f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, GUIController.instance.images["DropdownBG"].height));
            panel.AddPanel("Items Panel", GUIController.instance.images["DropdownBG"], new Vector2(345f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, GUIController.instance.images["DropdownBG"].height));
            panel.AddPanel("Bosses Panel", GUIController.instance.images["DropdownBG"], new Vector2(445f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DropdownBG"].width, 200f));
            panel.AddPanel("DreamGate Panel", GUIController.instance.images["DreamGateDropdownBG"], new Vector2(545f, 75f), Vector2.zero, new Rect(0, 0, GUIController.instance.images["DreamGateDropdownBG"].width, GUIController.instance.images["DreamGateDropdownBG"].height));

            //Cheats panel
            panel.GetPanel("Cheats Panel").AddButton("Infinite Jump", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, InfiniteJumpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Infinite Jump", 10);
            panel.GetPanel("Cheats Panel").AddButton("Infinite Soul", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, InfiniteSoulClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Infinite Soul", 10);
            panel.GetPanel("Cheats Panel").AddButton("Infinite HP", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, InfiniteHPClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Infinite HP", 10);
            panel.GetPanel("Cheats Panel").AddButton("Invincibility", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, InvincibilityClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Invincibility", 10);
            panel.GetPanel("Cheats Panel").AddButton("Noclip", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, NoclipClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Noclip", 10);
            panel.GetPanel("Cheats Panel").AddButton("Kill Self", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 180f), Vector2.zero, KillSelfClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Kill Self", 10);


            //Charms panel
            panel.GetPanel("Charms Panel").AddButton("All Charms", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllCharmsClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "All Charms", 10);
            panel.GetPanel("Charms Panel").AddButton("Kingsoul", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, KingsoulClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Kingsoul: " + PlayerData.instance.royalCharmState, 10);
            panel.GetPanel("Charms Panel").AddButton("Grimmchild", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, GrimmchildClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Grimmchild: -", 9);
            panel.GetPanel("Charms Panel").AddButton("fHeart fix", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, FragileHeartFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "fHeart fix", 10);
            panel.GetPanel("Charms Panel").AddButton("fGreed fix", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, FragileGreedFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "fGreed fix", 10);
            panel.GetPanel("Charms Panel").AddButton("fStrength fix", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 180f), Vector2.zero, FragileStrengthFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "fStrength fix", 10);
            panel.GetPanel("Charms Panel").AddButton("Overcharm", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 210f), Vector2.zero, OvercharmClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Overcharm", 10);

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

            //Items panel
            panel.GetPanel("Items Panel").AddButton("Pale Ore", GUIController.instance.images["PaleOre"], new Vector2(5f, 30f), new Vector2(23f, 22f), PaleOreClicked, new Rect(0, 0, GUIController.instance.images["PaleOre"].width, GUIController.instance.images["PaleOre"].height));
            panel.GetPanel("Items Panel").AddButton("Simple Key", GUIController.instance.images["SimpleKey"], new Vector2(33f, 30f), new Vector2(23f, 23f), SimpleKeyClicked, new Rect(0, 0, GUIController.instance.images["SimpleKey"].width, GUIController.instance.images["SimpleKey"].height));
            panel.GetPanel("Items Panel").AddButton("Rancid Egg", GUIController.instance.images["RancidEgg"], new Vector2(61f, 30f), new Vector2(23f, 30f), RancidEggClicked, new Rect(0, 0, GUIController.instance.images["RancidEgg"].width, GUIController.instance.images["RancidEgg"].height));
            panel.GetPanel("Items Panel").AddButton("Geo", GUIController.instance.images["Geo"], new Vector2(5f, 63f), new Vector2(23f, 23f), GeoClicked, new Rect(0, 0, GUIController.instance.images["Geo"].width, GUIController.instance.images["Geo"].height));
            panel.GetPanel("Items Panel").AddButton("Essence", GUIController.instance.images["Essence"], new Vector2(33f, 63f), new Vector2(23f, 23f), EssenceClicked, new Rect(0, 0, GUIController.instance.images["Essence"].width, GUIController.instance.images["Essence"].height));
            panel.GetPanel("Items Panel").AddButton("Lantern", GUIController.instance.images["Lantern"], new Vector2(5f, 96f), new Vector2(37f, 41f), LanternClicked, new Rect(0, 0, GUIController.instance.images["Lantern"].width, GUIController.instance.images["Lantern"].height));
            panel.GetPanel("Items Panel").AddButton("Tram Pass", GUIController.instance.images["TramPass"], new Vector2(43f, 96f), new Vector2(37f, 27f), TramPassClicked, new Rect(0, 0, GUIController.instance.images["TramPass"].width, GUIController.instance.images["TramPass"].height));
            panel.GetPanel("Items Panel").AddButton("Map & Quill", GUIController.instance.images["MapQuill"], new Vector2(5f, 147f), new Vector2(37f, 30f), MapQuillClicked, new Rect(0, 0, GUIController.instance.images["MapQuill"].width, GUIController.instance.images["MapQuill"].height));
            panel.GetPanel("Items Panel").AddButton("City Crest", GUIController.instance.images["CityKey"], new Vector2(43f, 147f), new Vector2(37f, 50f), CityKeyClicked, new Rect(0, 0, GUIController.instance.images["CityKey"].width, GUIController.instance.images["CityKey"].height));
            panel.GetPanel("Items Panel").AddButton("Sly Key", GUIController.instance.images["SlyKey"], new Vector2(5f, 207f), new Vector2(37f, 39f), SlyKeyClicked, new Rect(0, 0, GUIController.instance.images["SlyKey"].width, GUIController.instance.images["SlyKey"].height));
            panel.GetPanel("Items Panel").AddButton("Elegant Key", GUIController.instance.images["ElegantKey"], new Vector2(43f, 207f), new Vector2(37f, 36f), ElegantKeyClicked, new Rect(0, 0, GUIController.instance.images["ElegantKey"].width, GUIController.instance.images["ElegantKey"].height));
            panel.GetPanel("Items Panel").AddButton("Love Key", GUIController.instance.images["LoveKey"], new Vector2(5f, 256f), new Vector2(37f, 36f), LoveKeyClicked, new Rect(0, 0, GUIController.instance.images["LoveKey"].width, GUIController.instance.images["LoveKey"].height));
            panel.GetPanel("Items Panel").AddButton("King's Brand", GUIController.instance.images["Kingsbrand"], new Vector2(43f, 256f), new Vector2(37f, 35f), KingsbrandClicked, new Rect(0, 0, GUIController.instance.images["Kingsbrand"].width, GUIController.instance.images["Kingsbrand"].height));
            panel.GetPanel("Items Panel").AddButton("Bullshit Flower", GUIController.instance.images["Flower"], new Vector2(5f, 302f), new Vector2(37f, 35f), FlowerClicked, new Rect(0, 0, GUIController.instance.images["Flower"].width, GUIController.instance.images["Flower"].height));
            
            //Items panel button glow
            panel.GetPanel("Items Panel").AddImage("Lantern Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 91f), new Vector2(47f, 51f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Tram Pass Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 91f), new Vector2(47f, 37f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Map & Quill Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 142f), new Vector2(47f, 40f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("City Crest Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 142f), new Vector2(47f, 60f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Sly Key Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 202f), new Vector2(47f, 49f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Elegant Key Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 202f), new Vector2(47f, 46f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Love Key Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 251f), new Vector2(47f, 46f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("King's Brand Glow", GUIController.instance.images["BlueGlow"], new Vector2(38f, 251f), new Vector2(47f, 45f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Bullshit Flower Glow", GUIController.instance.images["BlueGlow"], new Vector2(0f, 297f), new Vector2(47f, 45f), new Rect(0f, 0f, GUIController.instance.images["BlueGlow"].width, GUIController.instance.images["BlueGlow"].height));

            //Boss panel
            panel.GetPanel("Bosses Panel").AddButton("Respawn Boss", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, RespawnBossClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Respawn Boss", 10);
            panel.GetPanel("Bosses Panel").AddButton("Respawn Ghost", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 50f), Vector2.zero, RespawnGhostClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Respawn Ghost", 9);

            panel.GetPanel("Bosses Panel").AddButton("Failed Champ", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 110f), Vector2.zero, FailedChampClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Failed Champ", 10);
            panel.GetPanel("Bosses Panel").AddButton("Soul Tyrant", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 130f), Vector2.zero, SoulTyrantClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Soul Tyrant", 10);
            panel.GetPanel("Bosses Panel").AddButton("Lost Kin", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, LostKinClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Lost Kin", 10);
            panel.GetPanel("Bosses Panel").AddButton("NK Grimm", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 180f), Vector2.zero, NKGrimmClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "NK Grimm", 10);

            //Dream gate left panel
            panel.GetPanel("DreamGate Panel").AddButton("Read Data", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, ReadDataClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Read Data", 10);
            panel.GetPanel("DreamGate Panel").AddButton("Save Data", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 50f), Vector2.zero, SaveDataClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Save Data", 10);
            panel.GetPanel("DreamGate Panel").AddButton("Delete Item", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 70f), Vector2.zero, DeleteItemClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Delete Item", 10);
            panel.GetPanel("DreamGate Panel").AddButton("Add Item", GUIController.instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, AddItemClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.trajanNormal, "Add Item", 10);

            //Dream gate right panel
            panel.GetPanel("DreamGate Panel").AddButton("Right1", GUIController.instance.images["ButtonRectEmpty"], new Vector2(90f, 30f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right2", GUIController.instance.images["ButtonRectEmpty"], new Vector2(90f, 50f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right3", GUIController.instance.images["ButtonRectEmpty"], new Vector2(90f, 70f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right4", GUIController.instance.images["ButtonRectEmpty"], new Vector2(90f, 90f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right5", GUIController.instance.images["ButtonRectEmpty"], new Vector2(90f, 110f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right6", GUIController.instance.images["ButtonRectEmpty"], new Vector2(90f, 130f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.instance.arial, "");

            //Dream gate scroll
            panel.GetPanel("DreamGate Panel").AddButton("Scroll Up", GUIController.instance.images["ScrollBarArrowUp"], new Vector2(180f, 30f), Vector2.zero, ScrollUpClicked, new Rect(0f, 0f, GUIController.instance.images["ScrollBarArrowUp"].width, GUIController.instance.images["ScrollBarArrowUp"].height));
            panel.GetPanel("DreamGate Panel").AddButton("Scroll Down", GUIController.instance.images["ScrollBarArrowDown"], new Vector2(180f, 130f), Vector2.zero, ScrollDownClicked, new Rect(0f, 0f, GUIController.instance.images["ScrollBarArrowDown"].width, GUIController.instance.images["ScrollBarArrowDown"].height));

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

            if (panel.GetPanel("Items Panel").active) RefreshItemsMenu();

            if (panel.GetPanel("Charms Panel").active)
            {
                panel.GetButton("Overcharm", "Charms Panel").SetTextColor(PlayerData.instance.overcharmed ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Kingsoul", "Charms Panel").UpdateText("Kingsoul: " + PlayerData.instance.royalCharmState);

                if (DebugMod.GrimmTroupe())
                {
                    panel.GetButton("Grimmchild", "Charms Panel").UpdateText("Grimmchild: " + PlayerData.instance.GetIntInternal("grimmChildLevel"));
                }
            }

            if (panel.GetPanel("Cheats Panel").active) panel.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(PlayerData.instance.infiniteAirJump ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);

            if (panel.GetPanel("Bosses Panel").active)
            {
                panel.GetButton("Failed Champ", "Bosses Panel").SetTextColor(PlayerData.instance.falseKnightDreamDefeated ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Soul Tyrant", "Bosses Panel").SetTextColor(PlayerData.instance.mageLordDreamDefeated ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Lost Kin", "Bosses Panel").SetTextColor(PlayerData.instance.infectedKnightDreamDefeated ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);

                if (DebugMod.GrimmTroupe())
                {
                    panel.GetButton("NK Grimm", "Bosses Panel").SetTextColor((PlayerData.instance.GetBoolInternal("killedNightmareGrimm") || PlayerData.instance.GetBoolInternal("destroyedNightmareLantern")) ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                }
            }
            if (panel.GetPanel("DreamGate Panel").active)
            {
                panel.GetPanel("DreamGate Panel").GetButton("Delete Item").SetTextColor(DreamGate.delMenu ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);

                panel.GetPanel("DreamGate Panel").GetButton("Right1").UpdateText("");
                panel.GetPanel("DreamGate Panel").GetButton("Right2").UpdateText("");
                panel.GetPanel("DreamGate Panel").GetButton("Right3").UpdateText("");
                panel.GetPanel("DreamGate Panel").GetButton("Right4").UpdateText("");
                panel.GetPanel("DreamGate Panel").GetButton("Right5").UpdateText("");
                panel.GetPanel("DreamGate Panel").GetButton("Right6").UpdateText("");

                int i = 0;
                int buttonNum = 1;

                foreach (string entryName in DreamGate.DGData.Keys)
                {
                    if (i >= DreamGate.scrollPosition)
                    {
                        panel.GetPanel("DreamGate Panel").GetButton("Right" + buttonNum).UpdateText(entryName);
                        buttonNum++;
                        if (buttonNum > 6)
                        {
                            break;
                        }
                    }

                    i++;
                }
            }
        }

        private static void RefreshItemsMenu()
        {
            panel.GetImage("Lantern Glow", "Items Panel").SetActive(true);
            panel.GetImage("Tram Pass Glow", "Items Panel").SetActive(true);
            panel.GetImage("Map & Quill Glow", "Items Panel").SetActive(true);
            panel.GetImage("City Crest Glow", "Items Panel").SetActive(true);
            panel.GetImage("Sly Key Glow", "Items Panel").SetActive(true);
            panel.GetImage("Elegant Key Glow", "Items Panel").SetActive(true);
            panel.GetImage("Love Key Glow", "Items Panel").SetActive(true);
            panel.GetImage("King's Brand Glow", "Items Panel").SetActive(true);
            panel.GetImage("Bullshit Flower Glow", "Items Panel").SetActive(true);

            if (!PlayerData.instance.hasLantern) panel.GetImage("Lantern Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasTramPass) panel.GetImage("Tram Pass Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasQuill) panel.GetImage("Map & Quill Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasCityKey) panel.GetImage("City Crest Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasSlykey) panel.GetImage("Sly Key Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasWhiteKey) panel.GetImage("Elegant Key Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasLoveKey) panel.GetImage("Love Key Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasKingsBrand) panel.GetImage("King's Brand Glow", "Items Panel").SetActive(false);
            if (!PlayerData.instance.hasXunFlower || PlayerData.instance.xunFlowerBroken) panel.GetImage("Bullshit Flower Glow", "Items Panel").SetActive(false);
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

        private static void NoclipClicked(string buttonName)
        {
            DebugMod.noclip = !DebugMod.noclip;

            if (DebugMod.noclip)
            {
                Console.AddLine("Enabled noclip");
                DebugMod.noclipPos = DebugMod.refKnight.transform.position;
            }
            else
            {
                Console.AddLine("Disabled noclip");
            }

            panel.GetButton("Noclip", "Cheats Panel").SetTextColor(DebugMod.noclip ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
        }

        private static void KillSelfClicked(string buttonName)
        {
            if (DebugMod.gm.isPaused) UIManager.instance.TogglePauseGame();
            HeroController.instance.TakeHealth(9999);
            HeroController.instance.heroDeathPrefab.SetActive(true);
            DebugMod.gm.ReadyForRespawn();
            GameCameras.instance.hudCanvas.gameObject.SetActive(false);
            GameCameras.instance.hudCanvas.gameObject.SetActive(true);
        }

        public static void UpdateButtonColors()
        {
            panel.GetButton("Noclip", "Cheats Panel").SetTextColor(Color.white);
            panel.GetButton("Invincibility", "Cheats Panel").SetTextColor(Color.white);
            panel.GetButton("Infinite HP", "Cheats Panel").SetTextColor(Color.white);
            panel.GetButton("Infinite Soul", "Cheats Panel").SetTextColor(Color.white);
            panel.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(Color.white);
        }

        private static void AllCharmsClicked(string buttonName)
        {
            for (int i = 1; i <= 40; i++)
            {
                PlayerData.instance.SetBoolInternal("gotCharm_" + i, true);

                if (i == 36 && !DebugMod.GrimmTroupe())
                {
                    break;
                }
            }

            PlayerData.instance.charmSlots = 10;
            PlayerData.instance.hasCharm = true;
            PlayerData.instance.charmsOwned = 40;
            PlayerData.instance.royalCharmState = 4;
            PlayerData.instance.gotKingFragment = true;
            PlayerData.instance.gotQueenFragment = true;
            PlayerData.instance.notchShroomOgres = true;
            PlayerData.instance.notchFogCanyon = true;
            PlayerData.instance.colosseumBronzeOpened = true;
            PlayerData.instance.colosseumBronzeCompleted = true;
            PlayerData.instance.salubraNotch1 = true;
            PlayerData.instance.salubraNotch2 = true;
            PlayerData.instance.salubraNotch3 = true;
            PlayerData.instance.salubraNotch4 = true;

            if (DebugMod.GrimmTroupe())
            {
                PlayerData.instance.SetBoolInternal("fragileGreed_unbreakable", true);
                PlayerData.instance.SetBoolInternal("fragileHealth_unbreakable", true);
                PlayerData.instance.SetBoolInternal("fragileStrength_unbreakable", true);
                PlayerData.instance.SetIntInternal("grimmChildLevel", 5);
                PlayerData.instance.charmSlots = 11;
            }

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

        private static void OvercharmClicked(string buttonName)
        {
            PlayerData.instance.canOvercharm = true;
            PlayerData.instance.overcharmed = !PlayerData.instance.overcharmed;

            Console.AddLine("Set overcharmed: " + PlayerData.instance.overcharmed);
        }

        private static void GrimmchildClicked(string buttonName)
        {
            if (!DebugMod.GrimmTroupe())
            {
                Console.AddLine("Grimmchild does not exist on this patch");
                return;
            }

            if (!PlayerData.instance.GetBoolInternal("gotCharm_40"))
            {
                PlayerData.instance.SetBoolInternal("gotCharm_40", true);
            }

            PlayerData.instance.SetIntInternal("grimmChildLevel", PlayerData.instance.GetIntInternal("grimmChildLevel") + 1);

            if (PlayerData.instance.GetIntInternal("grimmChildLevel") >= 6)
            {
                PlayerData.instance.SetIntInternal("grimmChildLevel", 0);
            }

            panel.GetButton("Kingsoul", "Charms Panel").UpdateText("Kingsoul: " + PlayerData.instance.royalCharmState);
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
                FSMUtility.LocateFSM(DebugMod.refKnight, "Dream Nail").FsmVariables.GetFsmBool("Dream Warp Allowed").Value = true;
                Console.AddLine("Giving player both Dream Nail and Dream Gate");
            }
            else if (PlayerData.instance.hasDreamNail && !PlayerData.instance.hasDreamGate)
            {
                PlayerData.instance.hasDreamGate = true;
                FSMUtility.LocateFSM(DebugMod.refKnight, "Dream Nail").FsmVariables.GetFsmBool("Dream Warp Allowed").Value = true;
                Console.AddLine("Giving player Dream Gate");
            }
            else
            {
                PlayerData.instance.hasDreamGate = false;
                FSMUtility.LocateFSM(DebugMod.refKnight, "Dream Nail").FsmVariables.GetFsmBool("Dream Warp Allowed").Value = false;
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

        private static void RespawnGhostClicked(string buttonName)
        {
            BossHandler.RespawnGhost();
        }

        private static void RespawnBossClicked(string buttonName)
        {
            BossHandler.RespawnBoss();
        }

        private static void FailedChampClicked(string buttonName)
        {
            PlayerData.instance.falseKnightDreamDefeated = !PlayerData.instance.falseKnightDreamDefeated;

            Console.AddLine("Set Failed Champion killed: " + PlayerData.instance.falseKnightDreamDefeated);
        }

        private static void SoulTyrantClicked(string buttonName)
        {
            PlayerData.instance.mageLordDreamDefeated = !PlayerData.instance.mageLordDreamDefeated;

            Console.AddLine("Set Soul Tyrant killed: " + PlayerData.instance.mageLordDreamDefeated);
        }

        private static void LostKinClicked(string buttonName)
        {
            PlayerData.instance.infectedKnightDreamDefeated = !PlayerData.instance.infectedKnightDreamDefeated;

            Console.AddLine("Set Lost Kin killed: " + PlayerData.instance.infectedKnightDreamDefeated);
        }

        private static void NKGrimmClicked(string buttonName)
        {
            if (!DebugMod.GrimmTroupe())
            {
                Console.AddLine("Nightmare King Grimm does not exist on this patch");
                return;
            }

            if (PlayerData.instance.GetBoolInternal("killedNightmareGrimm") || PlayerData.instance.GetBoolInternal("destroyedNightmareLantern"))
            {
                PlayerData.instance.SetBoolInternal("troupeInTown", true);
                PlayerData.instance.SetBoolInternal("killedNightmareGrimm", false);
                PlayerData.instance.SetBoolInternal("destroyedNightmareLantern", false);
                PlayerData.instance.SetIntInternal("grimmChildLevel", 3);
                PlayerData.instance.SetIntInternal("flamesCollected", 3);
                PlayerData.instance.SetBoolInternal("grimmchildAwoken", false);
                PlayerData.instance.SetBoolInternal("metGrimm", true);
                PlayerData.instance.SetBoolInternal("foughtGrimm", true);
                PlayerData.instance.SetBoolInternal("killedGrimm", true);
            }
            else
            {
                PlayerData.instance.SetBoolInternal("troupeInTown", false);
                PlayerData.instance.SetBoolInternal("killedNightmareGrimm", true);
            }

            Console.AddLine("Set Nightmare King Grimm killed: " + PlayerData.instance.GetBoolInternal("killedNightmareGrimm"));
        }

        private static void PaleOreClicked(string buttonName)
        {
            PlayerData.instance.ore = 6;
            Console.AddLine("Set player pale ore to 6");
        }

        private static void SimpleKeyClicked(string buttonName)
        {
            PlayerData.instance.simpleKeys = 3;
            Console.AddLine("Set player simple keys to 3");
        }

        private static void RancidEggClicked(string buttonName)
        {
            PlayerData.instance.rancidEggs += 10;
            Console.AddLine("Giving player 10 rancid eggs");
        }

        private static void GeoClicked(string buttonName)
        {
            HeroController.instance.AddGeo(1000);
            Console.AddLine("Giving player 1000 geo");
        }

        private static void EssenceClicked(string buttonName)
        {
            PlayerData.instance.dreamOrbs += 100;
            Console.AddLine("Giving player 100 essence");
        }

        private static void LanternClicked(string buttonName)
        {
            if (!PlayerData.instance.hasLantern)
            {
                PlayerData.instance.hasLantern = true;
                Console.AddLine("Giving player lantern");
            }
            else
            {
                PlayerData.instance.hasLantern = false;
                Console.AddLine("Taking away lantern");
            }
        }

        private static void TramPassClicked(string buttonName)
        {
            if (!PlayerData.instance.hasTramPass)
            {
                PlayerData.instance.hasTramPass = true;
                Console.AddLine("Giving player tram pass");
            }
            else
            {
                PlayerData.instance.hasTramPass = false;
                Console.AddLine("Taking away tram pass");
            }
        }

        private static void MapQuillClicked(string buttonName)
        {
            if (!PlayerData.instance.hasQuill)
            {
                PlayerData.instance.hasQuill = true;
                Console.AddLine("Giving player map quill");
            }
            else
            {
                PlayerData.instance.hasQuill = false;
                Console.AddLine("Taking away map quill");
            }
        }

        private static void CityKeyClicked(string buttonName)
        {
            if (!PlayerData.instance.hasCityKey)
            {
                PlayerData.instance.hasCityKey = true;
                Console.AddLine("Giving player city crest");
            }
            else
            {
                PlayerData.instance.hasCityKey = false;
                Console.AddLine("Taking away city crest");
            }
        }

        private static void SlyKeyClicked(string buttonName)
        {
            if (!PlayerData.instance.hasSlykey)
            {
                PlayerData.instance.hasSlykey = true;
                Console.AddLine("Giving player shopkeeper's key");
            }
            else
            {
                PlayerData.instance.hasSlykey = false;
                Console.AddLine("Taking away shopkeeper's key");
            }
        }

        private static void ElegantKeyClicked(string buttonName)
        {
            if (!PlayerData.instance.hasWhiteKey)
            {
                PlayerData.instance.hasWhiteKey = true;
                PlayerData.instance.usedWhiteKey = false;
                Console.AddLine("Giving player elegant key");
            }
            else
            {
                PlayerData.instance.hasWhiteKey = false;
                Console.AddLine("Taking away elegant key");
            }
        }

        private static void LoveKeyClicked(string buttonName)
        {
            if (!PlayerData.instance.hasLoveKey)
            {
                PlayerData.instance.hasLoveKey = true;
                Console.AddLine("Giving player love key");
            }
            else
            {
                PlayerData.instance.hasLoveKey = false;
                Console.AddLine("Taking away love key");
            }
        }

        private static void KingsbrandClicked(string buttonName)
        {
            if (!PlayerData.instance.hasKingsBrand)
            {
                PlayerData.instance.hasKingsBrand = true;
                Console.AddLine("Giving player kingsbrand");
            }
            else
            {
                PlayerData.instance.hasKingsBrand = false;
                Console.AddLine("Taking away kingsbrand");
            }
        }

        private static void FlowerClicked(string buttonName)
        {
            if (!PlayerData.instance.hasXunFlower || PlayerData.instance.xunFlowerBroken)
            {
                PlayerData.instance.hasXunFlower = true;
                PlayerData.instance.xunFlowerBroken = false;
                Console.AddLine("Giving player delicate flower");
            }
            else
            {
                PlayerData.instance.hasLantern = false;
                Console.AddLine("Taking away delicate flower");
            }
        }

        private static void ReadDataClicked(string buttonName)
        {
            DreamGate.addMenu = false;
            DreamGate.delMenu = false;
            if (!DreamGate.dataBusy)
            {
                Console.AddLine("Updating DGdata from the file...");
                DreamGate.ReadData(true);
            }
        }

        private static void SaveDataClicked(string buttonName)
        {
            DreamGate.addMenu = false;
            DreamGate.delMenu = false;
            if (!DreamGate.dataBusy)
            {
                Console.AddLine("Writing DGdata to the file...");
                DreamGate.WriteData();
            }
        }

        private static void DeleteItemClicked(string buttonName)
        {
            DreamGate.addMenu = false;
            DreamGate.delMenu = !DreamGate.delMenu;
        }

        private static void AddItemClicked(string buttonName)
        {
            DreamGate.addMenu = true;
            DreamGate.delMenu = false;

            string entryName = DebugMod.gm.GetSceneNameString();
            int i = 1;

            if (entryName.Length > 5) entryName = entryName.Substring(0, 5);

            while (DreamGate.DGData.ContainsKey(entryName))
            {
                entryName = DebugMod.gm.GetSceneNameString() + i;
                i++;
            }

            DreamGate.AddEntry(entryName);
        }

        private static void SetWarpClicked(string buttonName)
        {
            string text = panel.GetPanel("DreamGate Panel").GetButton(buttonName).GetText();

            if (!String.IsNullOrEmpty(text))
            {
                DreamGate.ClickedEntry(text);
            }
        }

        private static void ScrollUpClicked(string buttonName)
        {
            if (DreamGate.scrollPosition > 0)
            {
                DreamGate.scrollPosition--;
            }
        }

        private static void ScrollDownClicked(string buttonName)
        {
            if (DreamGate.scrollPosition + 6 < DreamGate.DGData.Count)
            {
                DreamGate.scrollPosition++;
            }
        }
    }
}
