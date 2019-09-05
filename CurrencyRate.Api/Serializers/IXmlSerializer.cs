namespace CurrencyRate.Api.Serializers
{
    public interface IXmlSerializer
    {
        T Deserialize<T>(string data);
    }
}