namespace RG_PSI_PZ3.Helpers
{
    public class Configuration
    {
        public Range LatitudeRange { get; } = new Range(min: 45.2325, max: 45.277031);
        public Range LongitudeRange { get; } = new Range(min: 19.793909, max: 19.894459);
        public Range PlaneXRange { get; set; } = new Range(-1.5, 1.5);
        public Range PlaneYRange { get; set; } = new Range(-1, 1);

        public Configuration()
        {
            // TODO: Read values from .xml (Optional)
        }
    }
}