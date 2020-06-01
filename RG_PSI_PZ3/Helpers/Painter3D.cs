using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace RG_PSI_PZ3.Helpers
{
    public class Painter3D
    {
        private readonly Model3DGroup _modelGroup;
        private readonly PowerEntityTo3DMapper _powerEntityMapper;
        private readonly LineEntityTo3DMapper _lineMapper;

        public Painter3D(Model3DGroup modelGroup, PowerEntityTo3DMapper powerEntityMapper, LineEntityTo3DMapper lineMapper)
        {
            _modelGroup = modelGroup;
            _lineMapper = lineMapper;
            _powerEntityMapper = powerEntityMapper;
        }

        public void DrawEntities(Storage storage)
        {
            DrawPowerEntities(storage);
            DrawLines(storage);
        }

        private void DrawLines(Storage storage)
        {
            foreach (var lineEntity in storage.LineEntities)
            {
                var models = _lineMapper.MapTo3D(lineEntity);
                models.ForEach(g => _modelGroup.Children.Add(g));
            }
        }

        private void DrawPowerEntities(Storage storage)
        {
            var addedModelsCache = new List<GeometryModel3D>();

            foreach (var cell in storage.PowerEntityCells)
            {
                cell.Model3D = _powerEntityMapper.MapTo3D(cell.PowerEntity);
                cell.UpdateModelColor();

                RiseIfIntersects(cell.Model3D, addedModelsCache);

                addedModelsCache.Add(cell.Model3D);
                _modelGroup.Children.Add(cell.Model3D);
            }
        }

        private static void RiseZ(MeshGeometry3D mesh, double amountToRise)
        {
            for (int i = 0; i < mesh.Positions.Count; i++)
            {
                var currPos = mesh.Positions[i];
                mesh.Positions[i] = new Point3D(currPos.X, currPos.Y, currPos.Z + amountToRise);
            }
        }

        private void RiseIfIntersects(GeometryModel3D model3D, List<GeometryModel3D> addedModelsCache)
        {
            foreach (var existing in addedModelsCache)
            {
                var mesh = (MeshGeometry3D)model3D.Geometry;
                double height = existing.Bounds.SizeZ;
                double emptySpace = height / 1.5;
                double amountToRise = height + emptySpace;

                while (mesh.Bounds.IntersectsWith(existing.Bounds))
                {
                    RiseZ(mesh, amountToRise);
                }

                model3D.Geometry = mesh;
            }
        }
    }
}