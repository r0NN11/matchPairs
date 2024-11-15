namespace Save
{
	public interface ISaveSystem
	{
		void SaveValue<T>(string key, T value);
		T LoadValue<T>(string key, T defaultValue = default);
		void DeleteSave();
	}
}
