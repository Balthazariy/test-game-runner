namespace TestGame.Settings
{
    public enum SceneNames
    {
        Unknown,

        Init,
        Splash,
        Game,
    }

    public enum GameStates
    {
        Idle,
        Run,
        Dead,
        Complete,
    }

    public enum CacheType
    {
        UserLocalData,
        AppSettingsData,
    }

    public enum Languages
    {
        Unknown,

        Ukrainian,
        Russian,
        English,
    }

    public enum LogTypes
    {
        Unknown,

        Info,
        Warning,
        Error,
        Debug,
    }

    public enum Sounds
    {
        Unknown,

        Click,
        GameBackground,
        ShowView,
        HideView,
    }
}