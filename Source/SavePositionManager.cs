using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using static DebugMod.EnemiesPanel;
using System;

namespace DebugMod
{
    public class SavePositionManager
    {
        public static List<EnemyData> FSMs = new List<EnemyData>();
        public static List<EnemyData> CreatedFSMs = new List<EnemyData>();
        //public static List<Vector3>;
        //public static List<Vector3,>
        // too complicated public static PlayMakerFSM KnightFsm = DebugMod.RefKnight.LocateMyFSM("Knight-ProxyFSM") ;
        public static Vector3 KnightPos;
        public static Vector3 CamPos;
        public static Vector2 KnightVel;
        public static string PositionScene;
        public static bool InitializedPosition = false;
        // public static List
        public static void SaveState()
        {

            //float dash_timer = ReflectionHelper.GetField<HeroController, float>(HeroController.instance, "dash_timer")
            KnightPos = DebugMod.RefKnight.gameObject.transform.position;
            KnightVel = HeroController.instance.current_velocity;
            CamPos = DebugMod.RefCamera.gameObject.transform.position;
            PositionScene = DebugMod.GetSceneName();
            InitializedPosition = true;
            FSMs = GetAllEnemies(new List<EnemyData>());
            //TODO: check this . used to be FSMs = GetAllEnemies(CreatedFSMs);
            FSMs.ForEach(delegate (EnemyData dat) {     dat.gameObject.SetActive(false);    });
            Console.AddLine("Positional save set in " + DebugMod.GetSceneName());
        }
        public static void LoadState()
        {
            if (PositionScene == DebugMod.GetSceneName() && InitializedPosition) { 
                try
                {
                    RemoveAllCopies();
                    CreatedFSMs = Create();
                    // Move knight to saved location, change velocity to saved velocity, Move Camera to saved camera position, 
                    DebugMod.RefKnight.gameObject.transform.position = KnightPos;
                    HeroController.instance.current_velocity = KnightVel;
                    DebugMod.RefCamera.gameObject.transform.position = CamPos;
                }
                catch (Exception e)
                {
                    Console.AddLine("No positional save in " + DebugMod.GetSceneName()+", or a bug has occured.");
                }
            } else {
                Console.AddLine("No positional save in " + DebugMod.GetSceneName());
            }
        }

        public static List<EnemyData> Create() 
        {
            List<EnemyData> data = new List<EnemyData>();
            for (int i = 0; i < FSMs.Count; i++)
            {
                EnemyData dattemp = FSMs.FindAll(ed => ed.gameObject != null)[i];
                GameObject gameObject = UnityEngine.Object.Instantiate(dattemp.FSM.gameObject, dattemp.gameObject.transform.position, dattemp.gameObject.transform.rotation) as GameObject;
                Component component = gameObject.GetComponent<tk2dSprite>();
                PlayMakerFSM playMakerFSM2 = FSMUtility.LocateFSM(gameObject, dattemp.FSM.FsmName);
                int health = playMakerFSM2.FsmVariables.GetFsmInt("HP").Value;
                gameObject.SetActive(true);
                data.Add(new EnemyData(health, playMakerFSM2, component, parent, gameObject));
            };
            return data;
        }   
        public static void RemoveAllCopies()
        {
            //get all copies and remove them.
            CreatedFSMs.ForEach(delegate (EnemyData dat)
            {
               if (!FSMs.Any(ed => ed.gameObject == dat.gameObject))
                    GameObject.Destroy(dat.gameObject.gameObject.gameObject.gameObject);
            });
        }
        // this works but idk how ??
        public static List<EnemyData> GetAllEnemies(List<EnemyData> Exclude)
        {
            float boxSize = 250f;
            List<EnemyData> ret = new List<EnemyData>();
            if (HeroController.instance != null && !HeroController.instance.cState.transitioning && DebugMod.GM.IsGameplayScene())
            {
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
                        if (playMakerFSM && array[i].gameObject.activeSelf && !(Exclude.Any(ed => ed.gameObject == array[i].gameObject)) && !Ignore(array[i].gameObject.name))
                        {
                            Component component = array[i].gameObject.GetComponent<tk2dSprite>();
                            if (component == null)
                            {
                                component = null; //?
                            }
                            int Health = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                            ret.Add(new EnemyData(Health, playMakerFSM, component, parent, array[i].gameObject));
                        }
                    }
                }  
            }
            return ret;
        }
    }
}
