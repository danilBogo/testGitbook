using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Homework10.Tests;

public class IntegrationCalculatorControllerTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		private const string SuccessString = "Result: ";
		private const string ErrorString = "Error: ";

		public IntegrationCalculatorControllerTests(WebApplicationFactory<Program> fixture)
		{
			_client = fixture.CreateClient();
		}

		[Theory]
		[InlineData("2 %2B 3", "5")]
		[InlineData("(10 - 3) * 2", "14")]
		[InlineData("3 - 4 / 2", "1")]
		[InlineData("8 * (2 %2B 2) - 3 * 4", "20")]
		[InlineData("10 - 3 * (-4)", "22")]
		public async Task Calculate_CalculateExpression_Success(string expression, string result)
		{
			await MakeTest(expression, result, SuccessString);
		}

		[Theory]
		[InlineData("", "Empty string")]
		[InlineData("10 + i", "Unknown character i")]
		[InlineData("3 - 4 / 2.2.3", "There is no such number 2.2.3")]
		[InlineData("2 - 2.23.1 - 23", "There is no such number 2.23.1")]
		[InlineData("(20 - 2.3.4) * 2", "There is no such number 2.3.4")]
		[InlineData("8 %2B 34 - / 2", "There are two operations in a row - and /")]
		[InlineData("4 - 10 * (/10 %2B 2)", "After the opening brackets, only negation can go: (/")]
		[InlineData("10 - 2 * (10 - 1 /)", "There is only a number before the closing parenthesis /)")]
		public async Task Calculate_CalculateExpression_Error(string expression, string result)
		{
			await MakeTest(expression, result, ErrorString);
		}
		
		[Fact]
		public async Task Calculate_CalculateExpression_FastSuccess()
		{
			var rnd = new Random();
			var expression = $"{rnd.Next(0, int.MaxValue / 2)} - {rnd.Next(0, int.MaxValue / 2)}";
            
			var str = $"expression={expression}";
			var stringContent = new StringContent(str, Encoding.UTF8);
			stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            
			var before = await GetRequestExecutionTime(expression);
			var after = await GetRequestExecutionTime(expression);
			Assert.True(before - after > 1000);
		}

		private async Task MakeTest(string expression, string result, string successOrError)
		{
			var response = await SendRequestWithExpression(expression);
			var output = await response.Content.ReadAsStringAsync();

			Assert.Equal(successOrError + result, FindResult(output));
		}

		private async Task<HttpResponseMessage> SendRequestWithExpression(string expression)
		{
			var stringContent = new StringContent($"expression={expression}", Encoding.UTF8);
			stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
			var response = await _client.PostAsync("https://localhost:5001/Calculator/CalculateMathExpression", stringContent);
			return response;
		}

		private async Task<long> GetRequestExecutionTime(string expression)
		{
			var watch = new Stopwatch();
			watch.Start();
			await SendRequestWithExpression(expression);
			watch.Stop();
			var result = watch.ElapsedMilliseconds;
			return result;
		}


		private string FindResult(string html)
		{
			return html.Split("<span id=\"response\" class=\"mt-3\">")[1].Split("</span>")[0];
		}
	}