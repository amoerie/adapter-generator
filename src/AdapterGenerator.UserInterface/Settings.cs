namespace AdapterGenerator.UserInterface
{
    internal sealed class Settings : System.Configuration.ApplicationSettingsBase
    {
        public static Settings Default { get; } = ((Settings)(Synchronized(new Settings())));

        [System.Configuration.UserScopedSettingAttribute]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [System.Configuration.DefaultSettingValueAttribute("Steel")]
        public string Accent
        {
            get => ((string)(this["Accent"]));
            set => this["Accent"] = value;
        }

        [System.Configuration.UserScopedSettingAttribute]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [System.Configuration.DefaultSettingValueAttribute("BaseDark")]
        public string Theme
        {
            get => ((string)(this["Theme"]));
            set => this["Theme"] = value;
        }
    }
}