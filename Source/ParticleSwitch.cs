using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public static class ParticleSwitch
    {
        private static bool particlesVisible = true;

        public static void Update()
        {
            if (Input.GetKey(KeyCode.LeftBracket))
            {
                if (particlesVisible)
                {
                    foreach (ParticleSystem ps in GameObject.FindObjectsOfType<ParticleSystem>())
                    {
                        ps.Stop();
                        ps.Clear();
                    }
                    particlesVisible = false;
                }
                else
                {
                    foreach (ParticleSystem ps in GameObject.FindObjectsOfType<ParticleSystem>())
                    {
                        ps.Play();
                    }
                    particlesVisible = true;
                }
            }
        }
    }
}
