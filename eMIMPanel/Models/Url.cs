using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eMIMPanel.Models
{
	public class Url
	{
        [JsonProperty("longUrl")]
		public string LongURL { get; set; }

		[JsonProperty("shortUrl")]
		public string ShortURL { get; set; }

		[JsonIgnore]
		public string CustomSegment { get; set; }

        [JsonProperty("bal")]
        public string bal { get; set; }

        [JsonProperty("AdPanel")]
        public string AdPanel { get; set; }

    }
}