using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public static class HelpPanel
    {
        private static CanvasPanel panel;
        private static int page = 1;

        public static bool visible;

        public static void BuildMenus(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.instance.images["HelpBG"], new Vector2(1123, 456), Vector2.zero, new Rect(0, 0, GUIController.instance.images["HelpBG"].width, GUIController.instance.images["HelpBG"].height));
            panel.AddText("Label", "Help", new Vector2(130f, -25f), Vector2.zero, GUIController.instance.trajanBold, 30);

            panel.AddText("Help", "F1 - Toggle All\nF2 - Toggle info\nF3 - Toggle top right\nF4 - Toggle console\nF5 - Force pause\nF6 - Hazard respawn\nF7 - Set respawn\nF8 - Force camera follow\nF9 - Toggle enemy panel\nF10 - Self damage\n\n` - Toggle Help", new Vector2(75f, 50f), Vector2.zero, GUIController.instance.trajanNormal);
            panel.AddButton("Next", GUIController.instance.images["ButtonRect"], new Vector2(125, 250), Vector2.zero, NextClicked, new Rect(0, 0, GUIController.instance.images["ButtonRect"].width, GUIController.instance.images["ButtonRect"].height), GUIController.instance.trajanBold, "Next");
        }

        private static void NextClicked(string buttonName)
        {
            if (page == 1)
            {
                page = 2;

                panel.GetButton("Next").UpdateText("Prev");
                panel.GetText("Help").UpdateText("Plus - Nail damage +4\nMinus - Nail damage -4\nNumpad+ - Timescale up\nNumpad- - Timescale down\nHome - Toggle hero light\nInsert - Toggle vignette\nPageUP - Zoom in\nPageDN - Zoom out\nEnd - Reset zoom\nDelete - Toggle HUD\nBackspc - Hide hero");
            }
            else
            {
                page = 1;

                panel.GetButton("Next").UpdateText("Next");
                panel.GetText("Help").UpdateText("F1 - Toggle All\nF2 - Toggle info\nF3 - Toggle top right\nF4 - Toggle console\nF5 - Force pause\nF6 - Hazard respawn\nF7 - Set respawn\nF8 - Force camera follow\nF9 - Toggle enemy panel\nF10 - Self damage\n\n` - Toggle Help");

            }
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
        }
    }
}
