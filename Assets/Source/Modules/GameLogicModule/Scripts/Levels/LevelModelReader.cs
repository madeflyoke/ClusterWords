using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;

namespace Source.Modules.GameLogicModule.Scripts.Levels
{
    public class LevelModelReader
    {
        private readonly List<LevelModel> _data;
        
        public List<LevelModel> ReadData(string path)
        {
            if (File.Exists(path) == false)
            {
                string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
                File.WriteAllText(path, json);
                return _data;
            }
            string readAllText = File.ReadAllText(path);
            List<LevelModel> deserializeObject = JsonConvert.DeserializeObject<List<LevelModel>>(readAllText);
            return deserializeObject;
        }
    }
}