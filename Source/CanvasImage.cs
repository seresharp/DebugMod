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

        public bool active;

        public CanvasImage(GameObject parent, Texture2D tex, Vector2 pos, Vector2 size, Rect subSprite)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(subSprite.width, subSprite.height);
            }

            imageObj = new GameObject();
            imageObj.AddComponent<CanvasRenderer>();
            RectTransform imageTransform = imageObj.AddComponent<RectTransform>();
            imageTransform.sizeDelta = new Vector2(subSprite.width, subSprite.height);
            imageObj.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(subSprite.x, tex.height - subSprite.height, subSprite.width, subSprite.height), Vector2.zero);

            CanvasGroup group = imageObj.AddComponent<CanvasGroup>();
            group.interactable = false;
            group.blocksRaycasts = false;

            imageObj.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2((pos.x + ((size.x / subSprite.width) * subSprite.width) / 2f) / 1920f, (1080f - (pos.y + ((size.y / subSprite.height) * subSprite.height) / 2f)) / 1080f);
            imageTransform.anchorMin = position;
            imageTransform.anchorMax = position;
            imageTransform.SetScaleX(size.x / subSprite.width);
            imageTransform.SetScaleY(size.y / subSprite.height);

            GameObject.DontDestroyOnLoad(imageObj);

            active = true;
        }

        public void SetWidth(float width)
        {
            if (imageObj != null)
            {
                imageObj.GetComponent<RectTransform>().SetScaleX(width / imageObj.GetComponent<RectTransform>().sizeDelta.x);
            }
        }

        public void SetHeight(float height)
        {
            if (imageObj != null)
            {
                imageObj.GetComponent<RectTransform>().SetScaleX(height / imageObj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            if (imageObj != null)
            {
                Vector2 sz = imageObj.GetComponent<RectTransform>().sizeDelta;
                Vector2 position = new Vector2((pos.x + sz.x / 2f) / 1920f, (1080f - (pos.y + sz.y / 2f)) / 1080f);
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
