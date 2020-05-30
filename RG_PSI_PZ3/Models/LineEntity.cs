using System.Collections.Generic;

namespace RG_PSI_PZ3.Models
{
    public class LineEntity
    {
        public string ConductorMaterial { get; set; }

        public long FirstEnd { get; set; }

        public long Id { get; set; }

        public bool IsUnderground { get; set; }

        public string LineType { get; set; }

        public string Name { get; set; }

        public float R { get; set; }

        public long SecondEnd { get; set; }

        public long ThermalConstantHeat { get; set; }

        public List<Point> Vertices { get; set; } = new List<Point>();
    }
}