namespace RG_PSI_PZ3.Helpers
{
    public static class StorageFactory
    {
        public static Storage LoadStorageFromXML(GeographicXmlLoader loader)
        {
            var substationEntities = loader.GetSubstationEntities();
            var nodeEntities = loader.GetNodeEntities();
            var switchEntities = loader.GetSwitchEntities();
            var lineEntities = loader.GetLineEntities();

            var storage = new Storage();
            storage.AddRange(substationEntities);
            storage.AddRange(nodeEntities);
            storage.AddRange(switchEntities);
            storage.AddValidLines(lineEntities);

            return storage;
        }
    }
}