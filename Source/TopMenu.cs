using System;
using Modding;
using UnityEngine;
using UnityEngine.UI;

namespace DebugMod
{
    public static class TopMenu
    {
        private static CanvasPanel panel;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.Instance.images["ButtonsMenuBG"], new Vector2(1092f, 25f), Vector2.zero, new Rect(0f, 0f, GUIController.Instance.images["ButtonsMenuBG"].width, GUIController.Instance.images["ButtonsMenuBG"].height));

            Rect buttonRect = new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height);
            
            //Main buttons
            panel.AddButton("Hide Menu", GUIController.Instance.images["ButtonRect"], new Vector2(46f, 28f), Vector2.zero, HideMenuClicked, buttonRect, GUIController.Instance.trajanBold, "Hide Menu");
            panel.AddButton("Kill All", GUIController.Instance.images["ButtonRect"], new Vector2(146f, 28f), Vector2.zero, KillAllClicked, buttonRect, GUIController.Instance.trajanBold, "Kill All");
            panel.AddButton("Set Spawn", GUIController.Instance.images["ButtonRect"], new Vector2(246f, 28f), Vector2.zero, SetSpawnClicked, buttonRect, GUIController.Instance.trajanBold, "Set Spawn");
            panel.AddButton("Respawn", GUIController.Instance.images["ButtonRect"], new Vector2(346f, 28f), Vector2.zero, RespawnClicked, buttonRect, GUIController.Instance.trajanBold, "Respawn");
            panel.AddButton("Dump Log", GUIController.Instance.images["ButtonRect"], new Vector2(446f, 28f), Vector2.zero, DumpLogClicked, buttonRect, GUIController.Instance.trajanBold, "Dump Log");
            panel.AddButton("Cheats", GUIController.Instance.images["ButtonRect"], new Vector2(46f, 68f), Vector2.zero, CheatsClicked, buttonRect, GUIController.Instance.trajanBold, "Cheats");
            panel.AddButton("Charms", GUIController.Instance.images["ButtonRect"], new Vector2(146f, 68f), Vector2.zero, CharmsClicked, buttonRect, GUIController.Instance.trajanBold, "Charms");
            panel.AddButton("Skills", GUIController.Instance.images["ButtonRect"], new Vector2(246f, 68f), Vector2.zero, SkillsClicked, buttonRect, GUIController.Instance.trajanBold, "Skills");
            panel.AddButton("Items", GUIController.Instance.images["ButtonRect"], new Vector2(346f, 68f), Vector2.zero, ItemsClicked, buttonRect, GUIController.Instance.trajanBold, "Items");
            panel.AddButton("Bosses", GUIController.Instance.images["ButtonRect"], new Vector2(446f, 68f), Vector2.zero, BossesClicked, buttonRect, GUIController.Instance.trajanBold, "Bosses");
            panel.AddButton("DreamGate", GUIController.Instance.images["ButtonRect"], new Vector2(546f, 68f), Vector2.zero, DreamGatePanelClicked, buttonRect, GUIController.Instance.trajanBold, "DreamGate");

            //Dropdown panels
            panel.AddPanel("Cheats Panel", GUIController.Instance.images["DropdownBG"], new Vector2(45f, 75f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 210f));
            panel.AddPanel("Charms Panel", GUIController.Instance.images["DropdownBG"], new Vector2(145f, 75f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 240f));
            panel.AddPanel("Skills Panel", GUIController.Instance.images["DropdownBG"], new Vector2(245f, 75f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, GUIController.Instance.images["DropdownBG"].height));
            panel.AddPanel("Items Panel", GUIController.Instance.images["DropdownBG"], new Vector2(345f, 75f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, GUIController.Instance.images["DropdownBG"].height));
            panel.AddPanel("Bosses Panel", GUIController.Instance.images["DropdownBG"], new Vector2(445f, 75f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 200f));
            panel.AddPanel("DreamGate Panel", GUIController.Instance.images["DreamGateDropdownBG"], new Vector2(545f, 75f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DreamGateDropdownBG"].width, GUIController.Instance.images["DreamGateDropdownBG"].height));

            //Cheats panel
            panel.GetPanel("Cheats Panel").AddButton("Infinite Jump", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, InfiniteJumpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Infinite Jump", 10);
            panel.GetPanel("Cheats Panel").AddButton("Infinite Soul", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, InfiniteSoulClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Infinite Soul", 10);
            panel.GetPanel("Cheats Panel").AddButton("Infinite HP", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, InfiniteHPClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Infinite HP", 10);
            panel.GetPanel("Cheats Panel").AddButton("Invincibility", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, InvincibilityClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Invincibility", 10);
            panel.GetPanel("Cheats Panel").AddButton("Noclip", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, NoclipClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Noclip", 10);
            panel.GetPanel("Cheats Panel").AddButton("Kill Self", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 180f), Vector2.zero, KillSelfClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Kill Self", 10);


            //Charms panel
            panel.GetPanel("Charms Panel").AddButton("All Charms", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllCharmsClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "All Charms", 10);
            panel.GetPanel("Charms Panel").AddButton("Kingsoul", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, KingsoulClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Kingsoul: " + PlayerData.instance.royalCharmState, 10);
            panel.GetPanel("Charms Panel").AddButton("Grimmchild", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, GrimmchildClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Grimmchild: -", 9);
            panel.GetPanel("Charms Panel").AddButton("fHeart fix", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, FragileHeartFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "fHeart fix", 10);
            panel.GetPanel("Charms Panel").AddButton("fGreed fix", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, FragileGreedFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "fGreed fix", 10);
            panel.GetPanel("Charms Panel").AddButton("fStrength fix", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 180f), Vector2.zero, FragileStrengthFixClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "fStrength fix", 10);
            panel.GetPanel("Charms Panel").AddButton("Overcharm", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 210f), Vector2.zero, OvercharmClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Overcharm", 10);

            //Skills panel buttons
            panel.GetPanel("Skills Panel").AddButton("All Skills", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, AllSkillsClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "All Skills", 10);
            panel.GetPanel("Skills Panel").AddButton("Scream", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 60f), Vector2.zero, ScreamClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Scream: " + PlayerData.instance.screamLevel, 10);
            panel.GetPanel("Skills Panel").AddButton("Fireball", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, FireballClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Fireball: " + PlayerData.instance.fireballLevel, 10);
            panel.GetPanel("Skills Panel").AddButton("Quake", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 120f), Vector2.zero, QuakeClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Quake: " + PlayerData.instance.quakeLevel, 10);
            panel.GetPanel("Skills Panel").AddButton("Mothwing Cloak", GUIController.Instance.images["MothwingCloak"], new Vector2(5f, 150f), new Vector2(37f, 34f), MothwingCloakClicked, new Rect(0f, 0f, GUIController.Instance.images["MothwingCloak"].width, GUIController.Instance.images["MothwingCloak"].height));
            panel.GetPanel("Skills Panel").AddButton("Mantis Claw", GUIController.Instance.images["MantisClaw"], new Vector2(43f, 150f), new Vector2(37f, 34f), MantisClawClicked, new Rect(0f, 0f, GUIController.Instance.images["MantisClaw"].width, GUIController.Instance.images["MantisClaw"].height));
            panel.GetPanel("Skills Panel").AddButton("Monarch Wings", GUIController.Instance.images["MonarchWings"], new Vector2(5f, 194f), new Vector2(37f, 33f), MonarchWingsClicked, new Rect(0f, 0f, GUIController.Instance.images["MonarchWings"].width, GUIController.Instance.images["MonarchWings"].height));
            panel.GetPanel("Skills Panel").AddButton("Crystal Heart", GUIController.Instance.images["CrystalHeart"], new Vector2(43f, 194f), new Vector2(37f, 34f), CrystalHeartClicked, new Rect(0f, 0f, GUIController.Instance.images["CrystalHeart"].width, GUIController.Instance.images["CrystalHeart"].height));
            panel.GetPanel("Skills Panel").AddButton("Isma's Tear", GUIController.Instance.images["IsmasTear"], new Vector2(5f, 238f), new Vector2(37f, 40f), IsmasTearClicked, new Rect(0f, 0f, GUIController.Instance.images["IsmasTear"].width, GUIController.Instance.images["IsmasTear"].height));
            panel.GetPanel("Skills Panel").AddButton("Dream Nail", GUIController.Instance.images["DreamNail1"], new Vector2(43f, 251f), new Vector2(37f, 59f), DreamNailClicked, new Rect(0f, 0f, GUIController.Instance.images["DreamNail1"].width, GUIController.Instance.images["DreamNail1"].height));
            panel.GetPanel("Skills Panel").AddButton("Dream Gate", GUIController.Instance.images["DreamGate"], new Vector2(5f, 288f), new Vector2(37f, 36f), DreamGateClicked, new Rect(0f, 0f, GUIController.Instance.images["DreamGate"].width, GUIController.Instance.images["DreamGate"].height));
            panel.GetPanel("Skills Panel").AddButton("Great Slash", GUIController.Instance.images["NailArt_GreatSlash"], new Vector2(5f, 329f), new Vector2(23f, 23f), GreatSlashClicked, new Rect(0f, 0f, GUIController.Instance.images["NailArt_GreatSlash"].width, GUIController.Instance.images["NailArt_GreatSlash"].height));
            panel.GetPanel("Skills Panel").AddButton("Dash Slash", GUIController.Instance.images["NailArt_DashSlash"], new Vector2(33f, 329f), new Vector2(23f, 23f), DashSlashClicked, new Rect(0f, 0f, GUIController.Instance.images["NailArt_DashSlash"].width, GUIController.Instance.images["NailArt_DashSlash"].height));
            panel.GetPanel("Skills Panel").AddButton("Cyclone Slash", GUIController.Instance.images["NailArt_CycloneSlash"], new Vector2(61f, 329f), new Vector2(23f, 23f), CycloneSlashClicked, new Rect(0f, 0f, GUIController.Instance.images["NailArt_CycloneSlash"].width, GUIController.Instance.images["NailArt_CycloneSlash"].height));

            //Skills panel button glow
            panel.GetPanel("Skills Panel").AddImage("Mothwing Cloak Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 145f), new Vector2(47f, 44f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Mantis Claw Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 145f), new Vector2(47f, 44f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Monarch Wings Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 189f), new Vector2(47f, 43f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Crystal Heart Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 189f), new Vector2(47f, 44f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Isma's Tear Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 233f), new Vector2(47f, 50f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Dream Nail Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 246f), new Vector2(47f, 69f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Dream Gate Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 283f), new Vector2(47f, 46f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Great Slash Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Dash Slash Glow", GUIController.Instance.images["BlueGlow"], new Vector2(28f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Skills Panel").AddImage("Cyclone Slash Glow", GUIController.Instance.images["BlueGlow"], new Vector2(56f, 324f), new Vector2(33f, 33f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));

            //Items panel
            panel.GetPanel("Items Panel").AddButton("Pale Ore", GUIController.Instance.images["PaleOre"], new Vector2(5f, 30f), new Vector2(23f, 22f), PaleOreClicked, new Rect(0, 0, GUIController.Instance.images["PaleOre"].width, GUIController.Instance.images["PaleOre"].height));
            panel.GetPanel("Items Panel").AddButton("Simple Key", GUIController.Instance.images["SimpleKey"], new Vector2(33f, 30f), new Vector2(23f, 23f), SimpleKeyClicked, new Rect(0, 0, GUIController.Instance.images["SimpleKey"].width, GUIController.Instance.images["SimpleKey"].height));
            panel.GetPanel("Items Panel").AddButton("Rancid Egg", GUIController.Instance.images["RancidEgg"], new Vector2(61f, 30f), new Vector2(23f, 30f), RancidEggClicked, new Rect(0, 0, GUIController.Instance.images["RancidEgg"].width, GUIController.Instance.images["RancidEgg"].height));
            panel.GetPanel("Items Panel").AddButton("Geo", GUIController.Instance.images["Geo"], new Vector2(5f, 63f), new Vector2(23f, 23f), GeoClicked, new Rect(0, 0, GUIController.Instance.images["Geo"].width, GUIController.Instance.images["Geo"].height));
            panel.GetPanel("Items Panel").AddButton("Essence", GUIController.Instance.images["Essence"], new Vector2(33f, 63f), new Vector2(23f, 23f), EssenceClicked, new Rect(0, 0, GUIController.Instance.images["Essence"].width, GUIController.Instance.images["Essence"].height));
            panel.GetPanel("Items Panel").AddButton("Lantern", GUIController.Instance.images["Lantern"], new Vector2(5f, 96f), new Vector2(37f, 41f), LanternClicked, new Rect(0, 0, GUIController.Instance.images["Lantern"].width, GUIController.Instance.images["Lantern"].height));
            panel.GetPanel("Items Panel").AddButton("Tram Pass", GUIController.Instance.images["TramPass"], new Vector2(43f, 96f), new Vector2(37f, 27f), TramPassClicked, new Rect(0, 0, GUIController.Instance.images["TramPass"].width, GUIController.Instance.images["TramPass"].height));
            panel.GetPanel("Items Panel").AddButton("Map & Quill", GUIController.Instance.images["MapQuill"], new Vector2(5f, 147f), new Vector2(37f, 30f), MapQuillClicked, new Rect(0, 0, GUIController.Instance.images["MapQuill"].width, GUIController.Instance.images["MapQuill"].height));
            panel.GetPanel("Items Panel").AddButton("City Crest", GUIController.Instance.images["CityKey"], new Vector2(43f, 147f), new Vector2(37f, 50f), CityKeyClicked, new Rect(0, 0, GUIController.Instance.images["CityKey"].width, GUIController.Instance.images["CityKey"].height));
            panel.GetPanel("Items Panel").AddButton("Sly Key", GUIController.Instance.images["SlyKey"], new Vector2(5f, 207f), new Vector2(37f, 39f), SlyKeyClicked, new Rect(0, 0, GUIController.Instance.images["SlyKey"].width, GUIController.Instance.images["SlyKey"].height));
            panel.GetPanel("Items Panel").AddButton("Elegant Key", GUIController.Instance.images["ElegantKey"], new Vector2(43f, 207f), new Vector2(37f, 36f), ElegantKeyClicked, new Rect(0, 0, GUIController.Instance.images["ElegantKey"].width, GUIController.Instance.images["ElegantKey"].height));
            panel.GetPanel("Items Panel").AddButton("Love Key", GUIController.Instance.images["LoveKey"], new Vector2(5f, 256f), new Vector2(37f, 36f), LoveKeyClicked, new Rect(0, 0, GUIController.Instance.images["LoveKey"].width, GUIController.Instance.images["LoveKey"].height));
            panel.GetPanel("Items Panel").AddButton("King's Brand", GUIController.Instance.images["Kingsbrand"], new Vector2(43f, 256f), new Vector2(37f, 35f), KingsbrandClicked, new Rect(0, 0, GUIController.Instance.images["Kingsbrand"].width, GUIController.Instance.images["Kingsbrand"].height));
            panel.GetPanel("Items Panel").AddButton("Bullshit Flower", GUIController.Instance.images["Flower"], new Vector2(5f, 302f), new Vector2(37f, 35f), FlowerClicked, new Rect(0, 0, GUIController.Instance.images["Flower"].width, GUIController.Instance.images["Flower"].height));
            
            //Items panel button glow
            panel.GetPanel("Items Panel").AddImage("Lantern Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 91f), new Vector2(47f, 51f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Tram Pass Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 91f), new Vector2(47f, 37f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Map & Quill Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 142f), new Vector2(47f, 40f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("City Crest Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 142f), new Vector2(47f, 60f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Sly Key Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 202f), new Vector2(47f, 49f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Elegant Key Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 202f), new Vector2(47f, 46f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Love Key Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 251f), new Vector2(47f, 46f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("King's Brand Glow", GUIController.Instance.images["BlueGlow"], new Vector2(38f, 251f), new Vector2(47f, 45f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));
            panel.GetPanel("Items Panel").AddImage("Bullshit Flower Glow", GUIController.Instance.images["BlueGlow"], new Vector2(0f, 297f), new Vector2(47f, 45f), new Rect(0f, 0f, GUIController.Instance.images["BlueGlow"].width, GUIController.Instance.images["BlueGlow"].height));

            //Boss panel
            panel.GetPanel("Bosses Panel").AddButton("Respawn Boss", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, RespawnBossClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Respawn Boss", 10);
            panel.GetPanel("Bosses Panel").AddButton("Respawn Ghost", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 50f), Vector2.zero, RespawnGhostClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Respawn Ghost", 9);

            panel.GetPanel("Bosses Panel").AddButton("Failed Champ", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 110f), Vector2.zero, FailedChampClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Failed Champ", 10);
            panel.GetPanel("Bosses Panel").AddButton("Soul Tyrant", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 130f), Vector2.zero, SoulTyrantClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Soul Tyrant", 10);
            panel.GetPanel("Bosses Panel").AddButton("Lost Kin", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 150f), Vector2.zero, LostKinClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Lost Kin", 10);
            panel.GetPanel("Bosses Panel").AddButton("NK Grimm", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 180f), Vector2.zero, NKGrimmClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "NK Grimm", 10);

            //Dream gate left panel
            panel.GetPanel("DreamGate Panel").AddButton("Read Data", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 30f), Vector2.zero, ReadDataClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Read Data", 10);
            panel.GetPanel("DreamGate Panel").AddButton("Save Data", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 50f), Vector2.zero, SaveDataClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Save Data", 10);
            panel.GetPanel("DreamGate Panel").AddButton("Delete Item", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 70f), Vector2.zero, DeleteItemClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Delete Item", 10);
            panel.GetPanel("DreamGate Panel").AddButton("Add Item", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 90f), Vector2.zero, AddItemClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.trajanNormal, "Add Item", 10);

            //Dream gate right panel
            panel.GetPanel("DreamGate Panel").AddButton("Right1", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(90f, 30f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right2", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(90f, 50f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right3", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(90f, 70f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right4", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(90f, 90f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right5", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(90f, 110f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.arial, "");
            panel.GetPanel("DreamGate Panel").AddButton("Right6", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(90f, 130f), Vector2.zero, SetWarpClicked, new Rect(0f, 0f, 80f, 20f), GUIController.Instance.arial, "");

            //Dream gate scroll
            panel.GetPanel("DreamGate Panel").AddButton("Scroll Up", GUIController.Instance.images["ScrollBarArrowUp"], new Vector2(180f, 30f), Vector2.zero, ScrollUpClicked, new Rect(0f, 0f, GUIController.Instance.images["ScrollBarArrowUp"].width, GUIController.Instance.images["ScrollBarArrowUp"].height));
            panel.GetPanel("DreamGate Panel").AddButton("Scroll Down", GUIController.Instance.images["ScrollBarArrowDown"], new Vector2(180f, 130f), Vector2.zero, ScrollDownClicked, new Rect(0f, 0f, GUIController.Instance.images["ScrollBarArrowDown"].width, GUIController.Instance.images["ScrollBarArrowDown"].height));

            panel.FixRenderOrder();
        }

        public static void Update()
        {
            if (panel == null)
            {
                return;
            }

            if (DebugMod.GM.IsNonGameplayScene())
            {
                if (panel.active)
                {
                    panel.SetActive(false, true);
                }

                return;
            }

            if (DebugMod.settings.TopMenuVisible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!DebugMod.settings.TopMenuVisible && panel.active)
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

            if (panel.GetPanel("Cheats Panel").active)
            {
                panel.GetButton("Infinite Jump", "Cheats Panel").SetTextColor(PlayerData.instance.infiniteAirJump ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Infinite Soul", "Cheats Panel").SetTextColor(DebugMod.infiniteSoul ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Infinite HP", "Cheats Panel").SetTextColor(DebugMod.infiniteHP ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Invincibility", "Cheats Panel").SetTextColor(PlayerData.instance.isInvincible ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                panel.GetButton("Noclip", "Cheats Panel").SetTextColor(DebugMod.noclip ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
            }

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

                foreach (string entryName in DreamGate.dgData.Keys)
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
            if (PlayerData.instance.dreamNailUpgraded) panel.GetButton("Dream Nail", "Skills Panel").UpdateSprite(GUIController.Instance.images["DreamNail2"], new Rect(0f, 0f, GUIController.Instance.images["DreamNail2"].width, GUIController.Instance.images["DreamNail2"].height));
            else panel.GetButton("Dream Nail", "Skills Panel").UpdateSprite(GUIController.Instance.images["DreamNail1"], new Rect(0f, 0f, GUIController.Instance.images["DreamNail1"].width, GUIController.Instance.images["DreamNail1"].height));
            if (PlayerData.instance.hasShadowDash) panel.GetButton("Mothwing Cloak", "Skills Panel").UpdateSprite(GUIController.Instance.images["ShadeCloak"], new Rect(0f, 0f, GUIController.Instance.images["ShadeCloak"].width, GUIController.Instance.images["ShadeCloak"].height));
            else panel.GetButton("Mothwing Cloak", "Skills Panel").UpdateSprite(GUIController.Instance.images["MothwingCloak"], new Rect(0f, 0f, GUIController.Instance.images["MothwingCloak"].width, GUIController.Instance.images["MothwingCloak"].height));

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

        //TODO: Remove all these pointless functions, call BindableFunctions.whatever directly

        private static void HideMenuClicked(string buttonName)
        {
            Text text = CanvasUtil.CreateTextPanel(GUIController.Instance.canvas, "", 27, TextAnchor.MiddleCenter,
                new CanvasUtil.RectData(
                    new Vector2(0, 50),
                    new Vector2(0, 45),
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0.5f, 0.5f))).GetComponent<Text>();
            text.font = CanvasUtil.TrajanBold;
            text.text = $"Press {Enum.GetName(typeof(KeyCode), DebugMod.settings.binds["Toggle All UI"])} to unhide the menu!";
            text.fontSize = 42;
            text.CrossFadeAlpha(1f, 0f, false);
            text.CrossFadeAlpha(0f, 6f, false);
            BindableFunctions.ToggleAllPanels();
        }

        private static void KillAllClicked(string buttonName)
        {
            BindableFunctions.KillAll();
        }

        private static void SetSpawnClicked(string buttonName)
        {
            BindableFunctions.SetHazardRespawn();
        }

        private static void RespawnClicked(string buttonName)
        {
            BindableFunctions.Respawn();
        }

        private static void DumpLogClicked(string buttonName)
        {
            BindableFunctions.DumpConsoleLog();
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
            BindableFunctions.ToggleInfiniteJump();
        }

        private static void InfiniteSoulClicked(string buttonName)
        {
            BindableFunctions.ToggleInfiniteSoul();
        }

        private static void InfiniteHPClicked(string buttonName)
        {
            BindableFunctions.ToggleInfiniteHP();
        }

        private static void InvincibilityClicked(string buttonName)
        {
            BindableFunctions.ToggleInvincibility();
        }

        private static void NoclipClicked(string buttonName)
        {
            BindableFunctions.ToggleNoclip();
        }

        private static void KillSelfClicked(string buttonName)
        {
            BindableFunctions.KillSelf();
        }

        private static void AllCharmsClicked(string buttonName)
        {
            BindableFunctions.GiveAllCharms();
        }

        private static void KingsoulClicked(string buttonName)
        {
            BindableFunctions.IncreaseKingsoulLevel();
        }

        private static void FragileHeartFixClicked(string buttonName)
        {
            BindableFunctions.FixFragileHeart();
        }

        private static void FragileGreedFixClicked(string buttonName)
        {
            BindableFunctions.FixFragileGreed();
        }

        private static void FragileStrengthFixClicked(string buttonName)
        {
            BindableFunctions.FixFragileStrength();
        }

        private static void OvercharmClicked(string buttonName)
        {
            BindableFunctions.ToggleOvercharm();
        }

        private static void GrimmchildClicked(string buttonName)
        {
            BindableFunctions.IncreaseGrimmchildLevel();
        }

        private static void AllSkillsClicked(string buttonName)
        {
            BindableFunctions.GiveAllSkills();
        }

        private static void ScreamClicked(string buttonName)
        {
            BindableFunctions.IncreaseScreamLevel();
        }

        private static void FireballClicked(string buttonName)
        {
            BindableFunctions.IncreaseFireballLevel();
        }

        private static void QuakeClicked(string buttonName)
        {
            BindableFunctions.IncreaseQuakeLevel();
        }

        private static void MothwingCloakClicked(string buttonName)
        {
            BindableFunctions.ToggleMothwingCloak();
        }

        private static void MantisClawClicked(string buttonName)
        {
            BindableFunctions.ToggleMantisClaw();
        }

        private static void MonarchWingsClicked(string buttonName)
        {
            BindableFunctions.ToggleMonarchWings();
        }

        private static void CrystalHeartClicked(string buttonName)
        {
            BindableFunctions.ToggleCrystalHeart();
        }

        private static void IsmasTearClicked(string buttonName)
        {
            BindableFunctions.ToggleIsmasTear();
        }

        private static void DreamNailClicked(string buttonName)
        {
            BindableFunctions.ToggleDreamNail();
        }

        private static void DreamGateClicked(string buttonName)
        {
            BindableFunctions.ToggleDreamGate();
        }

        private static void GreatSlashClicked(string buttonName)
        {
            BindableFunctions.ToggleGreatSlash();
        }

        private static void DashSlashClicked(string buttonName)
        {
            BindableFunctions.ToggleDashSlash();
        }

        private static void CycloneSlashClicked(string buttonName)
        {
            BindableFunctions.ToggleCycloneSlash();
        }

        private static void RespawnGhostClicked(string buttonName)
        {
            BindableFunctions.RespawnGhost();
        }

        private static void RespawnBossClicked(string buttonName)
        {
            BindableFunctions.RespawnBoss();
        }

        private static void FailedChampClicked(string buttonName)
        {
            BindableFunctions.ToggleFailedChamp();
        }

        private static void SoulTyrantClicked(string buttonName)
        {
            BindableFunctions.ToggleSoulTyrant();
        }

        private static void LostKinClicked(string buttonName)
        {
            BindableFunctions.ToggleLostKin();
        }

        private static void NKGrimmClicked(string buttonName)
        {
            BindableFunctions.ToggleNKGrimm();
        }

        private static void PaleOreClicked(string buttonName)
        {
            BindableFunctions.GivePaleOre();
        }

        private static void SimpleKeyClicked(string buttonName)
        {
            BindableFunctions.GiveSimpleKey();
        }

        private static void RancidEggClicked(string buttonName)
        {
            BindableFunctions.GiveRancidEgg();
        }

        private static void GeoClicked(string buttonName)
        {
            BindableFunctions.GiveGeo();
        }

        private static void EssenceClicked(string buttonName)
        {
            BindableFunctions.GiveEssence();
        }

        private static void LanternClicked(string buttonName)
        {
            BindableFunctions.ToggleLantern();
        }

        private static void TramPassClicked(string buttonName)
        {
            BindableFunctions.ToggleTramPass();
        }

        private static void MapQuillClicked(string buttonName)
        {
            BindableFunctions.ToggleMapQuill();
        }

        private static void CityKeyClicked(string buttonName)
        {
            BindableFunctions.ToggleCityKey();
        }

        private static void SlyKeyClicked(string buttonName)
        {
            BindableFunctions.ToggleSlyKey();
        }

        private static void ElegantKeyClicked(string buttonName)
        {
            BindableFunctions.ToggleElegantKey();
        }

        private static void LoveKeyClicked(string buttonName)
        {
            BindableFunctions.ToggleLoveKey();
        }

        private static void KingsbrandClicked(string buttonName)
        {
            BindableFunctions.ToggleKingsbrand();
        }

        private static void FlowerClicked(string buttonName)
        {
            BindableFunctions.ToggleXunFlower();
        }

        private static void ReadDataClicked(string buttonName)
        {
            BindableFunctions.ReadDGData();
        }

        private static void SaveDataClicked(string buttonName)
        {
            BindableFunctions.SaveDGData();
        }

        private static void DeleteItemClicked(string buttonName)
        {
            DreamGate.delMenu = !DreamGate.delMenu;
        }

        private static void AddItemClicked(string buttonName)
        {
            BindableFunctions.AddDGPosition();
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
            if (DreamGate.scrollPosition + 6 < DreamGate.dgData.Count)
            {
                DreamGate.scrollPosition++;
            }
        }
    }
}
