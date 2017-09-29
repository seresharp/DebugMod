using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GlobalEnums;

namespace DebugMod
{
    public static class EnemyController
    {
        public static bool enemyPanel;
        public static bool enemyCheck;
        public static Dictionary<GameObject, EnemyData> enemyPool = new Dictionary<GameObject, EnemyData>();

        private static Vector2 enemyScroll;
        private static bool autoUpdate;
        private static float lastTime;
        private static bool attachHP;
        private static bool showBox;
        private static int hpLimit;

        public static void Reset()
        {
            enemyPool.Clear();
            enemyCheck = false;
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

        public static void RefreshEnemyList(GameObject baseObj)
        {
            if (enemyPanel && !enemyCheck)
            {
                GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName(DebugMod.GetSceneName()).GetRootGameObjects();
                if (rootGameObjects != null)
                {
                    foreach (GameObject gameObject in rootGameObjects)
                    {
                        if ((gameObject.layer == 11 || gameObject.layer == 17) && !Ignore(gameObject.name))
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
                                enemyPool.Add(gameObject, new EnemyData(value, playMakerFSM, component));
                            }
                        }
                        enemyDescendants(gameObject.transform, baseObj);
                    }
                }
                if (enemyPool.Count > 0)
                {
                    Console.AddLine("Enemy data filled, entries added: " + enemyPool.Count);
                }
                enemyCheck = true;
                enemyUpdate(200f);
            }
        }

        public static void selfDamage()
        {
            if (PlayerData.instance.health <= 0 || HeroController.instance.cState.dead || !GameManager.instance.IsGameplayScene() || GameManager.instance.IsGamePaused() || HeroController.instance.cState.recoiling || HeroController.instance.cState.invulnerable)
            {
                Console.AddLine(string.Concat(new string[]
                {
                    "Unacceptable conditions for selfDamage(",
                    PlayerData.instance.health.ToString(),
                    ",",
                    HeroController.instance.cState.dead.ToString(),
                    ",",
                    GameManager.instance.IsGameplayScene().ToString(),
                    ",",
                    HeroController.instance.cState.recoiling.ToString(),
                    ",",
                    GameManager.instance.IsGamePaused().ToString(),
                    ",",
                    HeroController.instance.cState.invulnerable.ToString(),
                    ").",
                    " Pressed too many times at once?"
                }));
                return;
            }
            if (!enemyPanel)
            {
                Console.AddLine("Enable EnemyPanel for self-damage");
                return;
            }
            if (enemyPool.Count < 1)
            {
                Console.AddLine("Unable to locate a single enemy in the scene.");
                return;
            }
            System.Random random = new System.Random();
            if (HeroController.instance.cState.facingRight)
            {
                HeroController.instance.TakeDamage(enemyPool.ElementAt(random.Next(0, enemyPool.Count)).Key, CollisionSide.right);
                Console.AddLine("Attempting self-damage, right side");
                return;
            }
            HeroController.instance.TakeDamage(enemyPool.ElementAt(random.Next(0, enemyPool.Count)).Key, CollisionSide.left);
            Console.AddLine("Attempting self-damage, left side");
        }

        public static void CheckForAutoUpdate()
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

            if (enemyPanel && HeroController.instance != null && !HeroController.instance.cState.transitioning && DebugMod.gm.IsGameplayScene())
            {
                int count = enemyPool.Count;
                int layerMask = 133120;
                Collider2D[] array = Physics2D.OverlapBoxAll(DebugMod.refKnight.transform.position, new Vector2(boxSize, boxSize), 1f, layerMask);
                if (array != null)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        PlayMakerFSM playMakerFSM = FSMUtility.LocateFSM(array[i].gameObject, "health_manager_enemy");
                        if (playMakerFSM == null)
                        {
                            FSMUtility.LocateFSM(array[i].gameObject, "health_manager");
                        }
                        if (playMakerFSM && !enemyPool.ContainsKey(array[i].gameObject) && !Ignore(array[i].gameObject.name))
                        {
                            Component component = array[i].gameObject.GetComponent<tk2dSprite>();
                            if (component == null)
                            {
                                component = null;
                            }
                            int value = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                            enemyPool.Add(array[i].gameObject, new EnemyData(value, playMakerFSM, component));
                        }
                    }
                    if (enemyPool.Count > count)
                    {
                        Console.AddLine("EnemyList updated: +" + (enemyPool.Count - count));
                        return;
                    }
                }
            }
            else if (autoUpdate && (!enemyPanel || !GameManager.instance.IsGameplayScene() || HeroController.instance == null))
            {
                autoUpdate = false;
                Console.AddLine("Cancelling enemy auto-scan due to weird conditions");
            }
        }

        public static void enemyDescendants(Transform transform, GameObject baseObj)
        {
            List<Transform> list = new List<Transform>();
            foreach (object obj in transform)
            {
                Transform transform2 = (Transform)obj;
                if ((transform2.gameObject.layer == 11 || transform2.gameObject.layer == 17) && !enemyPool.ContainsKey(transform2.gameObject) && !Ignore(transform2.gameObject.name))
                {
                    PlayMakerFSM playMakerFSM = FSMUtility.LocateFSM(transform2.gameObject, "health_manager_enemy");
                    if (playMakerFSM == null)
                    {
                        playMakerFSM = FSMUtility.LocateFSM(baseObj, "health_manager");
                    }
                    Component component = transform2.gameObject.GetComponent<tk2dSprite>();
                    if (playMakerFSM)
                    {
                        if (component == null)
                        {
                            component = null;
                        }
                        int value = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                        enemyPool.Add(transform2.gameObject, new EnemyData(value, playMakerFSM, component));
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
                        if ((transform3.gameObject.layer == 11 || transform3.gameObject.layer == 17) && !enemyPool.ContainsKey(transform3.gameObject) && !Ignore(transform3.gameObject.name))
                        {
                            PlayMakerFSM playMakerFSM2 = FSMUtility.LocateFSM(transform3.gameObject, "health_manager_enemy");
                            if (playMakerFSM2 == null)
                            {
                                playMakerFSM2 = FSMUtility.LocateFSM(GUIController.instance.gameObject, "health_manager");
                            }
                            Component component2 = transform3.gameObject.GetComponent<tk2dSprite>();
                            if (playMakerFSM2)
                            {
                                if (component2 == null)
                                {
                                    component2 = null;
                                }
                                int value2 = playMakerFSM2.FsmVariables.GetFsmInt("HP").Value;
                                enemyPool.Add(transform3.gameObject, new EnemyData(value2, playMakerFSM2, component2));
                            }
                        }
                        list.Add(transform3);
                    }
                }
            }
        }

        public static void UpdateGUI(Matrix4x4 matrix, Transform baseTrans)
        {
            if (enemyPanel && !HeroController.instance.cState.transitioning && DebugMod.gm.IsGameplayScene())
            {
                GUI.skin.button.alignment = TextAnchor.MiddleRight;
                GUI.skin.label.fontStyle = FontStyle.Bold;
                GUI.skin.button.fontSize = 18;
                GUI.contentColor = Color.white;
                GUI.skin.label.alignment = TextAnchor.MiddleRight;
                GUI.skin.label.fontSize = 19;
                GUI.Label(new Rect(1560f, 150f, 350f, 30f), "[ENEMY LIST]");
                GUI.skin.label.fontSize = 18;
                int num3 = 0;
                if (enemyPool.Count > 0)
                {
                    int num4 = 0;
                    int num5 = 0;
                    List<GameObject> tempEnemyData = new List<GameObject>();
                    tempEnemyData.AddRange(enemyPool.Keys);
                    for (int j = 0; j < tempEnemyData.Count; j++)
                    {
                        GameObject gameObject = tempEnemyData[j];
                        if (gameObject != null)
                        {
                            Vector3 position2 = gameObject.transform.position;
                            string text2 = gameObject.name;
                            if (text2.Length > 28)
                            {
                                text2 = text2.Substring(0, 28) + "...";
                            }
                            int value7 = enemyPool[gameObject].FSM.FsmVariables.GetFsmInt("HP").Value;
                            if (UIManager.instance.uiState == UIState.PLAYING)
                            {
                                if (gameObject.activeSelf)
                                {
                                    num3++;
                                    if (attachHP)
                                    {
                                        GUI.matrix = matrix;
                                        GUI.skin.label.fontStyle = FontStyle.Bold;
                                        GUI.skin.label.fontSize = 22;
                                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                                        if (enemyPool[gameObject].Spr != null)
                                        {
                                            int hp = enemyPool[gameObject].HP;
                                            Vector2 vector = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                                            Bounds bounds = (enemyPool[gameObject].Spr as tk2dSprite).GetBounds();
                                            Vector3 vector2 = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
                                            float num6 = (Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)).y - vector2.y) / 2f;
                                            float num7 = 100f * GUIController.instance.scale.x;
                                            float num8 = 20f * GUIController.instance.scale.y;
                                            if (hp >= 1000)
                                            {
                                                num7 = 120f * GUIController.instance.scale.x;
                                            }
                                            float num9 = num7;
                                            num9 *= (float)value7 / (float)hp;
                                            if (value7 > hp)
                                            {
                                                num9 = num7;
                                            }
                                            float num10 = num7 / 2f;
                                            GUIController.instance.DrawRect(new Rect(vector.x - num10 - 1f, (float)Screen.height - vector.y - 1f - num6, num7 + 1f, num8 + 1f), Color.black);
                                            if (hp != 0 && value7 > 0)
                                            {
                                                GUIController.instance.DrawRect(new Rect(vector.x - num10, (float)Screen.height - vector.y - num6, num9, num8), Color.red);
                                            }
                                            GUI.skin.label.fontSize = (int)Math.Round((double)(22f * GUIController.instance.scale.y), 0);
                                            GUI.Label(new Rect(vector.x - num10, (float)Screen.height - vector.y - num6 - 2f, num7, num8 + 2f), value7.ToString() + "/" + hp.ToString());
                                        }
                                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                                        GUI.skin.label.fontSize = 18;
                                        GUI.skin.label.alignment = TextAnchor.MiddleRight;
                                        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, GUIController.instance.scale);
                                    }
                                    else if (position2.y > 0f && position2.y < DebugMod.gm.sceneHeight && position2.x > 0f && position2.x < DebugMod.gm.sceneWidth && enemyPool[gameObject].HP >= hpLimit)
                                    {
                                        Vector2 scrollPosition = Vector2.zero;
                                        scrollPosition = GUI.BeginScrollView(new Rect(1525f, 180f, 395f, 540f), scrollPosition, new Rect(0f, 0f, 360f, (float)(enemyPool.Count * 30)), GUIStyle.none, GUIStyle.none);
                                        Rect position4 = new Rect(0f, (float)(num4 * 30), 390f, 30f);
                                        string[] array2 = new string[5];
                                        array2[0] = text2;
                                        array2[1] = "   ";
                                        array2[2] = value7.ToString();
                                        array2[3] = "/";
                                        int num11 = 4;
                                        array2[num11] = enemyPool[gameObject].HP.ToString();
                                        GUI.Label(position4, string.Concat(array2));
                                        GUI.EndScrollView();
                                        num4++;
                                    }
                                    if (showBox && (enemyPool[gameObject].Spr as tk2dSprite).boxCollider2D != null)
                                    {
                                        GUI.matrix = matrix;
                                        BoxCollider2D boxCollider2D = (enemyPool[gameObject].Spr as tk2dSprite).boxCollider2D;
                                        Bounds bounds2 = boxCollider2D.bounds;
                                        Vector3 position3 = baseTrans.TransformPoint(boxCollider2D.bounds.center);
                                        Vector2 vector3 = Camera.main.WorldToScreenPoint(position3);
                                        Vector3 vector4 = Camera.main.WorldToScreenPoint(new Vector3(bounds2.min.x, bounds2.min.y, bounds2.min.z));
                                        Vector3 vector5 = Camera.main.WorldToScreenPoint(new Vector3(bounds2.max.x, bounds2.max.y, bounds2.max.z));
                                        float num12 = vector5.x - vector4.x;
                                        float num13 = vector5.y - vector4.y;
                                        float x = vector3.x - num12 * 0.5f;
                                        float y = (float)Screen.height - vector3.y - num13 * 0.5f;
                                        Color yellow = Color.yellow;
                                        yellow.a = 0.7f;
                                        GUIController.instance.DrawRect(new Rect(x, y, num12, num13), yellow);
                                        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, GUIController.instance.scale);
                                    }
                                }
                            }
                            else if (UIManager.instance.uiState == UIState.PAUSED)
                            {
                                GUI.contentColor = Color.white;
                                if (!gameObject.activeSelf)
                                {
                                    GUI.contentColor = Color.gray;
                                }
                                enemyScroll = GUI.BeginScrollView(new Rect(1430f, 180f, 490f, 540f), enemyScroll, new Rect(0f, 0f, 470f, (float)(enemyPool.Count * 30)));
                                Rect position5 = new Rect(90f, (float)(num5 * 30), 390f, 30f);
                                string[] array3 = new string[5];
                                array3[0] = text2;
                                array3[1] = "   ";
                                array3[2] = value7.ToString();
                                array3[3] = "/";
                                int num14 = 4;
                                array3[num14] = enemyPool[gameObject].HP.ToString();
                                if (GUI.Button(position5, string.Concat(array3)))
                                {
                                    Console.AddLine("Color: " + (enemyPool[gameObject].Spr as tk2dSprite).color.a.ToString());
                                    gameObject.SetActive(!gameObject.activeSelf);
                                    Console.AddLine("Changed the state of enemy gO to " + gameObject.activeSelf.ToString().ToUpper());
                                }
                                GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                                if (GUI.Button(new Rect(0f, (float)(num5 * 30), 30f, 30f), "C"))
                                {
                                    GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, gameObject.transform.position, baseTrans.rotation) as GameObject;
                                    Component component = gameObject2.GetComponent<tk2dSprite>();
                                    PlayMakerFSM playMakerFSM2 = FSMUtility.LocateFSM(gameObject2, enemyPool[gameObject].FSM.FsmName);
                                    int value8 = playMakerFSM2.FsmVariables.GetFsmInt("HP").Value;
                                    enemyPool.Add(gameObject2, new EnemyData(value8, playMakerFSM2, component));
                                    Console.AddLine("Cloning enemy as: " + gameObject2.name);
                                }
                                if (GUI.Button(new Rect(30f, (float)(num5 * 30), 30f, 30f), "D"))
                                {
                                    Console.AddLine("Destroying enemy: " + gameObject.name);
                                    enemyPool.Remove(gameObject);
                                    UnityEngine.Object.DestroyImmediate(gameObject);
                                }
                                if (GUI.Button(new Rect(60f, (float)(num5 * 30), 30f, 30f), "∞"))
                                {
                                    enemyPool[gameObject].SetHP(9999);
                                    Console.AddLine("HP for enemy: " + gameObject.name + " is now 9999");
                                }
                                GUI.skin.button.alignment = TextAnchor.MiddleRight;
                                GUI.EndScrollView();
                                GUI.contentColor = Color.white;
                            }
                            num5++;
                        }
                        else
                        {
                            enemyPool.Remove(gameObject);
                        }
                    }
                }
                if (enemyPool.Count > 0 && UIManager.instance.uiState != UIState.PAUSED)
                {
                    GUI.skin.label.fontStyle = FontStyle.Italic;
                    GUI.skin.label.fontSize = 18;
                    GUI.skin.label.alignment = TextAnchor.MiddleRight;
                    int num15 = 0;
                    if (hpLimit != 0)
                    {
                        num15 = enemyPool.Aggregate(delegate(KeyValuePair<GameObject, EnemyData> l, KeyValuePair<GameObject, EnemyData> r)
                        {
                            if (l.Value.HP <= r.Value.HP)
                            {
                                return r;
                            }
                            return l;
                        }).Value.HP;
                    }
                    if (attachHP && num3 != 0)
                    {
                        GUI.Label(new Rect(1560f, 180f, 350f, 30f), "<HP bars enabled>");
                    }
                    if (num3 == 0)
                    {
                        GUI.Label(new Rect(1560f, 180f, 350f, 30f), "<No active enemies>");
                    }
                    if (!attachHP && num3 != 0 && hpLimit > num15)
                    {
                        GUI.Label(new Rect(1560f, 180f, 350f, 30f), "<No matching enemies>");
                    }
                    GUI.skin.label.fontSize = 18;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                }
                float num16 = 0f;
                if (enemyPool.Count < 1)
                {
                    GUI.skin.label.fontStyle = FontStyle.Italic;
                    GUI.skin.label.fontSize = 18;
                    GUI.Label(new Rect(1560f, 180f, 350f, 30f), "<empty>");
                    GUI.skin.label.fontSize = 18;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    num16 = 30f;
                }
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                if (UIManager.instance.uiState == UIState.PAUSED)
                {
                    GUILayout.BeginArea(new Rect(1685f, 122f, 230f, 30f));
                    GUILayout.BeginHorizontal("", new GUILayoutOption[0]);
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    GUILayout.Label("Narrow HP:", new GUILayoutOption[]
                    {
                    GUILayout.MaxWidth(115f)
                    });
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUILayout.Label(hpLimit.ToString(), new GUILayoutOption[]
                    {
                    GUILayout.MaxWidth(30f),
                    GUILayout.MaxWidth(50f),
                    GUILayout.ExpandWidth(true)
                    });
                    GUI.skin.label.alignment = TextAnchor.MiddleRight;
                    GUI.skin.button.alignment = TextAnchor.MiddleCenter;
                    if (GUILayout.Button("<color=green>+</color>", new GUILayoutOption[]
                    {
                    GUILayout.MinWidth(30f)
                    }))
                    {
                        hpLimit += 10;
                    }
                    if (GUILayout.Button("<color=red>-</color>", new GUILayoutOption[]
                    {
                    GUILayout.MinWidth(30f)
                    }) && hpLimit - 10 >= 0)
                    {
                        hpLimit -= 10;
                    }
                    GUI.skin.button.alignment = TextAnchor.MiddleRight;
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();
                    num16 = 180f + (float)enemyPool.Count * 30f + num16;
                    if (enemyPool.Count > 18)
                    {
                        num16 = 723f;
                    }
                    GUI.BeginGroup(new Rect(1484f, num16, 426f, 80f));
                    TextAnchor alignment12 = GUI.skin.button.alignment;
                    GUI.skin.button.alignment = TextAnchor.MiddleRight;
                    if (GUI.Button(new Rect(0f, 0f, 120f, 30f), "HITBOX?"))
                    {
                        showBox = !showBox;
                    }
                    if (GUI.Button(new Rect(122f, 0f, 120f, 30f), "HP BARS?"))
                    {
                        attachHP = !attachHP;
                    }
                    if (GUI.Button(new Rect(245f, 0f, 90f, 30f), new GUIContent("AUTO?", "Possible performance hit!")))
                    {
                        if (!autoUpdate)
                        {
                            autoUpdate = true;
                            Console.AddLine("Enemy collision autoscan is ENABLED. WARNING: may affect performance!");
                        }
                        else
                        {
                            autoUpdate = false;
                            Console.AddLine("Enemy collision autoscan is DISABLED");
                        }
                    }
                    TextAnchor alignment13 = GUI.skin.label.alignment;
                    FontStyle fontStyle6 = GUI.skin.label.fontStyle;
                    GUI.skin.label.alignment = TextAnchor.UpperLeft;
                    GUI.skin.label.fontStyle = FontStyle.Italic;
                    GUI.Label(new Rect(0f, 40f, 300f, 30f), GUI.tooltip);
                    GUI.skin.label.alignment = alignment13;
                    GUI.skin.label.fontStyle = fontStyle6;
                    if (GUI.Button(new Rect(336f, 0f, 90f, 30f), "SCAN"))
                    {
                        enemyUpdate(200f);
                        Console.AddLine("Refreshing collider data...");
                    }
                    GUI.skin.button.alignment = alignment12;
                    if (showBox)
                    {
                        TextAnchor alignment14 = GUI.skin.label.alignment;
                        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                        GUI.Label(new Rect(2f, 0f, 30f, 30f), "<color=lime>✔</color>");
                        GUI.skin.label.alignment = alignment14;
                    }
                    if (attachHP)
                    {
                        TextAnchor alignment15 = GUI.skin.label.alignment;
                        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                        GUI.Label(new Rect(124f, 0f, 30f, 30f), "<color=lime>✔</color>");
                        GUI.skin.label.alignment = alignment15;
                    }
                    if (autoUpdate)
                    {
                        TextAnchor alignment16 = GUI.skin.label.alignment;
                        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                        GUI.Label(new Rect(247f, 0f, 30f, 30f), "<color=lime>✔</color>");
                        GUI.skin.label.alignment = alignment16;
                    }
                    GUI.EndGroup();
                }
            }
        }
    }

    public struct EnemyData
    {
        public int HP;
        public PlayMakerFSM FSM;
        public Component Spr;

        public EnemyData(int hp, PlayMakerFSM fsm, Component spr)
        {
            HP = hp;
            FSM = fsm;
            Spr = spr;
        }

        public void SetHP(int health)
        {
            HP = health;
            FSM.FsmVariables.GetFsmInt("HP").Value = 9999;
        }
    }
}
