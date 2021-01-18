using UnityEngine;
using InControl;
using System.Collections.Generic;

namespace DebugMod
{
    public static class SaveStatesPanel
    {
        private static CanvasPanel panel;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(
                canvas,
                GUIController.Instance.images["BlankVertical"],
                new Vector2(570f, 230f),
                Vector2.zero,
                new Rect(
                    0f,
                    0f,
                    GUIController.Instance.images["BlankVertical"].width,
                    GUIController.Instance.images["BlankVertical"].height
                )
            );

            //Labels
            panel.AddText("Slot0", "0", new Vector2(10f, 20f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot1", "1", new Vector2(10f, 40f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot2", "2", new Vector2(10f, 60f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot3", "3", new Vector2(10f, 80f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot4", "4", new Vector2(10f, 100f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot5", "5", new Vector2(10f, 120f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot6", "6", new Vector2(10f, 140f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot7", "7", new Vector2(10f, 160f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot8", "8", new Vector2(10f, 180f), Vector2.zero, GUIController.Instance.arial, 15);
            panel.AddText("Slot9", "9", new Vector2(10f, 200f), Vector2.zero, GUIController.Instance.arial, 15);
            
            //Values
            panel.AddText("0", "", new Vector2(50f, 20f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("1", "", new Vector2(50f, 40f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("2", "", new Vector2(50f, 60f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("3", "", new Vector2(50f, 80f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("4", "", new Vector2(50f, 100f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("5", "", new Vector2(50f, 120f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("6", "", new Vector2(50f, 140f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("7", "", new Vector2(50f, 160f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("8", "", new Vector2(50f, 180f), Vector2.zero, GUIController.Instance.trajanNormal);
            panel.AddText("9", "", new Vector2(50f, 200f), Vector2.zero, GUIController.Instance.trajanNormal);
           
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

            if (DebugMod.settings.SaveStatePanelVisible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!DebugMod.settings.SaveStatePanelVisible && panel.active)
            {
                panel.SetActive(false, true);
            }

            if (panel.active)
            {
                panel.GetText("0").UpdateText("Open");
                panel.GetText("1").UpdateText("Open");
                panel.GetText("2").UpdateText("Open");
                panel.GetText("4").UpdateText("Open");
                panel.GetText("5").UpdateText("Open");
                panel.GetText("6").UpdateText("Open");
                panel.GetText("7").UpdateText("Open");
                panel.GetText("8").UpdateText("Open");
                panel.GetText("9").UpdateText("Open");

                foreach (KeyValuePair<int, string[]> entry in SaveStateManager.GetSaveStatesInfo())
                {
                    panel.GetText(entry.Key.ToString()).UpdateText(string.Format("{0}\n{1}", entry.Value[2], entry.Value[1]));
                }
            }
        }

        private static string GetStringForBool(bool b)
        {
            return b ? "✓" : "X";
        }
    }
}
