using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DebugMod
{
    public static class Console
    {
        private static CanvasPanel panel;
        private static List<string> history = new List<string>();
        private static Vector2 scrollPosition = Vector2.zero;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.Instance.images["ConsoleBg"], new Vector2(1275, 800), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["ConsoleBg"].width, GUIController.Instance.images["ConsoleBg"].height));
            panel.AddText("Console", "", new Vector2(10f, 25f), Vector2.zero, GUIController.Instance.arial);
            panel.FixRenderOrder();

            GUIController.Instance.arial.RequestCharactersInTexture("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/? ", 13);
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

            if (DebugMod.settings.ConsoleVisible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!DebugMod.settings.ConsoleVisible && panel.active)
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
        }

        public static void Reset()
        {
            history.Clear();
            scrollPosition = Vector2.zero;
        }

        public static void AddLine(string chatLine)
        {
            while (history.Count > 1000)
            {
                history.RemoveAt(0);
            }

            int wrap = WrapIndex(GUIController.Instance.arial, 13, chatLine);

            while (wrap != -1)
            {
                int index = chatLine.LastIndexOf(' ', wrap, wrap);

                if (index != -1)
                {
                    history.Add(chatLine.Substring(0, index));
                    chatLine = chatLine.Substring(index + 1);
                    wrap = WrapIndex(GUIController.Instance.arial, 13, chatLine);
                }
                else
                {
                    break;
                }
            }

            history.Add(chatLine);

            scrollPosition.y = scrollPosition.y + 50f;
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
                DebugMod.Instance.LogError("[CONSOLE] Unable to write console history: " + arg);
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
