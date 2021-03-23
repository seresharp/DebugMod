using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Vasi;

namespace DebugMod
{
    public static class BossHandler
    {
        public static Dictionary<string, KeyValuePair<bool, string>> bossData;
        public static Dictionary<string, string> ghostData;
        public static bool bossFound;
        public static bool ghostFound;
        
        private static bool fsmToggle = false;
        private static NonBouncer _coro;

        public static void LookForBoss(string sceneName)
        {
            bossFound = false;
            ghostFound = false;
            if (bossData != null && bossData.ContainsKey(sceneName))
            {
                Console.AddLine("Found stored Boss in this scene, respawn available");
                bossFound = true;
            }
            if (ghostData != null && ghostData.ContainsKey(sceneName))
            {
                Console.AddLine("Found stored Ghost Boss in this scene, respawn available");
                ghostFound = true;
            }
        }

        public static void PopulateBossLists()
        {
            if (bossData == null)
            {
                bossData = new Dictionary<string, KeyValuePair<bool, string>>(16);
            }
            if (ghostData == null)
            {
                ghostData = new Dictionary<string, string>(7);
            }
            bossData.Clear();
            ghostData.Clear();
            bossData.Add("Ruins2_03", new KeyValuePair<bool, string>(true, "Battle Control"));
            bossData.Add("Crossroads_09", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Crossroads_04", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Fungus1_04", new KeyValuePair<bool, string>(false, "hornet1Defeated"));
            bossData.Add("Crossroads_10", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Fungus3_archive_02", new KeyValuePair<bool, string>(false, "defeatedMegaJelly"));
            bossData.Add("Fungus2_15", new KeyValuePair<bool, string>(false, "defeatedMantisLords"));
            bossData.Add("Waterways_12", new KeyValuePair<bool, string>(false, "flukeMotherDefeated"));
            bossData.Add("Waterways_05", new KeyValuePair<bool, string>(false, "defeatedDungDefender"));
            bossData.Add("Ruins1_24", new KeyValuePair<bool, string>(false, "mageLordDefeated"));
            bossData.Add("Deepnest_32", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Mines_18", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Mines_32", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Fungus3_23", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Ruins2_11", new KeyValuePair<bool, string>(true, "Battle Scene"));
            bossData.Add("Deepnest_East_Hornet", new KeyValuePair<bool, string>(false, "hornetOutskirtsDefeated"));
            bossData.Add("Grimm_Main_Tent", new KeyValuePair<bool, string>(false, "killedGrimm"));
            ghostData.Add("RestingGrounds_02", "xeroDefeated");
            ghostData.Add("Fungus1_35", "noEyesDefeated");
            ghostData.Add("Fungus2_32", "elderHuDefeated");
            ghostData.Add("Deepnest_East_10", "markothDefeated");
            ghostData.Add("Deepnest_40", "galienDefeated");
            ghostData.Add("Fungus3_40", "mumCaterpillarDefeated");
            ghostData.Add("Cliffs_02", "aladarSlugDefeated");
        }

        public static void RespawnBoss()
        {
            if (bossFound)
            {
                if (bossData[DebugMod.GetSceneName()].Key)
                {
                    PlayMakerFSM[] components = GameObject.Find(bossData[DebugMod.GetSceneName()].Value).GetComponents<PlayMakerFSM>();
                    if (components != null)
                    {
                        foreach (PlayMakerFSM playMakerFSM in components)
                        {
                            if (playMakerFSM.FsmVariables.GetFsmBool("Activated") != null)
                            {
                                playMakerFSM.FsmVariables.GetFsmBool("Activated").Value = false;
                                Console.AddLine("Boss control for this scene was reset, re-enter scene or warp");
                            }
                        }
                    }
                    else
                    {
                        Console.AddLine("GO does not exist or no FSM on it");
                    }
                }
                else
                {
                    if (bossData[DebugMod.GetSceneName()].Value == "killedGrimm")
                    {
                        PlayerData.instance.SetIntInternal("grimmChildLevel", 2);
                        PlayerData.instance.SetIntInternal("flamesCollected", 3);
                        PlayerData.instance.SetBoolInternal("grimmChildAwoken", false);
                        PlayerData.instance.SetBoolInternal("foughtGrimm", false);
                        PlayerData.instance.SetBoolInternal("killedGrimm", false);
                    }
                    else
                    {
                        PlayerData.instance.GetType().GetField(bossData[DebugMod.GetSceneName()].Value).SetValue(PlayerData.instance, false);
                    }
                    Console.AddLine("Boss control for this scene was reset, re-enter scene or warp");
                }
            }
            else
            {
                Console.AddLine("No boss in this scene to respawn");
            }
        }

        public static void RespawnGhost()
        {
            if (ghostFound)
            {
                PlayerData.instance.GetType().GetField(ghostData[DebugMod.GetSceneName()]).SetValue(PlayerData.instance, 0);
                Console.AddLine("Ghost Boss for this scene was reset, re-enter scene or warp");
            }
            else
            {
                Console.AddLine("No ghost in this scene to respawn");
            }
        }

        public static void UumuuExtra()
        {
            if (!fsmToggle)
            {
                var go = new GameObject();
                _coro = go.AddComponent<NonBouncer>();
                //GameManager.instance.
                Object.DontDestroyOnLoad(_coro);
                
                Console.AddLine(_coro.ToString());
                
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += StartUumuuCoro;
                fsmToggle = true;
                
            }
            else
            {
                Object.Destroy(_coro);
                UnityEngine.SceneManagement.SceneManager.sceneLoaded -= StartUumuuCoro;
                fsmToggle = false;
                Console.AddLine("Uumuu forced extra attack OFF");
            }

            /*
            if (DebugMod.GetSceneName() == "Fungus3_archive_02") 
            {
                PlayMakerFSM[] components = GameObject.Find("Mega Jellyfish").GetComponents<PlayMakerFSM>();

                if (components != null)
                {
                    foreach (PlayMakerFSM playMakerFSM in components)
                    {
                        //if (playMakerFSM.)
                        {
                            playMakerFSM.FsmVariables.GetFsmBool("Activated").Value = false;
                            Console.AddLine("Boss control for this scene was reset, re-enter scene or warp");
                        }
                    }
                }
                else
                {
                    Console.AddLine("GO does not exist or no FSM on it");
                }
            }
            */
        }

        private static void StartUumuuCoro(Scene scene, LoadSceneMode lsm)
        {
            //Console.AddLine("Uumuu test: " + scene.name);
            if (scene.name == "Fungus3_archive_02")
            {
                Console.AddLine(_coro.ToString());
                _coro.StartCoroutine(UumuuExtraCoro());
            }
        }

        private static IEnumerator UumuuExtraCoro()
        {
            yield return null;
            Console.AddLine("Uumuu coro check");
            // Find Uumuu and the FSM
            GameObject uumuu = GameObject.Find("Mega Jellyfish");
            Console.AddLine("Uumuu coro mid pls work");
            if (uumuu == null)
                yield break;
            Console.AddLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            PlayMakerFSM fsm = uumuu.LocateMyFSM("Mega Jellyfish");
            
            fsm.GetState("Idle").GetAction<WaitRandom>().timeMax = 1.2f;;
            

            // Fix the waits and the number of attacks


            //fsm.GetState("Idle").GetAction<WaitRandom>().timeMax = 1.5f;
            //fsm.GetState("Set Timer").GetAction<RandomFloat>().max = 2f;
            //fsm.FsmVariables.GetFsmFloat("Quirrel Time").Value = 4f;

            // Fix the pattern to 2 quick, then 1 long if it still needs to attack
            //FsmState choice = fsm.GetState("Choice");
            //choice.RemoveAction<SendRandomEventV2>();
            //choice.AddMethod(() => SetUumuuPattern(fsm));

            // Reset the multizap counter to 0 so the pattern remains 2 quick 1 optional long
            //fsm.GetState("Recover").AddMethod(() => fsm.FsmVariables.GetFsmInt("Ct Multizap").Value = 0); 

            // Set the initial RecoilSpeed to 0 so that dream nailing her on the first cycle doesn't push her
            //uumuu.GetComponent<Recoil>().SetRecoilSpeed(0);

            // Set her HP to 1028 value
            //uumuu.GetComponent<HealthManager>().hp = 250;

        }

        private static void SetUumuuPattern(PlayMakerFSM fsm)
        {
            if (fsm.FsmVariables.GetFsmInt("Ct Multizap").Value < 2)
            {
                fsm.Fsm.Event(fsm.FsmEvents.First(e => e.Name == "MULTIZAP"));
                fsm.FsmVariables.GetFsmInt("Ct Multizap").Value++;
            }
            else
            {
                fsm.Fsm.Event(fsm.FsmEvents.First(e => e.Name == "CHASE"));
            }
        }
    }
}
