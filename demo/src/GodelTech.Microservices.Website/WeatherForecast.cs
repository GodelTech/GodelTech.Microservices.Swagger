using System;
using System.ComponentModel.DataAnnotations;

namespace GodelTech.Microservices.Website
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Required]
        [MaxLength(120)]
        public string Summary { get; set; }
    }
}
