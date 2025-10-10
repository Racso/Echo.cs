using Racso.Echo.LogWriters;

namespace Racso.Echo.Unity
{
    public class EchoUnityFactory : EchoFactory
    {
        public EchoUnityFactory(LogWriterConfig config = null) : base(GetUnityLogger(config))
        {
        }

        private static UnityLogWriter GetUnityLogger(LogWriterConfig config)
        {
            config ??= new LogWriterConfig
            {
                SystemColor = SystemColor.LabelOnly,
                LevelLabels = false, // Unity already has them
                Timestamp = false // Unity already has it
            };

            return new UnityLogWriter(config);
        }
    }
}