using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace DebugMod
{
    public static class DreamGate
    {
        public static Dictionary<string, KeyValuePair<string, float[]>> dgData = new Dictionary<string, KeyValuePair<string, float[]>>();
        public static int scrollPosition;
        public static bool delMenu;
        public static bool dataBusy;

        public static void Reset()
        {
            dgData.Clear();
            scrollPosition = 0;
            ReadData(false);
        }

        public static void WriteData()
        {
            if (dgData != null)
            {
                if (File.Exists("dreamgate.dat"))
                {
                    try
                    {
                        File.Delete("dreamgate.dat");
                    }
                    catch (Exception arg)
                    {
                        DebugMod.instance.LogError("[DREAM GATE] Unable to delete existing dreamgate.dat " + arg);
                        Console.AddLine("Unable to delete existing dreamgate.dat " + arg);
                        return;
                    }
                }
                int num = 0;
                dataBusy = true;
                foreach (KeyValuePair<string, KeyValuePair<string, float[]>> keyValuePair in dgData)
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
                if (dgData == null)
                {
                    dgData = new Dictionary<string, KeyValuePair<string, float[]>>();
                }
                if (!update)
                {
                    dgData.Clear();
                }

                string[] array = null;

                try
                {
                    array = File.ReadAllLines("dreamgate.dat");
                }
                catch (Exception e)
                {
                    DebugMod.instance.LogError("Could not read dreamgate.dat:\n" + e);
                }
                
                if (array == null || array.Length == 0)
                {
                    Console.AddLine("Unable to read content of dreamgate.dat properly, file is empty?");
                    DebugMod.instance.LogWarn("[DREAM GATE] Unable to read content of dreamgate.dat properly, file is empty?");
                    dataBusy = false;
                    return;
                }
                foreach (string t in array)
                {
                    int num = t.Length - t.Replace("|", "").Length;
                    if (!string.IsNullOrEmpty(t) && t.Length < 500 && t.Length > 17 && num == 2)
                    {
                        string[] array2 = t.Split('|');
                        if (!string.IsNullOrEmpty(array2[0]) && !string.IsNullOrEmpty(array2[1]) && !string.IsNullOrEmpty(array2[2]))
                        {
                            string key = array2[0];
                            float num2 = 0f;
                            float num3 = 0f;
                            string[] array3 = array2[2].Split('-');
                            if (array3.Length == 2)
                            {
                                try
                                {
                                    num2 = float.Parse(array3[0], CultureInfo.InvariantCulture);
                                    num3 = float.Parse(array3[1], CultureInfo.InvariantCulture);
                                }
                                catch (FormatException)
                                {
                                    DebugMod.instance.LogError("[DREAM GATE] FormatException - incorrect float format");
                                    Console.AddLine("DGdata::FormatException - incorrect float format");
                                    dataBusy = false;
                                    return;
                                }
                                catch (OverflowException)
                                {
                                    DebugMod.instance.LogError("[DREAM GATE] OverflowException - incorrect float format");
                                    Console.AddLine("DGdata::OverflowException - incorrect float format");
                                    dataBusy = false;
                                    return;
                                }
                            }
                            if (num2 != 0f && num3 != 0f && !dgData.ContainsKey(key))
                            {
                                dgData.Add(key, new KeyValuePair<string, float[]>(array2[1], new float[]
                                {
                                    num2,
                                    num3
                                }));
                            }
                        }
                    }
                }
                dataBusy = false;
                if (dgData.Count > 0)
                {
                    Console.AddLine("Filled DGdata: " + dgData.Count);
                    DebugMod.instance.Log("[DREAM GATE] Filled DGdata: " + dgData.Count);
                    return;
                }
            }
            else
            {
                Console.AddLine("File dreamgate.dat not found!");
                DebugMod.instance.Log("[DREAM GATE] File dreamgate.dat not found!");
            }
        }

        public static void ClickedEntry(string text)
        {
            if (delMenu)
            {
                dataBusy = true;
                Console.AddLine("Removed entry " + text + " from the list");
                dgData.Remove(text);
                dataBusy = false;
                delMenu = false;
            }
            else
            {
                PlayerData pd = PlayerData.instance;

                pd.dreamGateScene = dgData[text].Key;
                pd.dreamGateX = dgData[text].Value[0];
                pd.dreamGateY = dgData[text].Value[1];
                
                Console.AddLine("New Dreamgate warp set: " + pd.dreamGateScene + "/" + pd.dreamGateX + "/" + pd.dreamGateY);
            }
        }

        public static void AddEntry(string name)
        {
            if (!string.IsNullOrEmpty(name) && !name.Contains("|") && dgData != null && !dgData.ContainsKey(name) && !dataBusy)
            {
                float[] value6 = new float[] { DebugMod.RefKnight.transform.position.x, DebugMod.RefKnight.transform.position.y };
                delMenu = false;
                dataBusy = true;
                dgData.Add(name, new KeyValuePair<string, float[]>(DebugMod.GM.sceneName, value6));
                dataBusy = false;
                Console.AddLine("Added new DGdata entry named: " + name);
            }
            else
            {
                Console.AddLine("Entry name either empty or contains symbol '|' or entry with the same name already exist or some weird as shit internal error");
            }
        }
    }
}
