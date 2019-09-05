using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRate.Api.AppSettingsModels;
using CurrencyRate.Api.Dto;
using CurrencyRate.Api.Responses;
using CurrencyRate.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyRate.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly List<Currency> _currencies;
        private readonly Dictionary<string, ICurrencyRateService> _currencyServices;
        private readonly ILogger<ExchangeController> _logger;

        public ExchangeController(IOptions<List<Currency>> currencies, Dictionary<string, ICurrencyRateService> currencyServices, ILogger<ExchangeController> logger)
        {
            _currencies = currencies.Value;
            _currencyServices = currencyServices;
            _logger = logger;
        }

        [HttpGet("{sum}/fordate/{date}")]
        public async Task<BaseResponse<List<ExchangeDto>>> Convert(decimal sum, string date)
        {
            _logger.LogInformation($"ConvertSum is invoked.");
            try
            {
                if (!DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateValue))
                {
                    throw new ArgumentException($"Параметр '{nameof(date)}' не содержит дату. Значение параметра: '{date}'", nameof(date));
                }

                List<ExchangeDto> result = new List<ExchangeDto>();

                foreach (var currency in _currencies)
                {
                    ExchangeDto exchangeDto = new ExchangeDto()
                    {
                        CurrencyCode = currency.Code,
                        CurrencyTitle = currency.Title,
                    };
                    result.Add(exchangeDto);

                    try
                    {
                        ICurrencyRateService service = _currencyServices[currency.ServiceName];
                        decimal convertedSum = await service.ConvertSum(sum, dateValue);
                        exchangeDto.Sum = convertedSum;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        exchangeDto.IsSuccess = false;
                        exchangeDto.Error = ex.Message;
                    }
                }
                return new BaseResponse<List<ExchangeDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BaseResponse<List<ExchangeDto>>(ex);
            }
            
        }
    }
}