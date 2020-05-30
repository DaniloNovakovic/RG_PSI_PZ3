using RG_PSI_PZ3.Models;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RG_PSI_PZ3.Helpers
{
    public class PowerEntityTo3DMapper
    {
        public Brush Brush { get; set; } = Brushes.MediumPurple;

        public const double DefaultElementSize = 0.01;
        private readonly IPlaneMapper _mapper;

        public double ElementHeight { get; set; } = DefaultElementSize;
        public double ElementWidth { get; set; } = DefaultElementSize;
        public double ElementZ { get; set; } = DefaultElementSize;

        public Range LatitudeRange { get; } = new Range(min: 45.2325, max: 45.277031);
        public Range LongitudeRange { get; } = new Range(min: 19.793909, max: 19.894459);
        public Range MapXRange { get; set; } = new Range(-1.5, 1.5);
        public Range MapYRange { get; set; } = new Range(-1, 1);

        public PowerEntityTo3DMapper(IPlaneMapper conversion)
        {
            _mapper = conversion;
        }

        public GeometryModel3D MapTo3D(PowerEntity entity)
        {
            var meshGeometry = new MeshGeometry3D
            {
                Positions = MapToPositions(entity),
                TriangleIndices = Indices.Cube
            };

            return new GeometryModel3D
            {
                Geometry = meshGeometry,
                Material = new DiffuseMaterial(Brush)
            };
        }

        private Point3DCollection MapToPositions(PowerEntity entity)
        {
            /**
            * Calculates position wich adhere to Indices.Cube
            * ex. Positions="0 0 0  1 0 0  0 1 0  1 1 0  0 0 1  1 0 1  0 1 1  1 1 1"
            */

            double x = _mapper.MapLongitudeToPlaneX(entity.Y);
            double y = _mapper.MapLatitudeToPlaneY(entity.X);

            x -= ElementWidth / 2;
            y -= ElementHeight / 2;

            return new Point3DCollection()
            {
                new Point3D(x, y, 0),
                new Point3D(x + ElementWidth, y, 0),
                new Point3D(x, y + ElementHeight, 0),
                new Point3D(x + ElementWidth, y + ElementHeight, 0),
                new Point3D(x, y, ElementZ),
                new Point3D(x + ElementWidth, y, ElementZ),
                new Point3D(x, y + ElementHeight, ElementZ),
                new Point3D(x + ElementWidth, y + ElementHeight, ElementZ)
            };
        }
    }
}