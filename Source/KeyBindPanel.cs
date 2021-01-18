using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DebugMod
{
    public static class KeyBindPanel
    {
        private static CanvasPanel panel;
        private static int page = 0;

        private static Dictionary<string, List<string>> bindPages = new Dictionary<string, List<string>>();
        private static List<string> pageKeys;
        
        public static KeyCode keyWarning = KeyCode.None;

        // TODO: Refactor to allow rotating images
        public static void BuildMenu(GameObject canvas)
        {
                panel = new CanvasPanel(canvas, GUIController.Instance.images["HelpBG"], new Vector2(1123, 456), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["HelpBG"].width, GUIController.Instance.images["HelpBG"].height));
                panel.AddText("Label", "Binds", new Vector2(130f, -25f), Vector2.zero, GUIController.Instance.trajanBold, 30);

                panel.AddText("Category", "", new Vector2(25f, 25f), Vector2.zero, GUIController.Instance.trajanNormal, 20);
                panel.AddText("Help", "", new Vector2(25f, 50f), Vector2.zero, GUIController.Instance.arial, 15);
                panel.AddButton("Page", GUIController.Instance.images["ButtonRect"], new Vector2(125, 250), Vector2.zero, NextClicked, new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height), GUIController.Instance.trajanBold, "# / #");


                panel.AddButton(
                        "NextPage",
                        GUIController.Instance.images["ScrollBarArrowRight"],
                        new Vector2(223, 254),
                        Vector2.zero,
                        NextClicked,
                        new Rect(
                            0,
                            0,
                            GUIController.Instance.images["ScrollBarArrowRight"].width,
                            GUIController.Instance.images["ScrollBarArrowRight"].height)
                    );
                panel.AddButton(
                        "PrevPage",
                        GUIController.Instance.images["ScrollBarArrowLeft"],
                        new Vector2(95, 254),
                        Vector2.zero,
                        NextClicked,
                        new Rect(
                            0,
                            0,
                            GUIController.Instance.images["ScrollBarArrowLeft"].width,
                            GUIController.Instance.images["ScrollBarArrowLeft"].height)
                    );

            for (int i = 0; i < 11; i++)
            {
                panel.AddButton(i.ToString(), GUIController.Instance.images["Scrollbar_point"], new Vector2(300f, 45f + 17.5f * i), Vector2.zero, ChangeBind, new Rect(0, 0, GUIController.Instance.images["Scrollbar_point"].width, GUIController.Instance.images["Scrollbar_point"].height));
            }

            //Build pages based on categories
            foreach (KeyValuePair<string, Pair> bindable in DebugMod.bindMethods)
            {
                string name = bindable.Key;
                string cat = (string)bindable.Value.First;

                if (!bindPages.ContainsKey(cat)) bindPages.Add(cat, new List<string>());
                bindPages[cat].Add(name);
            }

            pageKeys = bindPages.Keys.ToList();

            panel.GetText("Category").UpdateText(pageKeys[page]);
            panel.GetButton("Page").UpdateText((page + 1) + " / " + pageKeys.Count);
            UpdateHelpText();
        }

        public static void UpdateHelpText()
        {
            if (page < 0 || page >= pageKeys.Count) return;

            string cat = pageKeys[page];
            List<string> helpPage = bindPages[cat];

            string updatedText = "";

            foreach (string bindStr in helpPage)
            {
                updatedText += bindStr + " - ";

                if (DebugMod.settings.binds.ContainsKey(bindStr))
                {
                    KeyCode code = ((KeyCode)DebugMod.settings.binds[bindStr]);

                    if (code != KeyCode.None)
                    {
                        updatedText += ((KeyCode)DebugMod.settings.binds[bindStr]).ToString();
                    }
                    else
                    {
                        updatedText += "WAITING";
                    }
                }
                else
                {
                    updatedText += "UNBOUND";
                }

                updatedText += "\n";
            }

            panel.GetText("Help").UpdateText(updatedText);
        }

        private static void NextClicked(string buttonName)
        {
            if (buttonName.StartsWith("Prev"))
            {
                page--;
                if (page < 0) page = pageKeys.Count - 1;
            }
            else
            {
                page++;
                if (page >= pageKeys.Count) page = 0;
            }

            panel.GetText("Category").UpdateText(pageKeys[page]);
            panel.GetButton("Page").UpdateText((page + 1) + " / " + pageKeys.Count);
            UpdateHelpText();
        }

        private static void ChangeBind(string buttonName)
        {
            int num = Convert.ToInt32(buttonName);

            if (num < 0 || num >= bindPages[pageKeys[page]].Count)
            {
                DebugMod.instance.LogWarn("Invalid bind change button clicked. Should not be possible");
                return;
            }

            string bindName = bindPages[pageKeys[page]][num];

            if (DebugMod.settings.binds.ContainsKey(bindName))
            {
                DebugMod.settings.binds[bindName] = (int)KeyCode.None;
            }
            else
            {
                DebugMod.settings.binds.Add(bindName, (int)KeyCode.None);
            }

            UpdateHelpText();
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

            if (DebugMod.settings.HelpPanelVisible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!DebugMod.settings.HelpPanelVisible && panel.active)
            {
                panel.SetActive(false, true);
            }

            if (panel.active && page >= 0 && page < pageKeys.Count)
            {
                for (int i = 0; i < 11; i++)
                {
                    panel.GetButton(i.ToString()).SetActive(bindPages[pageKeys[page]].Count > i);
                }
            }
        }
    }
}
