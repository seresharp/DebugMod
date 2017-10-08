using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DebugMod
{
    public class CustomInput : MonoBehaviour
    {
        private EventTrigger eventTrigger = null;
        private Text textField = null;
        private float oldScale;
        private bool selected;
        private string defaultText;
        private float blinkTimer;
        private bool blink;
        private float oldTime;

        public string userInput;
        public UnityAction<string> submit;

        void Start()
        {
            eventTrigger = gameObject.GetComponent<EventTrigger>();
            if (eventTrigger == null) eventTrigger = gameObject.AddComponent<EventTrigger>();

            textField = gameObject.GetComponent<Text>();
            if (textField == null) textField = gameObject.AddComponent<Text>();

            defaultText = textField.text;

            AddEventTrigger(OnClick, EventTriggerType.PointerClick);
        }

        private void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
        {
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action()); 

            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            eventTrigger.triggers.Add(entry);
        }

        private void AddEventTrigger(UnityAction<BaseEventData> action, EventTriggerType triggerType)
        {
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventData));

            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            eventTrigger.triggers.Add(entry);
        }

        private void OnClick(BaseEventData data)
        {
            if (!selected)
            {
                selected = true;
                oldScale = Time.timeScale;
                Time.timeScale = 0;
                DebugMod.ih.StopUIInput();
                textField.text = "";
                userInput = "";

                blinkTimer = 0;
                blink = false;
                oldTime = Time.realtimeSinceStartup;
            }
        }

        public void Update()
        {
            if (selected)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    selected = false;
                    DebugMod.ih.StartUIInput();
                    Time.timeScale = oldScale;
                    textField.text = defaultText;
                }
                else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    selected = false;
                    DebugMod.ih.StartUIInput();
                    Time.timeScale = oldScale;
                    textField.text = defaultText;
                    if (submit != null) submit(userInput);
                }
                else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)) && userInput.Length > 0)
                {
                    userInput = userInput.Substring(0, userInput.Length - 1);
                }
                else
                {
                    foreach (char c in Input.inputString)
                    {
                        if (c >= ' ' && c <= '~')
                        {
                            userInput += c;
                        }
                    }
                }

                blinkTimer += Time.realtimeSinceStartup - oldTime;
                oldTime = Time.realtimeSinceStartup;

                if (blinkTimer >= 0.5f)
                {
                    blinkTimer = 0;
                    blink = !blink;
                }

                textField.text = " " + userInput;
                if (blink)
                {
                    textField.text += '|';
                }
            }
            else
            {
                textField.text = " " + defaultText;
            }
        }
    }

}
