using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ResultObject
{
    [JsonProperty("result")]
    public List<Result> Counties { get; set; }
}
