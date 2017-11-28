using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace DebugMod
{
    public static class DreamGate
    {
        public static Dictionary<string, KeyValuePair<string, float[]>> DGData = new Dictionary<string, KeyValuePair<string, float[]>>();
        public static int scrollPosition = 0;
        public static bool addMenu;
        public static bool delMenu;
        public static bool dataBusy;

        public static void Reset()
        {
            DGData.Clear();
            scrollPosition = 0;
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
                        DebugMod.Instance.LogError("[DREAM GATE] Unable to delete existing dreamgate.dat " + arg);
                        Console.AddLine("Unable to delete existing dreamgate.dat " + arg);
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
                    DebugMod.Instance.LogWarn("[DREAM GATE] Unable to read content of dreamgate.dat properly, file is empty?");
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
                                    DebugMod.Instance.LogError("[DREAM GATE] FormatException - incorrect float format");
                                    Console.AddLine("DGdata::FormatException - incorrect float format");
                                    dataBusy = false;
                                    return;
                                }
                                catch (OverflowException)
                                {
                                    DebugMod.Instance.LogError("[DREAM GATE] OverflowException - incorrect float format");
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
                    DebugMod.Instance.Log("[DREAM GATE] Filled DGdata: " + DGData.Count);
                    return;
                }
            }
            else
            {
                Console.AddLine("File dreamgate.dat not found!");
                DebugMod.Instance.Log("[DREAM GATE] File dreamgate.dat not found!");
            }
        }

        public static void ClickedEntry(string text)
        {
            if (delMenu)
            {
                dataBusy = true;
                Console.AddLine("Removed entry " + text + " from the list");
                DGData.Remove(text);
                dataBusy = false;
                delMenu = false;
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

        public static void AddEntry(string name)
        {
            if (!string.IsNullOrEmpty(name) && !name.Contains("|") && DGData != null && !DGData.ContainsKey(name) && !dataBusy)
            {
                float[] value6 = new float[] { DebugMod.RefKnight.transform.position.x, DebugMod.RefKnight.transform.position.y };
                delMenu = false;
                dataBusy = true;
                DGData.Add(name, new KeyValuePair<string, float[]>(DebugMod.GM.sceneName, value6));
                dataBusy = false;
                Console.AddLine("Added new DGdata entry named: " + name);
                addMenu = false;
            }
            else
            {
                Console.AddLine("Entry name either empty or contains symbol '|' or entry with the same name already exist or some weird as shit internal error");
            }
        }
    }
}
