using RG_PSI_PZ3.Models;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RG_PSI_PZ3.Helpers
{
    public class LineEntityTo3DMapper
    {
        private readonly IPlaneMapper _mapper;

        public LineEntityTo3DMapper(IPlaneMapper mapper)
        {
            _mapper = mapper;
        }

        public Brush Brush { get; set; } = Brushes.DarkRed;
        public double LineWidth { get; set; } = 0.0005;

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
            return new Point3D(planeX, planeY, z: LineWidth);
        }

        private GeometryModel3D Make3DLine(Point3D start, Point3D end, LineEntity tooltip)
        {
            var vecDiff = end - start;

            var nVector = Vector3D.CrossProduct(vecDiff, new Vector3D(0, 0, 1));
            nVector = Vector3D.Divide(nVector, nVector.Length);
            nVector = Vector3D.Multiply(nVector, LineWidth);

            var points = new Point3DCollection()
            {
                start - nVector,
                start + nVector,
                end + nVector,
                end - nVector
            };

            var meshGeometry = new MeshGeometry3D
            {
                Positions = points,
                TriangleIndices = Indices.Square
            };

            var model = new GeometryModel3D
            {
                Material = new DiffuseMaterial(Brush),
                Geometry = meshGeometry
            };

            model.SetValue(System.Windows.FrameworkElement.TagProperty, tooltip);

            return model;
        }
    }
}