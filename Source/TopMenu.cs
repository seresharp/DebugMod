using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public class TopMenu
    {
        private GameObject canvas;
        private CanvasImage background;
        private Dictionary<string, CanvasButton> buttons = new Dictionary<string, CanvasButton>();

        public bool active;

        public TopMenu(GameObject parent, Font font)
        {
            canvas = parent;

            buttons.Add("Hide Menu", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1138f + 45f, 38f + 13.5f), Vector2.zero, font, "Hide Menu"));
            buttons.Add("Kill All", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1238f + 45f, 38f + 13.5f), Vector2.zero, font, "Kill All"));
            buttons.Add("Set Spawn", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1338f + 45f, 38f + 13.5f), Vector2.zero, font, "Set Spawn"));
            buttons.Add("Respawn", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1438f + 45f, 38f + 13.5f), Vector2.zero, font, "Respawn"));
            buttons.Add("Dump Log", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1538f + 45f, 38f + 13.5f), Vector2.zero, font, "Dump Log"));
            buttons.Add("Cheats", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1138f + 45f, 78f + 13.5f), Vector2.zero, font, "Cheats"));
            buttons.Add("Charms", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1238f + 45f, 78f + 13.5f), Vector2.zero, font, "Charms"));
            buttons.Add("Skills", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1338f + 45f, 78f + 13.5f), Vector2.zero, font, "Skills"));
            buttons.Add("Items", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1438f + 45f, 78f + 13.5f), Vector2.zero, font, "Items"));
            buttons.Add("Bosses", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1538f + 45f, 78f + 13.5f), Vector2.zero, font, "Bosses"));
            buttons.Add("DreamGate", new CanvasButton(canvas, GUIController.instance.images["ButtonRect"], new Vector2(1638f + 45f, 78f + 13.5f), Vector2.zero, font, "DreamGate"));

            buttons["Hide Menu"].AddClickEvent(HideMenuClicked);
            buttons["Kill All"].AddClickEvent(KillAllClicked);
            buttons["Set Spawn"].AddClickEvent(SetSpawnClicked);
            buttons["Respawn"].AddClickEvent(RespawnClicked);
            buttons["Dump Log"].AddClickEvent(DumpLogClicked);
            buttons["Cheats"].AddClickEvent(CheatsClicked);
            buttons["Charms"].AddClickEvent(CharmsClicked);
            buttons["Skills"].AddClickEvent(SkillsClicked);
            buttons["Items"].AddClickEvent(ItemsClicked);
            buttons["Bosses"].AddClickEvent(BossesClicked);
            buttons["DreamGate"].AddClickEvent(DreamGateClicked);

            background = new CanvasImage(canvas, GUIController.instance.images["ButtonsMenuBG"], new Vector2(1920f - 414f, 80f), Vector2.zero);
            background.SetRenderIndex(0);
        }

        public void SetActive(bool b)
        {
            foreach (CanvasButton button in buttons.Values)
            {
                button.SetActive(b);
            }

            background.SetActive(b);

            active = b;
        }

        private void HideMenuClicked()
        {
            GUIController.instance.showMenus = false;
        }

        private void KillAllClicked()
        {
            PlayMakerFSM.BroadcastEvent("INSTA KILL");
            Console.AddLine("INSTA KILL broadcasted!");
        }

        private void SetSpawnClicked()
        {
            HeroController.instance.SetHazardRespawn(DebugMod.refKnight.transform.position, false);
            Console.AddLine("Manual respawn point on this map set to" + DebugMod.refKnight.transform.position.ToString());
        }

        private void RespawnClicked()
        {
            if (GameManager.instance.IsGameplayScene() && !HeroController.instance.cState.dead && PlayerData.instance.health > 0)
            {
                if (UIManager.instance.uiState.ToString() == "PAUSED")
                {
                    UIManager.instance.TogglePauseGame();
                    GameManager.instance.HazardRespawn();
                    Console.AddLine("Closing Pause Menu and respawning...");
                    return;
                }
                if (UIManager.instance.uiState.ToString() == "PLAYING")
                {
                    HeroController.instance.RelinquishControl();
                    GameManager.instance.HazardRespawn();
                    HeroController.instance.RegainControl();
                    Console.AddLine("Respawn signal sent");
                    return;
                }
                Console.AddLine("Respawn requested in some weird conditions, abort, ABORT");
            }
        }

        private void DumpLogClicked()
        {
            Console.AddLine("Saving console log...");
            Console.SaveHistory();
        }

        private void CheatsClicked()
        {

        }

        private void CharmsClicked()
        {

        }

        private void SkillsClicked()
        {

        }

        private void ItemsClicked()
        {

        }

        private void BossesClicked()
        {

        }

        private void DreamGateClicked()
        {

        }
    }
}
