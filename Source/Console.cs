using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using GlobalEnums;

namespace DebugMod
{
    public static class Console
    {
        public static bool visible;

        private static CanvasPanel panel;
        private static string GUIString = "";
        private static float alpha = 1f;
        private static List<string> history = new List<string>();
        private static Vector2 scrollPosition = Vector2.zero;
        private static float lastTime;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.instance.images["ConsoleBg"], new Vector2(1275, 800), Vector2.zero, new Rect(0, 0, GUIController.instance.images["ConsoleBg"].width, GUIController.instance.images["ConsoleBg"].height));

            panel.AddText("Console", "", new Vector2(10f, 25f), Vector2.zero, GUIController.instance.arial);
            panel.AddText("NoConsole", "", new Vector2(10f, 180f), Vector2.zero, GUIController.instance.arial);

            panel.FixRenderOrder();

            GUIController.instance.arial.RequestCharactersInTexture("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/? ", 13);
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

            if (panel.active)
            {
                string consoleString = "";
                int lineCount = 0;

                for (int i = history.Count - 1; i >= 0; i--)
                {
                    if (lineCount >= 8) break;
                    consoleString = history[i] + "\n" + consoleString;
                    lineCount++;
                }

                panel.GetText("Console").UpdateText(consoleString);
            }

            if (!panel.active)
            {
                float delta = Time.realtimeSinceStartup - lastTime;
                lastTime = Time.realtimeSinceStartup;

                alpha -= delta * .5f;

                if (alpha > 0 && DebugMod.gm.IsGameplayScene())
                {
                    Color c = Color.white;
                    c.a = alpha;

                    panel.GetText("NoConsole").SetActive(true);
                    panel.GetText("NoConsole").UpdateText(GUIString);
                    panel.GetText("NoConsole").SetTextColor(c);
                }
                else
                {
                    panel.GetText("NoConsole").SetActive(false);
                }
            }
        }

        public static void Reset()
        {
            history.Clear();
            alpha = 1f;
            GUIString = "";
            lastTime = Time.realtimeSinceStartup;
            scrollPosition = Vector2.zero;
        }

        public static void AddLine(string chatLine)
        {
            while (history.Count > 1000)
            {
                history.RemoveAt(0);
            }

            int wrap = WrapIndex(GUIController.instance.arial, 13, chatLine);

            while (wrap != -1)
            {
                int index = chatLine.LastIndexOf(' ', wrap, wrap);

                if (index != -1)
                {
                    history.Add(chatLine.Substring(0, index));
                    chatLine = chatLine.Substring(index + 1);
                    wrap = WrapIndex(GUIController.instance.arial, 13, chatLine);
                }
                else
                {
                    break;
                }
            }

            history.Add(chatLine);

            scrollPosition.y = scrollPosition.y + 50f;
            alpha = 1f;
            lastTime = Time.realtimeSinceStartup;

            if (!visible)
            {
                GUIString = chatLine;
            }
        }

        public static void SaveHistory()
        {
            try
            {
                File.WriteAllLines("console.txt", history.ToArray());
                Console.AddLine("Written history to console.txt");
            }
            catch (Exception arg)
            {
                Modding.ModHooks.ModLog("[DEBUG MOD] [CONSOLE] Unable to write console history: " + arg);
                Console.AddLine("Unable to write console history");
            }
        }

        private static int WrapIndex(Font font, int fontSize, string message)
        {
            int totalLength = 0;

            CharacterInfo characterInfo;

            char[] arr = message.ToCharArray();

            for (int i = 0; i < arr.Length; i++)
            {
                char c = arr[i];
                font.GetCharacterInfo(c, out characterInfo, fontSize);
                totalLength += characterInfo.advance;

                if (totalLength >= 564) return i;
            }

            return -1;
        }
    }
}
