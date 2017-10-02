using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DebugMod
{
    public class CanvasText
    {
        private GameObject textObj;

        public bool active;

        public CanvasText(GameObject parent, Vector2 pos, Font font, string text, int fontSize = 13)
        {
            textObj = new GameObject();
            textObj.AddComponent<CanvasRenderer>();
            RectTransform textTransform = textObj.AddComponent<RectTransform>();
            textTransform.sizeDelta = new Vector2(1920f, 1080f);

            CanvasGroup group = textObj.AddComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;

            Text t = textObj.AddComponent<Text>();
            t.text = text;
            t.font = font;
            t.fontSize = fontSize;
            t.alignment = TextAnchor.UpperLeft;

            textObj.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2((pos.x + 1920f / 2f) / 1920f, (1080f - (pos.y + 1080f / 2f)) / 1080f);
            textTransform.anchorMin = position;
            textTransform.anchorMax = position;

            GameObject.DontDestroyOnLoad(textObj);

            active = true;
        }

        public void UpdateText(string text)
        {
            if (textObj != null)
            {
                textObj.GetComponent<Text>().text = text;
            }
        }

        public void SetActive(bool a)
        {
            active = a;

            if (textObj != null)
            {
                textObj.SetActive(active);
            }
        }

        public void MoveToTop()
        {
            if (textObj != null)
            {
                textObj.transform.SetAsLastSibling();
            }
        }
    }
}
