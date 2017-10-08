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

        private static string GUIString = "";
        private static float alpha = 1f;
        private static List<string> history = new List<string>();
        private static Vector2 scrollPosition = Vector2.zero;
        private static float lastTime;

        public static void BuildMenu(GameObject canvas)
        {

        }

        public static void Reset()
        {
            history.Clear();
            alpha = 1f;
            GUIString = "";
            scrollPosition = Vector2.zero;

            AddLine("New session started " + DateTime.Now.ToString());
        }

        public static void AddLine(string chatLine)
        {
            if (history.Count > 1000)
            {
                history.RemoveAt(1);
            }

            history.Add(chatLine);
            scrollPosition.y = scrollPosition.y + 50f;
            alpha = 1f;

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
            }
            catch (Exception arg)
            {
                Modding.ModHooks.ModLog("[DEBUG MOD] [CONSOLE] Unable to write console history: " + arg);
            }
        }

        public static void UpdateGUI(bool forceInvisible)
        {
            float deltaTime = Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;

            if ((!visible || forceInvisible) && alpha > 0f)
            {
                alpha -= deltaTime * .3f;

                if (alpha <= 0f)
                {
                    alpha = 0f;
                    GUIString = "";
                }
            }

            if ((visible || UIManager.instance.uiState == UIState.PAUSED) && !forceInvisible)
            {
                GUI.skin.label.fontStyle = FontStyle.Bold;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GUI.skin.label.fontSize = 18;
                new GUIStyle().fontSize = 18;
                GUI.BeginGroup(new Rect(1125f, 720f, 820f, 360f));
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(800f),
                    GUILayout.MaxHeight(345f),
                    GUILayout.ExpandHeight(false)
                });
                if (history.Count <= 150)
                {
                    GUILayout.Label(string.Join("\n", history.ToArray()), new GUILayoutOption[]
                    {
                        GUILayout.ExpandHeight(true)
                    });
                }
                else
                {
                    GUILayout.Label(string.Join("\n", history.GetRange(history.Count - 150, 150).ToArray()), new GUILayoutOption[]
                    {
                        GUILayout.ExpandHeight(true)
                    });
                }
                GUILayout.EndScrollView();
                GUI.EndGroup();
            }
            else if (((!visible && UIManager.instance.uiState == UIState.PLAYING) || (forceInvisible && UIManager.instance.uiState == UIState.PAUSED)) && !string.IsNullOrEmpty(GUIString) && alpha > 0f)
            {
                Color color = GUI.color;
                GUI.color = new Color(1f, 1f, 1f, alpha);
                TextAnchor alignment9 = GUI.skin.label.alignment;
                GUI.skin.label.fontStyle = FontStyle.Bold;
                GUI.skin.label.alignment = TextAnchor.UpperRight;
                GUI.skin.label.fontSize = 18;
                GUI.Label(new Rect(1f, 1050f, 1915f, 30f), GUIString.ToString());
                GUI.color = color;
                GUI.skin.label.alignment = alignment9;
            }
        }
    }
}
