using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using GlobalEnums;

namespace DebugMod
{
    public static class EnemiesPanel
    {
        private static CanvasPanel panel;
        public static bool autoUpdate;
        private static float lastTime;
        public static List<EnemyData> enemyPool = new List<EnemyData>();
        private static GameObject parent;
        public static bool hpBars;
        public static bool hitboxes;
        public static MethodInfo takeDamage = typeof(HeroController).GetMethod("TakeDamage");
        public static ParameterInfo[] parameters = takeDamage.GetParameters();

        public static void BuildMenu(GameObject canvas)
        {
            parent = canvas;

            panel = new CanvasPanel(canvas, new Texture2D(1, 1), new Vector2(1920f - GUIController.Instance.images["EnemiesPBg"].width, 481f), Vector2.zero, new Rect(0, 0, 1, 1));

            panel.AddText("Panel Label", "Enemies", new Vector2(125f, -25f), Vector2.zero, GUIController.Instance.trajanBold, 30);

            panel.AddText("Enemy Names", "", new Vector2(90f, 20f), Vector2.zero, GUIController.Instance.arial);
            panel.AddText("Enemy HP", "", new Vector2(300f, 20f), Vector2.zero, GUIController.Instance.arial);

            panel.AddPanel("Pause", GUIController.Instance.images["EnemiesPBg"], Vector2.zero, Vector2.zero, new Rect(0, 0, GUIController.Instance.images["EnemiesPBg"].width, GUIController.Instance.images["EnemiesPBg"].height));
            panel.AddPanel("Play", GUIController.Instance.images["EnemiesBg"], new Vector2(57f, 0f), Vector2.zero, new Rect(0f, 0f, GUIController.Instance.images["EnemiesBg"].width, GUIController.Instance.images["EnemiesBg"].height));

            for (int i = 1; i <= 14; i++)
            {
                panel.GetPanel("Pause").AddButton("Del" + i, GUIController.Instance.images["ButtonDel"], new Vector2(20f, 20f + (i - 1) * 15f), new Vector2(12f, 12f), DelClicked, new Rect(0, 0, GUIController.Instance.images["ButtonDel"].width, GUIController.Instance.images["ButtonDel"].height));
                panel.GetPanel("Pause").AddButton("Clone" + i, GUIController.Instance.images["ButtonPlus"], new Vector2(40f, 20f + (i - 1) * 15f), new Vector2(12f, 12f), CloneClicked, new Rect(0, 0, GUIController.Instance.images["ButtonPlus"].width, GUIController.Instance.images["ButtonPlus"].height));
                panel.GetPanel("Pause").AddButton("Inf" + i, GUIController.Instance.images["ButtonInf"], new Vector2(60f, 20f + (i - 1) * 15f), new Vector2(12f, 12f), InfClicked, new Rect(0, 0, GUIController.Instance.images["ButtonInf"].width, GUIController.Instance.images["ButtonInf"].height));
            }

            panel.GetPanel("Pause").AddButton("Collision", GUIController.Instance.images["ButtonRect"], new Vector2(30f, 250f), Vector2.zero, CollisionClicked, new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height), GUIController.Instance.trajanBold, "Collision");
            panel.GetPanel("Pause").AddButton("HP Bars", GUIController.Instance.images["ButtonRect"], new Vector2(125f, 250f), Vector2.zero, HPBarsClicked, new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height), GUIController.Instance.trajanBold, "HP Bars");
            panel.GetPanel("Pause").AddButton("Auto", GUIController.Instance.images["ButtonRect"], new Vector2(220f, 250f), Vector2.zero, AutoClicked, new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height), GUIController.Instance.trajanBold, "Auto");
            panel.GetPanel("Pause").AddButton("Scan", GUIController.Instance.images["ButtonRect"], new Vector2(315f, 250f), Vector2.zero, ScanClicked, new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height), GUIController.Instance.trajanBold, "Scan");

            panel.FixRenderOrder();
        }

        private static void DelClicked(string buttonName)
        {
            int num = Convert.ToInt32(buttonName.Substring(3));
            EnemyData dat = enemyPool.FindAll(ed => ed.gameObject != null && ed.gameObject.activeSelf)[num - 1];

            Console.AddLine("Destroying enemy: " + dat.gameObject.name);
            UnityEngine.Object.DestroyImmediate(dat.gameObject);
        }

        private static void CloneClicked(string buttonName)
        {
            int num = Convert.ToInt32(buttonName.Substring(5));
            EnemyData dat = enemyPool.FindAll(ed => ed.gameObject != null && ed.gameObject.activeSelf)[num - 1];

            GameObject gameObject2 = UnityEngine.Object.Instantiate(dat.gameObject, dat.gameObject.transform.position, dat.gameObject.transform.rotation) as GameObject;
            Component component = gameObject2.GetComponent<tk2dSprite>();
            PlayMakerFSM playMakerFSM2 = FSMUtility.LocateFSM(gameObject2, dat.FSM.FsmName);
            int value8 = playMakerFSM2.FsmVariables.GetFsmInt("HP").Value;
            enemyPool.Add(new EnemyData(value8, playMakerFSM2, component, parent, gameObject2));
            Console.AddLine("Cloning enemy as: " + gameObject2.name);
        }

        private static void InfClicked(string buttonName)
        {
            int num = Convert.ToInt32(buttonName.Substring(3));
            EnemyData dat = enemyPool.FindAll(ed => ed.gameObject != null && ed.gameObject.activeSelf)[num - 1];

            dat.SetHP(9999);
            Console.AddLine("HP for enemy: " + dat.gameObject.name + " is now 9999");
        }

        private static void CollisionClicked(string buttonName)
        {
            BindableFunctions.ToggleEnemyCollision();
        }

        private static void HPBarsClicked(string buttonName)
        {
            BindableFunctions.ToggleEnemyHPBars();
        }

        private static void AutoClicked(string buttonName)
        {
            BindableFunctions.ToggleEnemyAutoScan();
        }

        private static void ScanClicked(string buttonName)
        {
            BindableFunctions.EnemyScan();
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

            if (DebugMod.settings.EnemiesPanelVisible && !panel.active)
            {
                panel.SetActive(true, false);
            }
            else if (!DebugMod.settings.EnemiesPanelVisible && panel.active)
            {
                panel.SetActive(false, true);
            }

            if (DebugMod.settings.EnemiesPanelVisible && UIManager.instance.uiState == UIState.PLAYING && (panel.GetPanel("Pause").active || !panel.GetPanel("Play").active))
            {
                panel.GetPanel("Pause").SetActive(false, true);
                panel.GetPanel("Play").SetActive(true, false);
            }
            else if (DebugMod.settings.EnemiesPanelVisible && UIManager.instance.uiState == UIState.PAUSED && (!panel.GetPanel("Pause").active || panel.GetPanel("Play").active))
            {
                panel.GetPanel("Pause").SetActive(true, false);
                panel.GetPanel("Play").SetActive(false, true);
            }

            if (!panel.active && enemyPool.Count > 0)
            {
                Reset();
            }

            if (panel.active)
            {
                EnemiesPanel.CheckForAutoUpdate();

                string enemyNames = "";
                string enemyHP = "";
                int enemyCount = 0;

                for (int i = 0; i < enemyPool.Count; i++)
                {
                    EnemyData dat = enemyPool[i];
                    GameObject obj = dat.gameObject;

                    if (obj == null || !obj.activeSelf)
                    {
                        if (obj == null)
                        {
                            dat.hpBar.Destroy();
                            dat.hitbox.Destroy();
                            enemyPool.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            dat.hpBar.SetPosition(new Vector2(-1000f, -1000f));
                        }
                    }
                    else
                    {
                        int hp = dat.FSM.FsmVariables.GetFsmInt("HP").Value;

                        if (hp != dat.HP)
                        {
                            dat.SetHP(hp);

                            if (hpBars)
                            {
                                Texture2D tex = new Texture2D(120, 40);

                                for (int x = 0; x < 120; x++)
                                {
                                    for (int y = 0; y < 40; y++)
                                    {
                                        if (x < 3 || x > 116 || y < 3 || y > 36)
                                        {
                                            tex.SetPixel(x, y, Color.black);
                                        }
                                        else
                                        {
                                            if ((float)hp / (float)dat.maxHP >= (x - 2f) / 117f)
                                            {
                                                tex.SetPixel(x, y, Color.red);
                                            }
                                            else
                                            {
                                                tex.SetPixel(x, y, new Color(255f, 255f, 255f, 0f));
                                            }
                                        }
                                    }
                                }

                                tex.Apply();

                                dat.hpBar.UpdateBackground(tex, new Rect(0, 0, 120, 40));
                            }
                        }

                        if (hitboxes)
                        {
                            if ((dat.Spr as tk2dSprite).boxCollider2D != null)
                            {
                                BoxCollider2D boxCollider2D = (dat.Spr as tk2dSprite).boxCollider2D;
                                Bounds bounds2 = boxCollider2D.bounds;

                                float width = Math.Abs(bounds2.max.x - bounds2.min.x);
                                float height = Math.Abs(bounds2.max.y - bounds2.min.y);
                                Vector2 position = Camera.main.WorldToScreenPoint(boxCollider2D.transform.position + new Vector3(boxCollider2D.offset.x, boxCollider2D.offset.y, 0f));
                                Vector2 size = Camera.main.WorldToScreenPoint(boxCollider2D.transform.position + new Vector3(width, height, 0));
                                size -= position;

                                Quaternion rot = boxCollider2D.transform.rotation;
                                rot.eulerAngles = new Vector3(Mathf.Round(rot.eulerAngles.x / 90) * 90, Mathf.Round(rot.eulerAngles.y / 90) * 90, Mathf.Round(rot.eulerAngles.z / 90) * 90);
                                Vector2 pivot = Camera.main.WorldToScreenPoint(boxCollider2D.transform.position);
                                Vector2 pointA = Camera.main.WorldToScreenPoint((Vector2)boxCollider2D.transform.position + boxCollider2D.offset);
                                pointA.x -= size.x / 2f;
                                pointA.y -= size.y / 2f;
                                Vector2 pointB = pointA + size;

                                pointA = (Vector2)(rot * (pointA - pivot)) + pivot;
                                pointB = (Vector2)(rot * (pointB - pivot)) + pivot;

                                position = new Vector2(pointA.x < pointB.x ? pointA.x : pointB.x, pointA.y > pointB.y ? pointA.y : pointB.y);
                                size = new Vector2(Math.Abs(pointA.x - pointB.x), Math.Abs(pointA.y - pointB.y));

                                size.x *= 1920f / Screen.width;
                                size.y *= 1080f / Screen.height;

                                position.x *= 1920f / Screen.width;
                                position.y *= 1080f / Screen.height;
                                position.y = 1080f - position.y;
                                
                                dat.hitbox.SetPosition(position);
                                dat.hitbox.ResizeBG(size);
                            }

                            if (!dat.hitbox.active)
                            {
                                dat.hitbox.SetActive(true, true);
                            }
                        }

                        if (hpBars)
                        {
                            Vector2 enemyPos = Camera.main.WorldToScreenPoint(obj.transform.position);
                            enemyPos.x *= 1920f / Screen.width;
                            enemyPos.y *= 1080f / Screen.height;

                            enemyPos.y = 1080f - enemyPos.y;

                            Bounds bounds = (dat.Spr as tk2dSprite).GetBounds();
                            enemyPos.y -= (Camera.main.WorldToScreenPoint(bounds.max).y * (1080f / Screen.height) - Camera.main.WorldToScreenPoint(bounds.min).y * (1080f / Screen.height)) / 2f;
                            enemyPos.x -= 60;

                            dat.hpBar.SetPosition(enemyPos);
                            dat.hpBar.GetText("HP").UpdateText(dat.FSM.FsmVariables.GetFsmInt("HP").Value + "/" + dat.maxHP);

                            if (!dat.hpBar.active)
                            {
                                dat.hpBar.SetActive(true, true);
                            }
                        }

                        if (!hpBars && dat.hpBar.active)
                        {
                            dat.hpBar.SetActive(false, true);
                        }

                        if (!hitboxes && dat.hitbox.active)
                        {
                            dat.hitbox.SetActive(false, true);
                        }

                        if (++enemyCount <= 14)
                        {
                            enemyNames += obj.name + "\n";
                            enemyHP += dat.FSM.FsmVariables.GetFsmInt("HP").Value + "/" + dat.maxHP + "\n";
                        }
                    }
                }

                if (panel.GetPanel("Pause").active)
                {
                    for (int i = 1; i <= 14; i++)
                    {
                        if (i <= enemyCount)
                        {
                            panel.GetPanel("Pause").GetButton("Del" + i).SetActive(true);
                            panel.GetPanel("Pause").GetButton("Clone" + i).SetActive(true);
                            panel.GetPanel("Pause").GetButton("Inf" + i).SetActive(true);
                        }
                        else
                        {
                            panel.GetPanel("Pause").GetButton("Del" + i).SetActive(false);
                            panel.GetPanel("Pause").GetButton("Clone" + i).SetActive(false);
                            panel.GetPanel("Pause").GetButton("Inf" + i).SetActive(false);
                        }
                    }

                    panel.GetPanel("Pause").GetButton("Collision").SetTextColor(hitboxes ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                    panel.GetPanel("Pause").GetButton("HP Bars").SetTextColor(hpBars ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                    panel.GetPanel("Pause").GetButton("Auto").SetTextColor(autoUpdate ? new Color(244f / 255f, 127f / 255f, 32f / 255f) : Color.white);
                }

                if (enemyCount > 14)
                {
                    enemyNames += "And " + (enemyCount - 14) + " more";
                }

                panel.GetText("Enemy Names").UpdateText(enemyNames);
                panel.GetText("Enemy HP").UpdateText(enemyHP);
            }
        }

        public static void Reset()
        {
            foreach(EnemyData dat in enemyPool)
            {
                dat.hitbox.Destroy();
                dat.hpBar.Destroy();
            }
            enemyPool.Clear();
        }

        public static bool Ignore(string name)
        {
            if (name.IndexOf("Hornet Barb", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Needle Tink", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("worm", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Laser Turret", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Deep Spikes", StringComparison.OrdinalIgnoreCase) >= 0) return true;

            return false;
        }

        public static void RefreshEnemyList()
        {
            if (DebugMod.settings.EnemiesPanelVisible)
            {
                GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName(DebugMod.GetSceneName()).GetRootGameObjects();
                if (rootGameObjects != null)
                {
                    foreach (GameObject gameObject in rootGameObjects)
                    {
                        if ((gameObject.layer == 11 || gameObject.layer == 17 || gameObject.tag == "Boss") && !Ignore(gameObject.name))
                        {
                            PlayMakerFSM playMakerFSM = FSMUtility.LocateFSM(gameObject, "health_manager_enemy");
                            Component component = gameObject.GetComponent<tk2dSprite>();
                            if (playMakerFSM == null)
                            {
                                playMakerFSM = FSMUtility.LocateFSM(gameObject, "health_manager");
                            }
                            int num3 = gameObject.name.IndexOf("grass", StringComparison.OrdinalIgnoreCase);
                            int num2 = gameObject.name.IndexOf("hopper", StringComparison.OrdinalIgnoreCase);
                            if (num3 >= 0 && num2 >= 0)
                            {
                                component = gameObject.transform.FindChild("Sprite").gameObject.gameObject.GetComponent<tk2dSprite>();
                            }
                            if (playMakerFSM != null)
                            {
                                if (component == null)
                                {
                                    component = null;
                                }
                                int value = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                                enemyPool.Add(new EnemyData(value, playMakerFSM, component, parent, gameObject));
                            }
                        }
                        enemyDescendants(gameObject.transform);
                    }
                }
                if (enemyPool.Count > 0)
                {
                    Console.AddLine("Enemy data filled, entries added: " + enemyPool.Count);
                }
                enemyUpdate(200f);
            }
        }

        private static void SelfDamage()
        {
            BindableFunctions.SelfDamage();
        }

        private static void CheckForAutoUpdate()
        {
            if (!autoUpdate) return;

            float deltaTime = Time.realtimeSinceStartup - lastTime;

            if (deltaTime >= 2f)
            {
                lastTime = Time.realtimeSinceStartup;
                enemyUpdate(0f);
            }
        }

        public static void enemyUpdate(float boxSize)
        {
            if (autoUpdate)
            {
                boxSize = 50f;
            }

            if (DebugMod.settings.EnemiesPanelVisible && HeroController.instance != null && !HeroController.instance.cState.transitioning && DebugMod.GM.IsGameplayScene())
            {
                int count = enemyPool.Count;
                int layerMask = 133120;
                Collider2D[] array = Physics2D.OverlapBoxAll(DebugMod.RefKnight.transform.position, new Vector2(boxSize, boxSize), 1f, layerMask);
                if (array != null)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        PlayMakerFSM playMakerFSM = FSMUtility.LocateFSM(array[i].gameObject, "health_manager_enemy");
                        if (playMakerFSM == null)
                        {
                            FSMUtility.LocateFSM(array[i].gameObject, "health_manager");
                        }
                        if (playMakerFSM && !enemyPool.Any(ed => ed.gameObject == array[i].gameObject) && !Ignore(array[i].gameObject.name))
                        {
                            Component component = array[i].gameObject.GetComponent<tk2dSprite>();
                            if (component == null)
                            {
                                component = null;
                            }
                            int value = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                            enemyPool.Add(new EnemyData(value, playMakerFSM, component, parent, array[i].gameObject));
                        }
                    }
                    if (enemyPool.Count > count)
                    {
                        Console.AddLine("EnemyList updated: +" + (enemyPool.Count - count));
                        return;
                    }
                }
            }
            else if (autoUpdate && (!DebugMod.settings.EnemiesPanelVisible || !GameManager.instance.IsGameplayScene() || HeroController.instance == null))
            {
                autoUpdate = false;
                Console.AddLine("Cancelling enemy auto-scan due to weird conditions");
            }
        }

        private static void enemyDescendants(Transform transform)
        {
            List<Transform> list = new List<Transform>();
            foreach (object obj in transform)
            {
                Transform transform2 = (Transform)obj;
                if ((transform2.gameObject.layer == 11 || transform2.gameObject.layer == 17) && !enemyPool.Any(ed => ed.gameObject == transform2.gameObject) && !Ignore(transform2.gameObject.name))
                {
                    PlayMakerFSM playMakerFSM = FSMUtility.LocateFSM(transform2.gameObject, "health_manager_enemy");
                    if (playMakerFSM == null)
                    {
                        playMakerFSM = FSMUtility.LocateFSM(GUIController.Instance.gameObject, "health_manager");
                    }
                    Component component = transform2.gameObject.GetComponent<tk2dSprite>();
                    if (playMakerFSM)
                    {
                        if (component == null)
                        {
                            component = null;
                        }
                        int value = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                        enemyPool.Add(new EnemyData(value, playMakerFSM, component, parent, transform2.gameObject));
                    }
                }
                list.Add(transform2);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].childCount > 0)
                {
                    foreach (object obj2 in list[i])
                    {
                        Transform transform3 = (Transform)obj2;
                        if ((transform3.gameObject.layer == 11 || transform3.gameObject.layer == 17) && !enemyPool.Any(ed => ed.gameObject == transform3.gameObject) && !Ignore(transform3.gameObject.name))
                        {
                            PlayMakerFSM playMakerFSM2 = FSMUtility.LocateFSM(transform3.gameObject, "health_manager_enemy");
                            if (playMakerFSM2 == null)
                            {
                                playMakerFSM2 = FSMUtility.LocateFSM(GUIController.Instance.gameObject, "health_manager");
                            }
                            Component component2 = transform3.gameObject.GetComponent<tk2dSprite>();
                            if (playMakerFSM2)
                            {
                                if (component2 == null)
                                {
                                    component2 = null;
                                }
                                int value2 = playMakerFSM2.FsmVariables.GetFsmInt("HP").Value;
                                enemyPool.Add(new EnemyData(value2, playMakerFSM2, component2, parent, transform3.gameObject));
                            }
                        }
                        list.Add(transform3);
                    }
                }
            }
        }
    }
}
