namespace CurrencyRate.Api.Serializers
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string data);
    }
}