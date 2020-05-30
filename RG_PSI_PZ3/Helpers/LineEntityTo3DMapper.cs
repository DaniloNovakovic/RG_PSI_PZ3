using RG_PSI_PZ3.Models;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RG_PSI_PZ3.Helpers
{
    public class LineEntityTo3DMapper
    {
        public Brush Brush { get; set; } = Brushes.DarkRed;
        public double LineWidth { get; set; } = 0.005;

        private readonly IPlaneMapper _mapper;

        public LineEntityTo3DMapper(IPlaneMapper mapper)
        {
            _mapper = mapper;
        }

        public List<GeometryModel3D> MapTo3D(LineEntity entity)
        {
            var verts = entity.Vertices;
            var geometryLines = new List<GeometryModel3D>();

            for (int i = 0; i < verts.Count - 1; ++i)
            {
                var first = MapToPlanePoint3D(verts[i]);
                var second = MapToPlanePoint3D(verts[i + 1]);
                var line3d = Make3DLine(first, second, tooltip: entity);
                geometryLines.Add(line3d);
            }

            return geometryLines;
        }

        private Point3D MapToPlanePoint3D(Point vertice)
        {
            double planeX = _mapper.MapLongitudeToPlaneX(vertice.Y);
            double planeY = _mapper.MapLatitudeToPlaneY(vertice.X);
            return new Point3D(planeX, planeY, z: 0);
        }

        private GeometryModel3D Make3DLine(Point3D start, Point3D end, LineEntity tooltip)
        {
            double offset = LineWidth / 2;

            var points = new Point3DCollection()
            {
                new Point3D(start.X + offset, start.Y - offset, start.Z),
                new Point3D(start.X - offset, start.Y + offset, start.Z),
                new Point3D(end.X + offset, end.Y - offset, end.Z),
                new Point3D(end.X - offset, end.Y + offset, end.Z),
            };

            var meshGeometry = new MeshGeometry3D
            {
                Positions = points,
                TriangleIndices = Indices.Square
            };

            return new GeometryModel3D
            {
                Geometry = meshGeometry,
                Material = new DiffuseMaterial(Brush)
            };
        }
    }
}