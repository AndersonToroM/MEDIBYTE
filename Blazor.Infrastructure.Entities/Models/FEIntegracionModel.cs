using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Blazor.Infrastructure.Entities.Models;

public class FeGetToken
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("virtual_operator")]
    public string VirtualOperator { get; set; }
}

public class RespuestaFeGetToken
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("expires")]
    public DateTime Expires { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("virtual_operator_alias")]
    public string VirtualOperatorAlias { get; set; }
}

public class FEResult<T>
{
    [JsonProperty("IsValid")]
    public bool IsValid { get; set; }

    [JsonProperty("Warnings")]
    public List<object> Warnings { get; set; }

    [JsonProperty("Errors")]
    public List<object> Errors { get; set; }

    [JsonProperty("ResultData")]
    public T ResultData { get; set; }

    [JsonProperty("ResultCode")]
    public int ResultCode { get; set; }
}

