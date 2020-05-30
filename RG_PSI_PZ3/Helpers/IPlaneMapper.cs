namespace RG_PSI_PZ3.Helpers
{
    public interface IPlaneMapper
    {
        double MapLatitudeToPlaneY(double latitude);

        double MapLongitudeToPlaneX(double longitude);
    }
}