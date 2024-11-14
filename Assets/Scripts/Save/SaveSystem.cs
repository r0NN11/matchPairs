using UnityEngine;
using Zenject;

namespace Save
{
    public class SaveSystem : ISaveSystem
    {
        [Inject]
        public SaveSystem()
        {
            
        }
        public void SaveValue<T>(string key, T value)
        {
            string json = JsonUtility.ToJson(value);
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
            return JsonUtility.FromJson<T>(json);
        }
    }
}
