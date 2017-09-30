using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DebugMod
{
    public class CanvasButton
    {
        private GameObject buttonObj;
        private GameObject textObj;
        private Button button;
        private Texture2D texture;

        public bool active;

        public CanvasButton(GameObject parent, Texture2D tex, Vector2 pos, Vector2 size, Font font = null, string text = null)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(tex.width, tex.height);
            }

            texture = tex;

            buttonObj = new GameObject();
            buttonObj.AddComponent<CanvasRenderer>();
            RectTransform buttonTransform = buttonObj.AddComponent<RectTransform>();
            buttonTransform.sizeDelta = new Vector2(tex.width, tex.height);
            buttonObj.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            button = buttonObj.AddComponent<Button>();

            buttonObj.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2((pos.x + tex.width / 2f) / 1920f, (1080f - (pos.y + tex.height / 2f)) / 1080f);
            buttonTransform.anchorMin = position;
            buttonTransform.anchorMax = position;
            buttonTransform.SetScaleX(size.x / tex.width);
            buttonTransform.SetScaleY(size.y / tex.height);

            GameObject.DontDestroyOnLoad(buttonObj);

            if (font != null && text != null)
            {
                textObj = new GameObject();
                textObj.AddComponent<RectTransform>().sizeDelta = new Vector2(tex.width, tex.height);
                Text t = textObj.AddComponent<Text>();
                t.text = text;
                t.font = font;
                t.fontSize = 13;
                t.alignment = TextAnchor.MiddleCenter;
                textObj.transform.SetParent(buttonObj.transform, false);

                GameObject.DontDestroyOnLoad(textObj);
            }

            active = true;
        }

        public void AddClickEvent(UnityAction action)
        {
            if (buttonObj != null)
            {
                buttonObj.GetComponent<Button>().onClick.AddListener(action);
            }
        }

        public void UpdateText(string text)
        {
            if (textObj != null)
            {
                textObj.GetComponent<Text>().text = text;
            }
        }

        public void SetWidth(float width)
        {
            if (buttonObj != null && texture != null)
            {
                buttonObj.GetComponent<RectTransform>().SetScaleX(width / texture.width);
            }
        }

        public void SetHeight(float height)
        {
            if (buttonObj != null && texture != null)
            {
                buttonObj.GetComponent<RectTransform>().SetScaleY(height / texture.height);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            if (buttonObj != null)
            {
                Vector2 position = new Vector2(pos.x / 1920f, (1080f - pos.y) / 1080f);
                buttonObj.GetComponent<RectTransform>().anchorMin = position;
                buttonObj.GetComponent<RectTransform>().anchorMax = position;
            }
        }

        public void SetActive(bool b)
        {
            if (buttonObj != null)
            {
                buttonObj.SetActive(b);
                active = b;
            }
        }


        public void SetRenderIndex(int idx)
        {
            buttonObj.transform.SetSiblingIndex(idx);
        }
    }
}
