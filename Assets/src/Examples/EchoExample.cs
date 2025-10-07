using System.Collections;
using Racso.Echo;
using Racso.Echo.Editor;
using UnityEngine;

namespace Examples
{
    public static class LogSystems
    {
        public static readonly string Gameplay = "Gameplay";
        public static readonly string UI = "UI";
        public static readonly string Network = "Network";
        public static readonly string Audio = "Audio";
    }

    public class EchoExample : MonoBehaviour
    {
        private IEnumerator Start()
        {
            EchoFactory loggerFactory = new EchoFactory();
            EchoEditor.SetSystems(typeof(LogSystems));

            EchoLogger generalLogger = loggerFactory.GetLogger();

            var wait = new WaitForSeconds(2f);
            while (true)
            {
                yield return wait;
                generalLogger.Info(LogSystems.Gameplay, "This is a general log message from the Gameplay system.");
                generalLogger.Warn(LogSystems.UI, "This is a warning log message from the UI system.");
                generalLogger.Error(LogSystems.Network, "This is an error log message from the Network system.");
                generalLogger.Debug(LogSystems.Audio, "This is a debug log message from the Audio system.");
            }
        }
    }
}