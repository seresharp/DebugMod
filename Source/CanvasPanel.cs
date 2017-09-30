using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DebugMod
{
    public class CanvasPanel
    {
        private CanvasImage background;

        public CanvasPanel(GameObject parent, Texture2D tex, Vector2 pos, Vector2 size)
        {
            background = new CanvasImage(parent, tex, pos, size);
        }
    }
}
