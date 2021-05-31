using System;
using System.Collections.Generic;
using System.Linq;
using GlobalEnums;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

using Object = UnityEngine.Object;

namespace DebugMod
{
    //public class ShowHitboxes : Mod, ITogglableMod
    public class ShowHitboxes
    {
        public static int State;

        private static Material greenMat;
        private static Material redMat;
        private static Material yellowMat;
        private static Material blueMat;

        private Dictionary<Collider2D, LineRenderer> lines = new Dictionary<Collider2D, LineRenderer>();
        private List<Collider2D> colliders = new List<Collider2D>();

        public ShowHitboxes()
        {
            State = 0;

            greenMat = new Material(Shader.Find("Diffuse"));
            greenMat.renderQueue = 4000;
            greenMat.color = Color.green;

            redMat = new Material(Shader.Find("Diffuse"));
            redMat.renderQueue = 4000;
            redMat.color = Color.red;

            yellowMat = new Material(Shader.Find("Diffuse"));
            yellowMat.renderQueue = 4000;
            yellowMat.color = Color.yellow;

            blueMat = new Material(Shader.Find("Diffuse"));
            blueMat.renderQueue = 4000;
            blueMat.color = Color.blue;
        }

        public void Load()
        {
            Unload();
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SpawnHitboxes;
            ModHooks.Instance.HeroUpdateHook += UpdateHitboxes;
            State = DebugMod.settings.ShowHitBoxes;
            
            SpawnHitboxes();
        }


        public void Unload()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SpawnHitboxes;
            ModHooks.Instance.HeroUpdateHook -= UpdateHitboxes;
            State = DebugMod.settings.ShowHitBoxes;

            foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
            {
                if (obj.name == "Mod Hitbox")
                {
                    Object.Destroy(obj);
                }
            }
        }

        private void DestroyBorder(IEnumerable<GameObject> borders)
        {
            foreach (GameObject border in borders)
            {
                Object.Destroy(border);
            }
        }

        private void SpawnHitboxes()
        {
            if (!GameManager.instance.IsGameplayScene())
            {
                return;
            }

            DestroyBorder(Object.FindObjectsOfType<GameObject>().Where(x => x.name.Contains("sceneborder")));

            foreach (Collider2D col in lines.Keys.ToArray())
            {
                if (col != null && lines[col] != null)
                {
                    Object.Destroy(lines[col].gameObject);
                }
            }

            switch (State)
            {
                case 1: 
                    State1Boxes();
                    break;
                case 2: 
                    State2Boxes();
                    break;
            }
        }

        private void State1Boxes()
        {
            colliders = new List<Collider2D>();
            lines = new Dictionary<Collider2D, LineRenderer>();

            foreach (Collider2D col in Object.FindObjectsOfType<Collider2D>())
            {
                if (colliders.Contains(col))
                {
                    continue;
                }
                
                if (col.gameObject.layer == (int)PhysLayers.TERRAIN)
                {
                    lines.Add(col, SetupLineRenderer(col, null, greenMat));
                }
                else if (col.GetComponent<TransitionPoint>())
                {
                    lines.Add(col, SetupLineRenderer(col, null, blueMat));
                }
                else if (col.GetComponent<DamageHero>() || col.gameObject.LocateMyFSM("damages_hero") != null)
                {
                    colliders.Add(col);
                    lines.Add(col, SetupLineRenderer(col, null, redMat));
                }
                else if (col.gameObject == HeroController.instance.gameObject && !col.isTrigger)
                {
                    colliders.Add(col);
                    lines.Add(col, SetupLineRenderer(col, null, yellowMat));
                }
                else if (col.GetComponent<Breakable>())
                {
                    NonBouncer bounce = col.GetComponent<NonBouncer>();
                    if (bounce == null || !bounce.active)
                    {
                        colliders.Add(col);
                        lines.Add(col, SetupLineRenderer(col, null, blueMat));
                    }
                }
            }
        }  
        
        
        private void State2Boxes()
        {
            colliders = new List<Collider2D>();
            lines = new Dictionary<Collider2D, LineRenderer>();

            foreach (Collider2D col in Object.FindObjectsOfType<Collider2D>())
            {
                if (colliders.Contains(col))
                {
                    continue;
                }

                if (col.gameObject.layer == (int)PhysLayers.TERRAIN)
                {
                    lines.Add(col, SetupLineRenderer(col, null, greenMat));
                }
                else if (col.GetComponent<TransitionPoint>())
                {
                    lines.Add(col, SetupLineRenderer(col, null, blueMat));
                }
                else if (col.GetComponent<DamageHero>() || col.gameObject.LocateMyFSM("damages_hero") != null)
                {
                    colliders.Add(col);
                    lines.Add(col, SetupLineRenderer(col, null, redMat));
                }
                else if (col.GetComponent<Breakable>())
                {
                    NonBouncer bounce = col.GetComponent<NonBouncer>();
                    if (bounce == null || !bounce.active)
                    {
                        colliders.Add(col);
                        lines.Add(col, SetupLineRenderer(col, null, blueMat));
                    }
                }
                else if (col.isTrigger && col.gameObject.GetComponent<HazardRespawnTrigger>() != null)
                {
                    colliders.Add(col);
                    lines.Add(col, this.SetupLineRenderer(col, null, blueMat));
                }
                else if ((col.isTrigger && col.gameObject.GetComponent<CircleCollider2D>() == null) 
                         || (col.gameObject == HeroController.instance.gameObject && !col.isTrigger))
                {
                    colliders.Add(col);
                    lines.Add(col, this.SetupLineRenderer(col, null, yellowMat));
                }
            }
        }

        private void SpawnHitboxes(Scene from, Scene to) => SpawnHitboxes();

        private void UpdateHitboxes()
        {
            if (colliders == null || lines == null)
            {
                return;
            }

            foreach (Collider2D col in colliders)
            {
                if (col == null || !col.enabled)
                {
                    if (lines[col] != null)
                    {
                        Object.Destroy(lines[col].gameObject);
                    }

                    continue;
                }

                lines[col] = SetupLineRenderer(col, lines[col]);
            }
        }

        private LineRenderer SetupLineRenderer(Collider2D col, LineRenderer line = null, Material mat = null)
        {
            if (line == null)
            {
                if (mat == null)
                {
                    mat = greenMat;
                }

                GameObject obj = new GameObject("Mod Hitbox");
                obj.transform.SetParent(col.transform);
                obj.transform.position = Vector3.zero;

                line = obj.AddComponent<LineRenderer>();
                line.SetWidth(.05f, .05f);
                line.sharedMaterial = mat;
            }

            if (col is BoxCollider2D box)
            {
                Vector2 topRight = box.size / 2f;
                Vector2 bottomLeft = -topRight;
                Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);
                Vector2 topLeft = -bottomRight;

                line.SetVertexCount(5);
                line.SetPositions(new Vector3[]
                {
                    col.transform.TransformPoint(bottomLeft + box.offset),
                    col.transform.TransformPoint(topLeft + box.offset),
                    col.transform.TransformPoint(topRight + box.offset),
                    col.transform.TransformPoint(bottomRight + box.offset),
                    col.transform.TransformPoint(bottomLeft + box.offset)
                });
            }
            else if (col is CircleCollider2D circle)
            {
                Vector3 center = circle.transform.position + (Vector3)circle.offset;

                Vector3[] points = new Vector3[30];
                float sliceSize = Mathf.PI * 2f / points.Length;

                for (int i = 0; i < points.Length - 1; i++)
                {
                    float theta = sliceSize * i;
                    float sin = (float)Math.Sin(theta);
                    float cos = (float)Math.Cos(theta);

                    points[i] = new Vector2(
                        (cos - sin) * circle.transform.localScale.x * circle.radius,
                        (cos + sin) * circle.transform.localScale.y * circle.radius);
                }

                points[points.Length - 1] = points[0];

                line.SetVertexCount(points.Length);
                line.SetPositions(points);
            }
            else if (col is PolygonCollider2D poly)
            {
                Vector3[] points = new Vector3[poly.points.Length + 1];
                for (int i = 0; i < poly.points.Length; i++)
                {
                    points[i] = poly.transform.TransformPoint(poly.points[i]);
                }

                points[points.Length - 1] = points[0];
                line.SetVertexCount(points.Length);
                line.SetPositions(points);
            }
            else if (col is EdgeCollider2D edge)
            {
                Vector3[] points = new Vector3[edge.points.Length];
                for (int i = 0; i < edge.points.Length; i++)
                {
                    points[i] = edge.transform.TransformPoint(edge.points[i]);
                }

                line.SetVertexCount(points.Length);
                line.SetPositions(points);
            }

            return line;
        }

        private bool IsChildOf(GameObject child, GameObject parent)
        {
            if (child == null || parent == null)
            {
                return false;
            }

            Transform t = child.transform;

            while (t != null)
            {
                if (t.gameObject == parent)
                {
                    return true;
                }

                t = t.parent;
            }

            return false;
        }
    }
}
