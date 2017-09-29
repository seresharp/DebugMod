using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace DebugMod
{
    public static class SaveManager
    {
        public static string[] fileList;
        public static string[] copyList;

        private static bool listReady;
        private static int selGridInt;
        private static Vector2 scrollPosition;

        public static void GetPracticeSaves()
        {
            if (fileList != null && copyList != null) return;

            if (Directory.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + "practice"))
            {
                selGridInt = 0;
                fileList = Directory.GetFiles(Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + "practice", "*.dat");
                copyList = Directory.GetFiles(Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + "practice", "*.dat");
                if (fileList.Length != 0)
                {
                    for (int i = 0; i < fileList.Length; i++)
                    {
                        if (new FileInfo(fileList[i]).Name.Replace(".dat", "").Length > 25)
                        {
                            fileList[i] = new FileInfo(fileList[i]).Name.Replace(".dat", "").Substring(0, 25) + "...";
                        }
                        else
                        {
                            fileList[i] = new FileInfo(fileList[i]).Name.Replace(".dat", "");
                        }
                    }
                    listReady = true;
                    Modding.ModHooks.ModLog("[Debug Mod] [SAVEGAME MANAGER] Sucessfully updated list of files!");
                    return;
                }
                listReady = false;
                Modding.ModHooks.ModLog("[Debug Mod] [SAVEGAME MANAGER] Directory 'practice' is empty or does not contain any .dat files!");
            }
            listReady = false;
            Modding.ModHooks.ModLog("[Debug Mod] [SAVEGAME MANAGER] Directory 'practice' does not exist!");
        }

        public static void CopyPracticeFile()
        {
            string text = Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + "user1.dat";
            string sourceFileName = copyList[selGridInt];
            string text2 = Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + "user1_original.bak";
            if (File.Exists(text) && !File.Exists(text2))
            {
                try
                {
                    File.Replace(sourceFileName, text, text2);
                    Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] Replaced requested files and made backup");
                    return;
                }
                catch (Exception arg)
                {
                    Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] Unable to replace pre-existing discarded save file: " + arg);
                    return;
                }
            }
            try
            {
                File.Copy(sourceFileName, text, true);
                Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] Copied the practice file");
            }
            catch (Exception arg2)
            {
                Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] Unable to replace pre-existing discarded save file: " + arg2);
            }
        }

        public static void UpdateGUI()
        {
            if (UIManager.instance.menuState.ToString() == "MAIN_MENU")
            {
                if (listReady)
                {
                    GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                    GUI.skin.button.fontStyle = FontStyle.Bold;
                    GUI.skin.button.fontSize = 16;
                    scrollPosition = GUI.BeginScrollView(new Rect(0f, 20f, 320f, 300f), scrollPosition, new Rect(0f, 30f, 300f, (float)(fileList.Length * 30)), false, true);
                    selGridInt = GUI.SelectionGrid(new Rect(0f, 30f, 300f, (float)(fileList.Length * 30)), selGridInt, fileList, 1);
                    GUI.Label(new Rect(3f, (float)((selGridInt + 1) * 30 + 1), 30f, 30f), " <size=20><color=lime>✔</color> </size>");
                    GUI.EndScrollView();
                    if (GUI.Button(new Rect(0f, 330f, 148f, 30f), "COPY"))
                    {
                        Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] Selected file in list: " + fileList[selGridInt]);
                        Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] File to copy: " + copyList[selGridInt]);
                        CopyPracticeFile();
                        UIManager.instance.UIGoToProfileMenu();
                    }
                    if (GUI.Button(new Rect(151f, 330f, 148f, 30f), "REFRESH"))
                    {
                        Modding.ModHooks.ModLog("[DEBUG MOD] [SAVEGAME MANAGER] Refresh requested");
                        listReady = false;
                        GetPracticeSaves();
                        return;
                    }
                }
                else
                {
                    GUI.Label(new Rect(10f, 50f, 500f, 50f), "<size=16>Savegame Manager is unavailable, check README and output_log.txt for info</size>");
                }
            }
        }
    }
}
