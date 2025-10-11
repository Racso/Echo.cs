namespace Assets.Racso.EchoLogger.LogWriters
{
    public class LogWriterConfig
    {
        public bool Timestamp = true;
        public bool LevelLabels = true;
        public bool LevelColors = true;
        public SystemColor SystemColor = SystemColor.LabelOnly;
    }

    public enum SystemColor
    {
        None,
        LabelOnly,
        LabelAndMessage
    }
}