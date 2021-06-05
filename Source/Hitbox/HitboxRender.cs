using System;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

namespace DebugMod.Hitbox
{
    public class HitboxRender : MonoBehaviour
    {
        // ReSharper disable once StructCanBeMadeReadOnly
        private struct HitboxType: IComparable<HitboxType>
        {
            public static readonly HitboxType Knight = new(Color.yellow, 0);
            public static readonly HitboxType Enemy = new(new Color(0.8f, 0, 0), 1);
            public static readonly HitboxType Attack = new(Color.cyan, 2);
            public static readonly HitboxType Terrain = new(new Color(0, 0.8f, 0), 3);
            public static readonly HitboxType Trigger = new(new Color(0.5f, 0.5f, 1f), 4);
            public static readonly HitboxType Other = new(new Color(0.9f, 0.6f, 0.4f), 5);

            public readonly Color Color;
            public readonly int Depth;

            private HitboxType(Color color, int depth)
            {
                Color = color;
                Depth = depth;
            }

            public int CompareTo(HitboxType other)
            {
                return other.Depth.CompareTo(Depth);
            }
        }

        private readonly SortedDictionary<HitboxType, HashSet<Collider2D>> colliders = new()
        {
            {HitboxType.Knight, new HashSet<Collider2D>()},
            {HitboxType.Enemy, new HashSet<Collider2D>()},
            {HitboxType.Attack, new HashSet<Collider2D>()},
            {HitboxType.Terrain, new HashSet<Collider2D>()},
            {HitboxType.Trigger, new HashSet<Collider2D>()},
            {HitboxType.Other, new HashSet<Collider2D>()},
        };

        private float LineWidth => Math.Max(0.7f, Screen.width / 960f * GameCameras.instance.tk2dCam.ZoomFactor);

        private void Start()
        {
            foreach (Collider2D col in Resources.FindObjectsOfTypeAll<Collider2D>())
            {
                TryAddHitboxes(col);
            }
        }

        public void UpdateHitbox(GameObject go)
        {
            foreach (Collider2D col in go.GetComponentsInChildren<Collider2D>(true))
            {
                TryAddHitboxes(col);
            }
        }

        private Vector2 LocalToScreenPoint(Camera camera, Collider2D collider2D, Vector2 point)
        {
            Vector2 result = camera.WorldToScreenPoint(collider2D.transform.TransformPoint(point + collider2D.offset));
            return new Vector2((int) Math.Round(result.x), (int) Math.Round(Screen.height - result.y));
        }

        private void TryAddHitboxes(Collider2D collider2D)
        {
            if (collider2D == null)
            {
                return;
            }

            if (collider2D is BoxCollider2D or PolygonCollider2D or EdgeCollider2D or CircleCollider2D)
            {
                GameObject go = collider2D.gameObject;
                if (collider2D.GetComponent<DamageHero>() || collider2D.gameObject.LocateMyFSM("damages_hero"))
                {
                    colliders[HitboxType.Enemy].Add(collider2D);
                } else if (go.LocateMyFSM("health_manager_enemy") || go.LocateMyFSM("health_manager"))
                {
                    colliders[HitboxType.Other].Add(collider2D);
                } else if (go.layer == (int) PhysLayers.TERRAIN)
                {
                    colliders[HitboxType.Terrain].Add(collider2D);
                } else if (go == HeroController.instance?.gameObject && !collider2D.isTrigger)
                {
                    colliders[HitboxType.Knight].Add(collider2D);
                } else if (go.LocateMyFSM("damages_enemy") || go.name == "Damager" && go.LocateMyFSM("Damage"))
                {
                    colliders[HitboxType.Attack].Add(collider2D);
                } else if (collider2D.isTrigger && (collider2D.GetComponent<TransitionPoint>() || collider2D.GetComponent<HazardRespawnTrigger>()))
                {
                    colliders[HitboxType.Trigger].Add(collider2D);
                } else if (collider2D.GetComponent<Breakable>())
                {
                    NonBouncer bounce = collider2D.GetComponent<NonBouncer>();
                    if (bounce == null || !bounce.active)
                    {
                        colliders[HitboxType.Trigger].Add(collider2D);
                    }
                } else if (HitboxViewer.State == 2)
                {
                    colliders[HitboxType.Other].Add(collider2D);
                }
            }
        }

        private void OnGUI()
        {
            if (Event.current?.type != EventType.Repaint || GameManager.instance.isPaused || Camera.main == null)
            {
                return;
            }

            GUI.depth = int.MaxValue;
            Camera camera = Camera.main;
            float lineWidth = LineWidth;
            foreach (var pair in colliders)
            {
                foreach (Collider2D collider2D in pair.Value)
                {
                    DrawHitbox(camera, collider2D, pair.Key, lineWidth);
                }
            }
        }

        private void DrawHitbox(Camera camera, Collider2D collider2D, HitboxType hitboxType, float lineWidth)
        {
            if (collider2D == null || !collider2D.isActiveAndEnabled)
            {
                return;
            }

            int origDepth = GUI.depth;
            GUI.depth = hitboxType.Depth;
            if (collider2D is BoxCollider2D or EdgeCollider2D or PolygonCollider2D)
            {
                List<Vector2> points = null;
                switch (collider2D)
                {
                    case BoxCollider2D boxCollider2D:
                    {
                        Vector2 halfSize = boxCollider2D.size / 2f;
                        Vector2 topLeft = new(-halfSize.x, halfSize.y);
                        Vector2 topRight = halfSize;
                        Vector2 bottomRight = new(halfSize.x, -halfSize.y);
                        Vector2 bottomLeft = -halfSize;
                        points = new List<Vector2>
                        {
                            topLeft, topRight, bottomRight, bottomLeft, topLeft
                        };
                        break;
                    }
                    case EdgeCollider2D edgeCollider2D:
                        points = new List<Vector2>(edgeCollider2D.points);
                        break;
                    case PolygonCollider2D polygonCollider2D:
                    {
                        points = new List<Vector2>(polygonCollider2D.points);
                        if (points.Count > 0)
                        {
                            points.Add(points[0]);
                        }

                        break;
                    }
                }

                for (int i = 0; i < points.Count - 1; i++)
                {
                    Vector2 pointA = LocalToScreenPoint(camera, collider2D, points[i]);
                    Vector2 pointB = LocalToScreenPoint(camera, collider2D, points[i + 1]);
                    Drawing.DrawLine(pointA, pointB, hitboxType.Color, lineWidth, true);
                }
            } else if (collider2D is CircleCollider2D circleCollider2D)
            {
                Vector2 offset = circleCollider2D.offset;
                Vector2 center = LocalToScreenPoint(camera, collider2D, offset);
                Vector2 centerRight = LocalToScreenPoint(camera, collider2D, new Vector2(offset.x + circleCollider2D.radius, offset.y));
                int radius = (int) Math.Round(Math.Abs(centerRight.x - center.x));
                Drawing.DrawCircle(center, radius, hitboxType.Color, lineWidth, true, Math.Max(8, radius / 16));
            }

            GUI.depth = origDepth;
        }
    }
}