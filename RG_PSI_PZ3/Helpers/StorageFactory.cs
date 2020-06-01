namespace RG_PSI_PZ3.Helpers
{
    public static class StorageFactory
    {
        public static void LoadXMLToStorage(GeographicXmlLoader loader, Storage storage)
        {
            var substationEntities = loader.GetSubstationEntities();
            var nodeEntities = loader.GetNodeEntities();
            var switchEntities = loader.GetSwitchEntities();
            var lineEntities = loader.GetLineEntities();

            storage.AddRange(substationEntities);
            storage.AddRange(nodeEntities);
            storage.AddRange(switchEntities);
            storage.AddValidLines(lineEntities);
        }
    }
}