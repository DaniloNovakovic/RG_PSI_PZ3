using RG_PSI_PZ3.Models;
using System;
using System.Windows;

namespace RG_PSI_PZ3.Helpers.Behaviors
{
    public class PowerEntityClickBehavior
    {
        private readonly Window _window;
        private readonly Storage _storage;

        public PowerEntityClickBehavior(Window window, Storage storage)
        {
            _window = window;
            _storage = storage;
        }

        public void OnClick(PowerEntity powerEntity)
        {
            string mbText = powerEntity.ToString();
            if (_storage.PowerEntityCellById.TryGetValue(powerEntity.Id, out var cell))
            {
                mbText += $", Number of connections: {cell.NumberOfConnections}";
                mbText = mbText.Replace(", ", Environment.NewLine);
            }
            MessageBox.Show(_window, mbText, caption: "Entity Details");
        }
    }
}