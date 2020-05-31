using RG_PSI_PZ3.Models;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RG_PSI_PZ3.Helpers
{
    public class PowerEntityTo3DMapper
    {
        private readonly IPlaneMapper _mapper;

        public Brush Brush { get; set; } = Brushes.MediumPurple;

        public const double DefaultElementSize = 0.01;
        public double ElementHeight { get; set; } = DefaultElementSize;
        public double ElementWidth { get; set; } = DefaultElementSize;
        public double ElementZ { get; set; } = DefaultElementSize * 1.5;

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