using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DebugMod
{
    public class CanvasInput
    {
        private GameObject container;
        private GameObject textObj;

        public bool active;

        public CanvasInput(GameObject parent, Texture2D tex, Vector2 pos, Vector2 size, Rect subSprite, Font font, string text, UnityAction<string> submit, int fontSize = 13, FontStyle style = FontStyle.Normal, int textOffset = 0)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(subSprite.width, subSprite.height);
            }

            container = new GameObject();
            container.AddComponent<CanvasRenderer>();
            RectTransform imageTransform = container.AddComponent<RectTransform>();
            imageTransform.sizeDelta = new Vector2(subSprite.width, subSprite.height);
            Image image = container.AddComponent<Image>();
            image.sprite = Sprite.Create(tex, new Rect(subSprite.x, tex.height - subSprite.height, subSprite.width, subSprite.height), Vector2.zero);

            textObj = new GameObject();
            textObj.AddComponent<RectTransform>().sizeDelta = new Vector2(subSprite.width, subSprite.height);
            Text t = textObj.AddComponent<Text>();
            t.text = text;
            t.font = font;
            t.fontSize = fontSize;
            t.fontStyle = style;
            t.alignment = TextAnchor.MiddleLeft;

            textObj.AddComponent<InputField>().textComponent = t;

            textObj.transform.SetParent(container.transform, false);
            textObj.transform.SetPositionX(textObj.transform.GetPositionX() + textOffset);
            container.transform.SetParent(parent.transform, false);

            Vector2 position = new Vector2((pos.x + ((size.x / subSprite.width) * subSprite.width) / 2f) / 1920f, (1080f - (pos.y + ((size.y / subSprite.height) * subSprite.height) / 2f)) / 1080f);
            imageTransform.anchorMin = position;
            imageTransform.anchorMax = position;
            imageTransform.SetScaleX(size.x / subSprite.width);
            imageTransform.SetScaleY(size.y / subSprite.height);

            GameObject.DontDestroyOnLoad(textObj);
            GameObject.DontDestroyOnLoad(container);

            active = true;
        }
    }
}
