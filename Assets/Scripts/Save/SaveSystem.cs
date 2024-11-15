using Newtonsoft.Json;
using UnityEngine;

namespace Save
{
    public class SaveSystem : ISaveSystem
    {
        public void SaveValue<T>(string key, T value)
        {
            string json = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public T LoadValue<T>(string key, T defaultValue = default)
        {
            string json = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return defaultValue;
            }
        }

        public void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
