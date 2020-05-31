using System;
using System.Windows.Media;

namespace RG_PSI_PZ3.Helpers
{
    public static class Indices
    {
        public static Int32Collection Cube { get; set; } = new Int32Collection { 2, 3, 1, 2, 1, 0, 7, 1, 3, 7, 5, 1, 6, 5, 7, 6, 4, 5, 6, 2, 0, 2, 0, 4, 2, 7, 3, 2, 6, 7, 0, 1, 5, 0, 5, 4 };
        public static Int32Collection Square { get; set; } = new Int32Collection { 0, 1, 2, 0, 2, 3 };
    }
}