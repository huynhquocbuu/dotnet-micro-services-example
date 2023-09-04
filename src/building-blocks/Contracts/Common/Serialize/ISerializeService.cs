namespace Contracts.Common.Serialize;

public interface ISerializeService
{
    string Serialize<T>(T obj);
    string Serialize<T>(T obj, Type type);
    T Deserialize<T>(string text);
}