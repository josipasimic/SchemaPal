using FluentAssertions;
using Newtonsoft.Json;
using SchemaPal.DataTransferObjects;
using SchemaPal.SchemaElements;
using System.Net.Http.Headers;

namespace SchemaPal.IntegrationTests.Api.SchemaPalApi.DatabaseSchemas
{
    public class SaveDatabaseSchemaTests : SchemaPalApiTestsBase
    {
        [Fact]
        public async Task GivenDatabaseSchemaForUser_WhenSavingTheSchema_ThenResultIsSuccessful()
        {
            var token = await LoginUser(Guid.NewGuid().ToString(), "Password12345!", true);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var databaseSchema = new DatabaseSchema
            {
                Tables = new List<Table>
                {
                    new Table
                    {
                        Id = 1,
                        Name = "table1",
                        Columns = new List<Column>
                        {
                            new Column(234, "column234", 1)
                        }
                    }
                }
            };

            var schemaRecord = new ExtendedSchemaRecord
            {
                Name = "Test schema",
                SchemaJsonFormat = JsonConvert.SerializeObject(databaseSchema)
            };

            var result = await _schemaPalApiService.SaveDatabaseSchema(schemaRecord);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
        }
    }
}
