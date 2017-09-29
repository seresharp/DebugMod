using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public static class BossHandler
    {
        public static bool bossSub;

        public static Dictionary<string, KeyValuePair<bool, string>> bossData;
        public static Dictionary<string, string> ghostData;
        public static bool bossFound;
        public static bool ghostFound;

        public static void LookForBoss(string sceneName)
        {
            bossFound = false;
            ghostFound = false;
            if (bossData != null && bossData.ContainsKey(sceneName))
            {
                Console.AddLine("Found stored Boss in this scene, respawn available");
                bossFound = true;
            }
            if (ghostData != null && ghostData.ContainsKey(sceneName))
            {
                Console.AddLine("Found stored Ghost Boss in this scene, respawn available");
                ghostFound = true;
            }
        }

        public static void PopulateBossLists()
        {
            if (bossData == null)
            {
                bossData = new Dictionary<string, KeyValuePair<bool, string>>(16);
            }
            if (ghostData == null)
            {
                ghostData = new Dictionary<string, string>(7);
            }
            bossData.Clear();
            ghostData.Clear();
            bossData.Add("Ruins2_03", new KeyValuePair<bool, string>(true, "Battle Control"));
            bossData.Add("Crossroads_09", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Crossroads_04", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Fungus1_04", new KeyValuePair<bool, string>(false, "hornet1Defeated"));
            bossData.Add("Crossroads_10", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Fungus3_archive_02", new KeyValuePair<bool, string>(false, "defeatedMegaJelly"));
            bossData.Add("Fungus2_15", new KeyValuePair<bool, string>(false, "defeatedMantisLords"));
            bossData.Add("Waterways_12", new KeyValuePair<bool, string>(false, "flukeMotherDefeated"));
            bossData.Add("Waterways_05", new KeyValuePair<bool, string>(false, "defeatedDungDefender"));
            bossData.Add("Ruins1_24", new KeyValuePair<bool, string>(false, "mageLordDefeated"));
            bossData.Add("Deepnest_32", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Mines_18", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Mines_32", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Fungus3_23", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Ruins2_11", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Deepnest_East_Hornet", new KeyValuePair<bool, string>(false, "hornetOutskirtsDefeated"));
            ghostData.Add("RestingGrounds_02", "xeroDefeated");
            ghostData.Add("Fungus1_35", "noEyesDefeated");
            ghostData.Add("Fungus2_32", "elderHuDefeated");
            ghostData.Add("Deepnest_East_10", "markothDefeated");
            ghostData.Add("Deepnest_40", "galienDefeated");
            ghostData.Add("Fungus3_40", "mumCaterpillarDefeated");
            ghostData.Add("Cliffs_02", "aladarSlugDefeated");
        }

        public static void UpdateGUI()
        {
            if (bossSub)
            {
                TextAnchor alignment6 = GUI.skin.button.alignment;
                int fontSize7 = GUI.skin.button.fontSize;
                Color contentColor4 = GUI.contentColor;
                Color backgroundColor5 = GUI.backgroundColor;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                GUI.skin.button.fontSize = 18;
                GUI.contentColor = Color.white;
                GUI.backgroundColor = Color.white;
                GUILayout.BeginArea(new Rect(320f, 0f, 150f, 1005f));
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                GUILayout.Label("[BOSSES]:", new GUILayoutOption[0]);
                if (bossFound)
                {
                    if (GUILayout.Button("Force respawn", new GUILayoutOption[] { GUILayout.Height(20f) }))
                    {
                        if (bossData[DebugMod.GetSceneName()].Key)
                        {
                            PlayMakerFSM[] components = GameObject.Find(bossData[DebugMod.GetSceneName()].Value).GetComponents<PlayMakerFSM>();
                            if (components != null)
                            {
                                foreach (PlayMakerFSM playMakerFSM in components)
                                {
                                    if (playMakerFSM.FsmVariables.GetFsmBool("Activated") != null)
                                    {
                                        playMakerFSM.FsmVariables.GetFsmBool("Activated").Value = false;
                                        Console.AddLine("Boss control for this scene was reset, re-enter scene or warp");
                                    }
                                }
                            }
                            else
                            {
                                Console.AddLine("GO does not exist or no FSM on it");
                            }
                        }
                        else
                        {
                            PlayerData.instance.GetType().GetField(bossData[DebugMod.GetSceneName()].Value).SetValue(PlayerData.instance, false);
                            Console.AddLine("Boss control for this scene was reset, re-enter scene or warp");
                        }
                    }
                }
                else
                {
                    FontStyle fontStyle4 = GUI.skin.label.fontStyle;
                    GUI.skin.label.fontStyle = FontStyle.Italic;
                    GUILayout.Label("<no data>", new GUILayoutOption[0]);
                    GUI.skin.label.fontStyle = fontStyle4;
                }
                GUILayout.Label("[GHOSTS]:", new GUILayoutOption[0]);
                if (ghostFound)
                {
                    if (GUILayout.Button("Force respawn", new GUILayoutOption[] { GUILayout.Height(20f) }))
                    {
                        PlayerData.instance.GetType().GetField(ghostData[DebugMod.GetSceneName()]).SetValue(PlayerData.instance, 0);
                        Console.AddLine("Ghost Boss for this scene was reset, re-enter scene or warp");
                    }
                }
                else
                {
                    FontStyle fontStyle5 = GUI.skin.label.fontStyle;
                    GUI.skin.label.fontStyle = FontStyle.Italic;
                    GUILayout.Label("<no data>", new GUILayoutOption[0]);
                    GUI.skin.label.fontStyle = fontStyle5;
                }
                GUILayout.Label("[DREAMBOSS]:", new GUILayoutOption[0]);
                GUI.backgroundColor = Color.green;
                PlayerData.instance.falseKnightDreamDefeated = GUILayout.Toggle(PlayerData.instance.falseKnightDreamDefeated, "Failed Knght", new GUILayoutOption[0]);
                PlayerData.instance.mageLordDreamDefeated = GUILayout.Toggle(PlayerData.instance.mageLordDreamDefeated, "Soul Tyrant", new GUILayoutOption[0]);
                PlayerData.instance.infectedKnightDreamDefeated = GUILayout.Toggle(PlayerData.instance.infectedKnightDreamDefeated, "Lost Kin", new GUILayoutOption[0]);
                GUI.skin.button.alignment = alignment6;
                GUI.contentColor = contentColor4;
                GUI.backgroundColor = backgroundColor5;
                GUI.skin.button.fontSize = fontSize7;
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
    }
}
