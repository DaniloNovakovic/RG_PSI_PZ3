using RG_PSI_PZ3.Models;
using System.Collections.Generic;

namespace RG_PSI_PZ3.Helpers
{
    public class Storage
    {
        public ICollection<LineEntity> LineEntities { get; }
        public Dictionary<long, LineEntity> LineEntityById { get; set; }
        public Dictionary<long, PowerEntityStorageCell> PowerEntityCellById { get; set; }
        public ICollection<PowerEntityStorageCell> PowerEntityCells { get; }

        public Storage()
        {
            LineEntities = new List<LineEntity>();
            LineEntityById = new Dictionary<long, LineEntity>();
            PowerEntityCellById = new Dictionary<long, PowerEntityStorageCell>();
            PowerEntityCells = new List<PowerEntityStorageCell>();
        }

        public void AddRange(IEnumerable<PowerEntity> powerEntities)
        {
            foreach (var entity in powerEntities)
            {
                var cell = new PowerEntityStorageCell { PowerEntity = entity };
                PowerEntityCells.Add(cell);
                PowerEntityCellById[entity.Id] = cell;
            }
        }

        public void AddValidLines(IEnumerable<LineEntity> lineEntities)
        {
            foreach (var line in lineEntities)
            {
                if (!PowerEntityCellById.TryGetValue(line.FirstEnd, out var firstNodeCell))
                    continue;
                if (!PowerEntityCellById.TryGetValue(line.SecondEnd, out var secondNodeCell))
                    continue;

                firstNodeCell.NumberOfConnections++;
                secondNodeCell.NumberOfConnections++;

                LineEntities.Add(line);
                LineEntityById[line.Id] = line;
            }
        }
    }
}