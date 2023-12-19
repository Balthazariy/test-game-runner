using TestGame.Settings;
using UnityEngine;

namespace TestGame.Utilities
{
    public class Logger : MonoBehaviour
    {
        public static void Log(string message, LogTypes type)
        {
            if (!AppConstants.DEBUG_ENABLE)
            {
                return;
            }

            switch (type)
            {
                case LogTypes.Info:
                    Debug.Log("<color=#52b788>[INFO]</color> " + message);
                    break;
                case LogTypes.Warning:
                    Debug.Log("<color=#fdffb6>[WARNING]</color> " + message);
                    break;
                case LogTypes.Error:
                    Debug.Log("<color=#f72585>[ERROR]</color> " + message);
                    break;
                case LogTypes.Debug:
                    Debug.Log("<color=#8ecae6>[DEBUG]</color> " + message);
                    break;
            }
        }
    }
}