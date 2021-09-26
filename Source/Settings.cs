using Modding;

namespace DebugMod
{
    //
    //
    //
    //class required for DebugMod class definition
    public class SaveSettings : IModSettings { }

    public class GlobalSettings : IModSettings
    {
        //Save members
        public SerializableIntDictionary binds = new SerializableIntDictionary();

        public bool ConsoleVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool EnemiesPanelVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool HelpPanelVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool InfoPanelVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool MinInfoPanelVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool SaveStatePanelVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool TopMenuVisible
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool FirstRun
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool NumPadForSaveStates
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public int ShowHitBoxes
        {
            get => GetInt();
            set => SetInt(value);
        }

        public int MaxSaveStates
        {
            get => GetInt();
            set => SetInt(value);
        }

        public float AmountToMove
        {
            get => GetFloat(0.1f);
            set => SetFloat(value);
        }
    }
}
