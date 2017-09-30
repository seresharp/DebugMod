using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DebugMod
{
    public class CanvasImage
    {
        private GameObject imageObj;
        private Texture2D texture;

        public bool active;

        public CanvasImage(GameObject parent, Texture2D tex, Vector2 pos, Vector2 size)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(tex.width, tex.height);
            }

            texture = tex;

            imageObj = new GameObject();
            imageObj.AddComponent<CanvasRenderer>();
            RectTransform imageTransform = imageObj.AddComponent<RectTransform>();
            imageTransform.sizeDelta = new Vector2(tex.width, tex.height);
            imageObj.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

            CanvasGroup group = imageObj.AddComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;

            imageObj.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2(pos.x / 1920f, (1080f - pos.y) / 1080f);
            imageTransform.anchorMin = position;
            imageTransform.anchorMax = position;
            imageTransform.SetScaleX(size.x / tex.width);
            imageTransform.SetScaleY(size.y / tex.height);

            GameObject.DontDestroyOnLoad(imageObj);

            active = true;
        }

        public void SetWidth(float width)
        {
            if (imageObj != null && texture != null)
            {
                imageObj.GetComponent<RectTransform>().SetScaleX(width / texture.width);
            }
        }

        public void SetHeight(float height)
        {
            if (imageObj != null && texture != null)
            {
                imageObj.GetComponent<RectTransform>().SetScaleY(height / texture.height);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            if (imageObj != null)
            {
                Vector2 position = new Vector2(pos.x / 1920f, (1080f - pos.y) / 1080f);
                imageObj.GetComponent<RectTransform>().anchorMin = position;
                imageObj.GetComponent<RectTransform>().anchorMax = position;
            }
        }

        public void SetActive(bool b)
        {
            if (imageObj != null)
            {
                imageObj.SetActive(b);
                active = b;
            }
        }

        public void SetRenderIndex(int idx)
        {
            imageObj.transform.SetSiblingIndex(idx);
        }
    }
}
