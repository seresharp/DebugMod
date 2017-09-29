using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using UnityEngine;

namespace DebugMod
{
    public static class DreamGate
    {
        public static bool menuOpen;

        private static Dictionary<string, KeyValuePair<string, float[]>> DGData = new Dictionary<string, KeyValuePair<string, float[]>>();
        private static bool dataBusy;
        private static bool addMenu;
        private static string DGentry = "Entry name";
        private static bool delMenu;
        private static Vector2 scrollPosition = Vector2.zero;

        public static void Reset()
        {
            DGData.Clear();
            scrollPosition = Vector2.zero;
            ReadData(false);
        }

        public static void WriteData()
        {
            if (DGData != null)
            {
                if (File.Exists("dreamgate.dat"))
                {
                    try
                    {
                        File.Delete("dreamgate.dat");
                    }
                    catch (Exception arg)
                    {
                        Modding.ModHooks.ModLog("[DEBUG MOD] [DREAM GATE] Unable to delete existing dreamgate.dat " + arg);
                        Console.AddLine("[DebugMod::DGata] Unable to delete existing dreamgate.dat " + arg);
                        return;
                    }
                }
                int num = 0;
                dataBusy = true;
                foreach (KeyValuePair<string, KeyValuePair<string, float[]>> keyValuePair in DGData)
                {
                    File.AppendAllText("dreamgate.dat", string.Concat(new object[]
                    {
                    keyValuePair.Key,
                    "|",
                    keyValuePair.Value.Key,
                    "|",
                    keyValuePair.Value.Value[0],
                    "-",
                    keyValuePair.Value.Value[1],
                    Environment.NewLine
                    }));
                    num++;
                }
                dataBusy = false;
                if (File.Exists("dreamgate.dat"))
                {
                    Console.AddLine("DGdata written sucessfully, entries written: " + num.ToString());
                }
            }
        }

        public static void ReadData(bool update)
        {
            if (File.Exists("dreamgate.dat"))
            {
                dataBusy = true;
                if (DGData == null)
                {
                    DGData = new Dictionary<string, KeyValuePair<string, float[]>>();
                }
                if (!update)
                {
                    DGData.Clear();
                }
                string[] array = File.ReadAllLines("dreamgate.dat");
                if (array == null || array.Length == 0)
                {
                    Console.AddLine("Unable to read content of dreamgate.dat properly, file is empty?");
                    Modding.ModHooks.ModLog("[DEBUG MOD] [DREAM GATE] Unable to read content of dreamgate.dat properly, file is empty?");
                    dataBusy = false;
                    return;
                }
                for (int i = 0; i < array.Length; i++)
                {
                    int num = array[i].Length - array[i].Replace("|", "").Length;
                    if (!string.IsNullOrEmpty(array[i]) && array[i].Length < 500 && array[i].Length > 17 && num == 2)
                    {
                        string[] array2 = array[i].Split(new char[]
                        {
                        '|'
                        });
                        if (!string.IsNullOrEmpty(array2[0]) && !string.IsNullOrEmpty(array2[1]) && !string.IsNullOrEmpty(array2[2]))
                        {
                            string key = array2[0];
                            float num2 = 0f;
                            float num3 = 0f;
                            string[] array3 = array2[2].Split(new char[]
                            {
                            '-'
                            });
                            if (array3.Length == 2)
                            {
                                try
                                {
                                    num2 = float.Parse(array3[0], CultureInfo.InvariantCulture);
                                    num3 = float.Parse(array3[1], CultureInfo.InvariantCulture);
                                }
                                catch (FormatException)
                                {
                                    Modding.ModHooks.ModLog("[DEBUG MOD] [DREAM GATE] FormatException - incorrect float format");
                                    Console.AddLine("DGdata::FormatException - incorrect float format");
                                    dataBusy = false;
                                    return;
                                }
                                catch (OverflowException)
                                {
                                    Modding.ModHooks.ModLog("[DEBUG MOD] [DREAM GATE] OverflowException - incorrect float format");
                                    Console.AddLine("DGdata::OverflowException - incorrect float format");
                                    dataBusy = false;
                                    return;
                                }
                            }
                            if (num2 != 0f && num3 != 0f && !DGData.ContainsKey(key))
                            {
                                DGData.Add(key, new KeyValuePair<string, float[]>(array2[1], new float[]
                                {
                                num2,
                                num3
                                }));
                            }
                        }
                    }
                }
                dataBusy = false;
                if (DGData.Count > 0)
                {
                    Console.AddLine("Filled DGdata: " + DGData.Count);
                    Modding.ModHooks.ModLog("[DEBUG MOD] [DREAM GATE] Filled DGdata: " + DGData.Count);
                    return;
                }
            }
            else
            {
                Console.AddLine("File dreamgate.dat not found!");
                Modding.ModHooks.ModLog("[DEBUG MOD] [DREAM GATE] File dreamgate.dat not found!");
            }
        }

        public static void UpdateGUI()
        {
            if (DreamGate.menuOpen)
            {
                TextAnchor alignment7 = GUI.skin.button.alignment;
                int fontSize8 = GUI.skin.button.fontSize;
                Color contentColor5 = GUI.contentColor;
                Color backgroundColor6 = GUI.backgroundColor;
                TextAnchor alignment8 = GUI.skin.label.alignment;
                int fontSize10 = GUI.skin.label.fontSize;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                GUI.skin.button.fontSize = 18;
                GUI.contentColor = Color.white;
                GUI.backgroundColor = Color.white;
                GUILayout.BeginArea(new Rect(940f, 705f, 200f, 300f));
                GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
                if (GUILayout.Button("U", new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(30f) }))
                {
                    delMenu = false;
                    FSMUtility.LocateFSM(DebugMod.refKnight, "Dream Nail").FsmVariables.GetFsmBool("Dream Warp Allowed").Value = true;
                    Console.AddLine("Unlocked Dream Warp, use at your own risk");
                }
                GUILayout.FlexibleSpace();
                if (DGData != null && GUILayout.Button("R", new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(30f) }))
                {
                    delMenu = false;
                    if (!dataBusy)
                    {
                        Console.AddLine("Updating DGdata from the file...");
                        ReadData(true);
                    }
                    else
                    {
                        Console.AddLine("Cannot read DGdata right now");
                    }
                }
                GUILayout.FlexibleSpace();
                if (DGData != null && DGData.Count > 0 && GUILayout.Button("S", new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(30f) }))
                {
                    delMenu = false;
                    if (!dataBusy)
                    {
                        Console.AddLine("Writing DGdata to the file...");
                        WriteData();
                    }
                    else
                    {
                        Console.AddLine("Cannot write DGdata right now");
                    }
                }
                GUILayout.FlexibleSpace();
                Color backgroundColor7 = GUI.backgroundColor;
                if (addMenu)
                {
                    GUI.backgroundColor = Color.yellow;
                }
                addMenu = GUILayout.Toggle(addMenu, "A", "Button", new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(30f) });
                GUILayout.FlexibleSpace();
                if (delMenu)
                {
                    GUI.backgroundColor = Color.red;
                }
                delMenu = GUILayout.Toggle(delMenu, "D", "Button", new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(30f) });
                GUI.backgroundColor = backgroundColor7;
                GUILayout.EndHorizontal();
                if (addMenu && DGData != null)
                {
                    GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                    GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
                    GUI.skin.textField.fontSize = 18;
                    GUI.skin.textField.fontStyle = FontStyle.Bold;
                    GUILayout.BeginHorizontal("box", new GUILayoutOption[0]);
                    DGentry = GUILayout.TextField(DGentry, 25, new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(165f) });
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(30f) }))
                    {
                        if (!string.IsNullOrEmpty(DGentry) && !DGentry.Contains("|") && DGData != null && !DGData.ContainsKey(DGentry) && !dataBusy)
                        {
                            float[] value6 = new float[] { DebugMod.refKnight.transform.position.x, DebugMod.refKnight.transform.position.y };
                            delMenu = false;
                            dataBusy = true;
                            DGData.Add(DGentry, new KeyValuePair<string, float[]>(DebugMod.gm.sceneName, value6));
                            dataBusy = false;
                            Console.AddLine("Added new DGdata entry named: " + DGentry);
                            addMenu = false;
                        }
                        else
                        {
                            Console.AddLine("Entry name either empty or contains symbol '|' or entry with the same name already exist or some weird as shit internal error");
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUI.skin.button.alignment = alignment8;
                GUI.skin.button.alignment = TextAnchor.MiddleLeft;
                if (DGData != null && DGData.Count > 0)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, new GUILayoutOption[]
                    {
                        GUILayout.MaxHeight(200f),
                        GUILayout.MaxHeight(300f),
                        GUILayout.ExpandHeight(false)
                    });
                    if (!dataBusy)
                    {
                        foreach (string text in DGData.Keys)
                        {
                            if (GUILayout.Button(text, new GUILayoutOption[] { GUILayout.Height(20f) }))
                            {
                                if (delMenu)
                                {
                                    dataBusy = true;
                                    Console.AddLine("Removed entry " + text + " from the list");
                                    DGData.Remove(text);
                                    dataBusy = false;
                                    delMenu = false;
                                    GUIUtility.ExitGUI();
                                }
                                else
                                {
                                    PlayerData pd = PlayerData.instance;

                                    pd.dreamGateScene = DGData[text].Key;
                                    pd.dreamGateX = DGData[text].Value[0];
                                    pd.dreamGateY = DGData[text].Value[1];

                                    
                                    Console.AddLine("New Dreamgate warp set: " + pd.dreamGateScene + "/" + pd.dreamGateX + "/" + pd.dreamGateY);
                                }
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                }
                else
                {
                    GUILayout.BeginVertical("box", new GUILayoutOption[0]);
                    GUILayout.Label("No DGdata found", new GUILayoutOption[0]);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                }
                GUI.skin.button.alignment = alignment7;
                GUI.contentColor = contentColor5;
                GUI.backgroundColor = backgroundColor6;
                GUI.skin.button.fontSize = fontSize8;
                GUILayout.EndArea();
            }
        }
    }
}
