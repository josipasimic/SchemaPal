﻿using FluentResults;
using SchemaPal.DataTransferObjects;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SchemaPal.Services.UserServices
{
    public class SchemaPalApiService : ISchemaPalApiService
    {
        private string _accessToken = string.Empty;

        private readonly HttpClient _httpClient;

        public SchemaPalApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SchemaPalApi");
        }

        public async Task<Result> RegisterUser(UserRegistration userRegistration)
        {
            var response = await _httpClient.PostAsJsonAsync("Authentication/register", userRegistration);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Fail($"Registracija nije uspjela! HTTP kod greške: {(int)response.StatusCode}. Poruka: {response.ReasonPhrase}");
            }

            return Result.Ok();
        }

        public async Task<Result> LoginUser(UserLogin userLogin)
        {
            var response = await _httpClient.PostAsJsonAsync("Authentication/login", userLogin);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Fail($"Prijava nije uspjela! HTTP kod greške: {(int)response.StatusCode}. Poruka: {response.ReasonPhrase}");
            }

            var accessToken = await response.Content.ReadFromJsonAsync<AccessToken>();

            if (accessToken is null)
            {
                return Result.Fail("Prijava nije uspjela! Molimo pokušajte kasnije.");
            }

            _accessToken = accessToken.Token;

            return Result.Ok();
        }

        public async Task<Result<Guid>> SaveDatabaseSchema(ExtendedSchemaRecord schemaRecord)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var response = await _httpClient.PostAsJsonAsync($"DatabaseSchemas", schemaRecord);

            if (!response.IsSuccessStatusCode)
            {
                var error = new Error(response.ReasonPhrase);
                error.WithMetadata(nameof(HttpStatusCode), response.StatusCode);

                return Result.Fail(error);
            }

            var receivedSchemaId = await response.Content.ReadFromJsonAsync<Guid?>();

            if (!receivedSchemaId.HasValue
                || receivedSchemaId == Guid.Empty)
            {
                return Result.Fail("Greška tijekom spremanja sheme. Molimo pokušajte kasnije.");
            }

            return Result.Ok(receivedSchemaId.Value);
        }

        public async Task<Result<List<ShortSchemaRecord>>> GetDatabaseSchemasForLoggedInUser()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var response = await _httpClient.GetAsync("DatabaseSchemas");

            if (!response.IsSuccessStatusCode)
            {
                var error = new Error(response.ReasonPhrase);
                error.WithMetadata(nameof(HttpStatusCode), response.StatusCode);

                return Result.Fail(error);
            }

            var schemas = await response.Content.ReadFromJsonAsync<List<ShortSchemaRecord>>() ?? [];

            return Result.Ok(schemas);
        }

        public async Task<Result<ExtendedSchemaRecord>> GetDatabaseSchema(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var response = await _httpClient.GetAsync($"DatabaseSchemas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = new Error(response.ReasonPhrase);
                error.WithMetadata(nameof(HttpStatusCode), response.StatusCode);

                return Result.Fail(error);
            }

            var schemaRecord = await response.Content.ReadFromJsonAsync<ExtendedSchemaRecord>();

            if (schemaRecord is null)
            {
                return Result.Fail("Tražena shema nije pronađena.");
            }

            return Result.Ok(schemaRecord);
        }

        public async Task<Result> DeleteDatabaseSchema(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            var response = await _httpClient.DeleteAsync($"DatabaseSchemas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = new Error(response.ReasonPhrase);
                error.WithMetadata(nameof(HttpStatusCode), response.StatusCode);

                return Result.Fail(error);
            }

            return Result.Ok();
        }
    }
}