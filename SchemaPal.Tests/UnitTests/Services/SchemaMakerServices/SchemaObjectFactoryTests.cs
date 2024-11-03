using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SchemaPal.Enums;
using SchemaPal.Helpers.SchemaMakerHelpers;
using SchemaPal.SchemaElements;
using SchemaPal.Services.SchemaMakerServices;

namespace SchemaPal.Tests.UnitTests.Services.SchemaMakerServices
{
    public class SchemaObjectFactoryTests
    {
        [Fact]
        public void GivenDatabaseSchema_WhenCreatingNewTable_ThenSetDefaultTableData()
        {
            // Given
            var expectedTableId = 1;

            var mockCoordinatesCalculator = new Mock<ICoordinatesCalculator>();
            mockCoordinatesCalculator.Setup(calculator => calculator.CalculateConnectionPointsX(It.IsAny<List<ConnectionPoint>>()))
                .Returns(new Dictionary<string, double>());

            var schemaObjectFactory = new SchemaObjectFactory(mockCoordinatesCalculator.Object);
            var databaseSchema = new DatabaseSchema();

            // When
            schemaObjectFactory.CreateNewTable(databaseSchema);

            //Then
            using (new AssertionScope())
            {
                var table = databaseSchema.Tables.Find(t => t.Id == expectedTableId);
                table.Should().NotBeNull();
                table.Name.Should().Be($"Tablica {expectedTableId}");
                table.CoordinateX.Should().Be(SchemaMakerConstants.TableStartingCoordinateX);
                table.CoordinateY.Should().Be(SchemaMakerConstants.TableStartingCoordinateY);
            }
        }

        [Fact]
        public void GivenDatabaseSchema_WhenCreatingNewTable_ThenSetDefaultColumnData()
        {
            // Given
            var expectedColumnId = 1;

            var mockCoordinatesCalculator = new Mock<ICoordinatesCalculator>();
            mockCoordinatesCalculator.Setup(calculator => calculator.CalculateConnectionPointsX(It.IsAny<List<ConnectionPoint>>()))
                .Returns(new Dictionary<string, double>());

            var schemaObjectFactory = new SchemaObjectFactory(mockCoordinatesCalculator.Object);
            var databaseSchema = new DatabaseSchema();

            // When
            schemaObjectFactory.CreateNewTable(databaseSchema);

            //Then
            using (new AssertionScope())
            {
                var table = databaseSchema.Tables.Single();
                table.Columns.Should().HaveCount(1);
                table.Columns[0].Id.Should().Be(expectedColumnId);
                table.Columns[0].Name.Should().Be("Id");
                table.Columns[0].KeyType.Should().Be(KeyType.Primary);
                table.Columns[0].IsNullable.Should().BeFalse();
            }
        }

        [Fact]
        public void GivenDatabaseSchema_WhenCreatingNewTable_ThenAddConnectionPointsCorrectly()
        {
            // Given
            var expectedTableId = 1;
            var expectedColumnId = 1;
            var expectedLeftConnectionPointId = $"{expectedTableId}_{expectedColumnId}_{(int)TableSide.Left}";
            var expectedRightConnectionPointId = $"{expectedTableId}_{expectedColumnId}_{(int)TableSide.Right}";

            var mockLeftConnectionPointCoordinateX = 2;
            var mockRightConnectionPointCoordinateX = 3;
            var mockCoordinateY = 77;

            var mockCoordinatesCalculator = new Mock<ICoordinatesCalculator>();
            mockCoordinatesCalculator.Setup(calculator => calculator.CalculateConnectionPointsX(It.IsAny<List<ConnectionPoint>>()))
                .Returns(new Dictionary<string, double>
                {
                    [expectedLeftConnectionPointId] = mockLeftConnectionPointCoordinateX,
                    [expectedRightConnectionPointId] = mockRightConnectionPointCoordinateX
                });
            mockCoordinatesCalculator.Setup(calculator => calculator.CalculateConnectionPointY(expectedColumnId, It.IsAny<Table>()))
                .Returns(mockCoordinateY);

            var schemaObjectFactory = new SchemaObjectFactory(mockCoordinatesCalculator.Object);
            var databaseSchema = new DatabaseSchema();

            // When
            schemaObjectFactory.CreateNewTable(databaseSchema);

            //Then
            databaseSchema.ConnectionPoints.Should()
                .Satisfy(
                    cp => cp.UniqueIdentifier == expectedLeftConnectionPointId 
                        && cp.ConnectionPointLeftCoordinate == mockLeftConnectionPointCoordinateX 
                        && cp.ConnectionPointTopCoordinate == mockCoordinateY,
                    cp => cp.UniqueIdentifier == expectedRightConnectionPointId 
                        && cp.ConnectionPointLeftCoordinate == mockRightConnectionPointCoordinateX 
                        && cp.ConnectionPointTopCoordinate == mockCoordinateY
                );
        }

        [Theory]
        [MemberData(nameof(CloseRelationshipTestCases))]
        public void GivenDatabaseSchema_WhenClosingNewRelationship_ThenPerformCorrectClosureVariant(
            Relationship newRelationship,
            ConnectionPoint endingConnectionPoint,
            int expectedAddedRelationshipsCount)
        {
            // Given
            var schemaObjectFactory = new SchemaObjectFactory(null);

            var databaseSchema = new DatabaseSchema();
            var existingRelationship = new Relationship
            {
                SourceTableId = 1,
                SourceColumnId = 10,
                DestinationTableId = 2,
                DestinationColumnId = 20
            };
            databaseSchema.Relationships.Add(existingRelationship);

            // When
            schemaObjectFactory.CloseNewRelationship(databaseSchema, newRelationship, endingConnectionPoint, "mock_start_point");

            //Then
            using (new AssertionScope())
            {
                databaseSchema.Relationships.Should().HaveCount(1 + expectedAddedRelationshipsCount);
            }
        }

        public static TheoryData<Relationship, ConnectionPoint, int> CloseRelationshipTestCases()
        {
            return new TheoryData<Relationship, ConnectionPoint, int>
            {
                { new Relationship { SourceTableId = 1, SourceColumnId = 10 }, new ConnectionPoint(3, 30, TableSide.Left), 1 },
                { new Relationship { SourceTableId = 1, SourceColumnId = 10 }, new ConnectionPoint(1, 11, TableSide.Right), 0 },
                { new Relationship { SourceTableId = 1, SourceColumnId = 10 }, new ConnectionPoint(2, 20, TableSide.Right), 0 },
                { new Relationship { SourceTableId = 2, SourceColumnId = 20 }, new ConnectionPoint(1, 10, TableSide.Right), 0 }
            };
        }
    }
}