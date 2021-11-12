using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static DebugMod.EnemiesPanel;
namespace DebugMod
{
    public class SavePositionManager
    {
        public static List<EnemyData> FSMs = new List<EnemyData>();
        public static List<EnemyData> FSMs2 = new List<EnemyData>();
        static Vector3 vect = new(0,0,0);
        public static void SavePosition()
        {
            vect = DebugMod.RefKnight.transform.position;
            // save fsms :eyes:

            FSMs = GetAllEnemies(FSMs);
            FSMs.ForEach(delegate (EnemyData dat)
            {
                dat.gameObject.SetActive(false);
            });

        }
        public static void LoadPosition()
        {

            //debuging();
            //create and name fsms2
            RemoveAll();
            FSMs2 = Create();
            
            DebugMod.RefKnight.transform.position = vect;
            DebugMod.RefCamera.SnapTo(DebugMod.RefKnight.transform.position.x, DebugMod.RefKnight.transform.position.y);

        }

        public static List<EnemyData> Create() 
        {
            List<EnemyData> data = new List<EnemyData>();
            //EnemyData dat = enemyPool.FindAll(ed => ed.gameObject != null && ed.gameObject.activeSelf)[num - 1];
            FSMs.ForEach(delegate (EnemyData dat)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate(dat.gameObject, dat.gameObject.transform.position, dat.gameObject.transform.rotation) as GameObject;
                Component component = gameObject2.GetComponent<tk2dSprite>();
                PlayMakerFSM playMakerFSM2 = FSMUtility.LocateFSM(gameObject2, dat.FSM.FsmName);
                int value8 = playMakerFSM2.FsmVariables.GetFsmInt("HP").Value;
                gameObject2.SetActive(true);
                data.Add(new EnemyData(value8, playMakerFSM2, component, parent, gameObject2));
                Console.AddLine(dat.gameObject.name+"Cloning enemy as: " + gameObject2.name);
            });
           return data;
        }
        public static void RemoveAll()
        {
            //get all created
            FSMs2.ForEach(delegate (EnemyData dat)
            {
               // if (!FSMs.Any(ed => ed.gameObject == dat.gameObject))
                //{
                    UnityEngine.Object.Destroy(dat.gameObject);
                //}

            });
            
        }
        public static List<EnemyData> GetAllEnemies(List<EnemyData> Include)
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
                        if (playMakerFSM && array[i].gameObject.activeSelf &&   !(Include.Any(ed => ed.gameObject == array[i].gameObject)) && !Ignore(array[i].gameObject.name))
                        {
                            Component component = array[i].gameObject.GetComponent<tk2dSprite>();
                            if (component == null)
                            {
                                component = null;
                            }
                            int value = playMakerFSM.FsmVariables.GetFsmInt("HP").Value;
                            ret.Add(new EnemyData(value, playMakerFSM, component, parent, array[i].gameObject));
                        }
                    }
                }  
            }
            return ret;
        }
    }
}
