namespace RG_PSI_PZ3.Helpers
{
    public class LatLonToPlaneMapper : IPlaneMapper
    {
        private readonly Configuration _config;

        public LatLonToPlaneMapper(Configuration config = null)
        {
            _config = config ?? new Configuration();
        }

        public static double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            return minScale + ((value - min) / (max - min) * (maxScale - minScale));
        }

        public double MapLatitudeToPlaneY(double latitude)
        {
            return Scale(latitude, _config.LatitudeRange.Min, _config.LatitudeRange.Max, _config.PlaneYRange.Min, _config.PlaneYRange.Max);
        }

        public double MapLongitudeToPlaneX(double longitude)
        {
            return Scale(longitude, _config.LongitudeRange.Min, _config.LongitudeRange.Max, _config.PlaneXRange.Min, _config.PlaneXRange.Max);
        }
    }
}