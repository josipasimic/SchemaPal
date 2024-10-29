using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FluentResults;
using Moq;
using Moq.Protected;
using SchemaPal.DataTransferObjects;
using SchemaPal.Services;

namespace Tests.Services
{
    public class SchemaPalApiServiceTest 
    {
        [Theory]
        [MemberData(nameof(HttpClientResponseIsSuccessful))]
        [MemberData(nameof(HttpClientForbiddenResponse))]
        [MemberData(nameof(HttpClientMissingGuidInTheResponse))]
        public async void GivenExtendedSchemaRecord_WhenSavingSchemaCorrectly_ThenReturnCorrectResult(
            ExtendedSchemaRecord extendedSchemaRecord,
            HttpResponseMessage response,
            Result expectedResult)
        {
             var httpMessageHandler = new Mock<HttpMessageHandler>();
            
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

             var httpClient = new HttpClient(httpMessageHandler.Object)
             {
                BaseAddress = new Uri("http://SchemaPalApi")
             };
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
            
            var schemaPalApiService = new SchemaPalApiService(httpClientFactory.Object);
            var result = await schemaPalApiService.SaveDatabaseSchema(extendedSchemaRecord);
            result.Should().BeEquivalentTo(expectedResult);
        }

        public static TheoryData<ExtendedSchemaRecord, HttpResponseMessage, Result> HttpClientResponseIsSuccessful()
        {
            var extendedSchemaRecord = new ExtendedSchemaRecord();
            var guid = Guid.NewGuid();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = JsonContent.Create(guid, typeof(Guid))
            };

            return new TheoryData<ExtendedSchemaRecord, HttpResponseMessage, Result>
            {
                {extendedSchemaRecord, response, Result.Ok()}
            };
        }

        public static TheoryData<ExtendedSchemaRecord, HttpResponseMessage, Result> HttpClientForbiddenResponse()
        {
            var extendedSchemaRecord = new ExtendedSchemaRecord();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                ReasonPhrase = "Error."
            };

            var error = new Error("Error.");
            error.WithMetadata(nameof(HttpStatusCode), HttpStatusCode.Forbidden);
            return new TheoryData<ExtendedSchemaRecord, HttpResponseMessage, Result>
            {
                {extendedSchemaRecord, response, Result.Fail(error)}
            };
        }

        public static TheoryData<ExtendedSchemaRecord, HttpResponseMessage, Result> HttpClientMissingGuidInTheResponse()
        {
            var extendedSchemaRecord = new ExtendedSchemaRecord();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = JsonContent.Create(Guid.Empty, typeof(Guid))
            };
            return new TheoryData<ExtendedSchemaRecord, HttpResponseMessage, Result>
            {
                {extendedSchemaRecord, response, Result.Fail("Greška tijekom spremanja sheme. Molimo pokušajte kasnije.")}
            };
        }
    }
}