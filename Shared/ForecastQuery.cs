using System;
using System.Collections.Generic;
using MediatR;

namespace Ata.DeloSled.Shared
{
    public class ForecastQuery : IRequest<IEnumerable<WeatherForecast>>
    {
    }
}