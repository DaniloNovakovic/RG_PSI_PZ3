using RG_PSI_PZ3.Models;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RG_PSI_PZ3.Helpers
{
    public class PowerEntityStorageCell
    {
        public PowerEntity PowerEntity { get; set; }
        public GeometryModel3D Model3D { get; set; }

        private int _numberOfConnections;

        public int NumberOfConnections
        {
            get => _numberOfConnections;
            set
            {
                _numberOfConnections = value;

                if (Model3D == null)
                    return;

                UpdateModelColor();
            }
        }

        public void UpdateModelColor()
        {
            if (_numberOfConnections < 3)
            {
                Model3D.Material = new DiffuseMaterial(Brushes.PaleVioletRed);
            }
            else if (_numberOfConnections <= 5)
            {
                Model3D.Material = new DiffuseMaterial(Brushes.MediumVioletRed);
            }
            else
            {
                Model3D.Material = new DiffuseMaterial(Brushes.Red);
            }
        }
    }
}