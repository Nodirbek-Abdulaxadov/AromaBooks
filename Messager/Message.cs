using Messager.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Messager;
public class Message : IDisposable
{
	private string TOKEN = string.Empty;

	/// <summary>
	/// Initialize intance and token
	/// </summary>
	public Message()
	{
		GetToken();
	}

    /// <summary>
    /// Send sms with phone number
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns>SendResultSMS</returns>
    public async Task<SendResultSMS> SendSMSAsync(string phoneNumber)
    {
        int code = GetRandomCode();
        var sms = new SMS()
		{
			mobile_phone = phoneNumber.Replace("+",""),
			from = "4546",
			message = CreateSMS(code),
			callback_url = "https://software-engineer.uz"
		};
		using var httpClient = new HttpClient();
        var httpContent = new StringContent(JsonConvert.SerializeObject(sms),
            Encoding.UTF8, "application/json");

		var htm = new HttpRequestMessage(HttpMethod.Post, Constants.Send_SMS_URL);
		htm.Content = httpContent;
		htm.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

        var httpResponse = await httpClient.SendAsync(htm);

		
		if (httpResponse.IsSuccessStatusCode)
		{
			var result = new SendResultSMS("Successfully sent!");
			result.Success = true;
			result.Code = code;
			return result;
        }
		else
		{
            var result = new SendResultSMS("Something went wrong!");
            result.Success = false;
            return result;
        }
    }

	/// <summary>
	/// Login to sms api service
	/// </summary>
	/// <returns>token</returns>
	private async Task<string> LoginAsync()
	{
		using var httpClient = new HttpClient();
		var data = new
		{
            email = Constants.EMAIL,
			password = Constants.SecretKey
        };
        var httpContent = new StringContent(JsonConvert.SerializeObject(data), 
			Encoding.UTF8, "application/json");
        var httpResponse = await httpClient.PostAsync(Constants.LOGIN_URL, httpContent);

		if (httpResponse.IsSuccessStatusCode)
		{
            var json = await httpResponse.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<JWT_Token>(json).data.token;
        }
		else
		{
			return httpResponse.StatusCode.ToString();
        }
    }

	/// <summary>
	/// If token null or empty gets new token
	/// </summary>
	private void GetToken()
	{
		if (string.IsNullOrEmpty(TOKEN))
		{
            TOKEN = LoginAsync().Result;
        }
	}

	/// <summary>
	/// Creates new random code with 5 digits
	/// </summary>
	/// <returns>new code</returns>
	private int GetRandomCode()
	{
		Random random= new Random();
		return random.Next(10000, 99999);
	}

	/// <summary>
	/// Creates sms template with code
	/// </summary>
	/// <param name="code"></param>
	/// <returns>SMS</returns>
    private string CreateSMS(int code)
        => $"""
            Your verification code is:
            {code}
            """;

    public void Dispose() 
		=> GC.SuppressFinalize(this);
}