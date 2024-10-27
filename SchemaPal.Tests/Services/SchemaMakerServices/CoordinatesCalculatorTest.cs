using FluentAssertions;
using SchemaPal.Enums;
using SchemaPal.SchemaObjects;
using SchemaPal.Services.SchemaMakerServices;

namespace Tests.Services.SchemaMakerServices
{
    public class CoordinatesCalculatorTest
    {
        [Theory]
        [MemberData(nameof(TableWithThreeColumns))]
        public void GivenColumnIdAndTable_WhenCalculatingConnectionPointY_ThenCalculateCorrectly(
            int columnId,
            Table table,
            double expectedConnectionPointY
        )
        {
            var coordinatesCalculator = new CoordinatesCalculator();
            var connectionPointY = coordinatesCalculator.CalculateConnectionPointY(columnId, table);
            connectionPointY.Should().Be(expectedConnectionPointY);
        }

        public static TheoryData<int, Table, double> TableWithThreeColumns()
        {
            var table = new Table
            {
                Id = 1,
                Columns = new List<Column>
                {
                    new Column(1, "Id", 1),
                    new Column(2, "Name", 1),
                    new Column(3, "Surname", 1)
                }
            };

            return new TheoryData<int, Table, double>
            {
                {2, table, 98},
                {1, table, 59},
                {3, null, 0}
            };
        }

        [Theory]
        [MemberData(nameof(AllLeftTableSide))]
        [MemberData(nameof(AllRightTableSide))]
        [MemberData(nameof(MixedTableSides))]
        [MemberData(nameof(NoTableSide))]
        [MemberData(nameof(NoConnectionPoints))]
        public void GivenConnectionPoints_WhenCalculatingConnectionPointsX_ReturnCorrectXCoordinates(
            List<ConnectionPoint> connectionPoints,
            Dictionary<string, double> expectedCoordinatesX)
        {
            var coordinatesCalculator = new CoordinatesCalculator();
            var connectionPointCoordinatesX = coordinatesCalculator.CalculateConnectionPointsX(connectionPoints);
            connectionPointCoordinatesX.Should().BeEquivalentTo(expectedCoordinatesX);
        }

        public static TheoryData<List<ConnectionPoint>, Dictionary<string, double>> AllLeftTableSide()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(0, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(2, 1, SchemaPal.Enums.TableSide.Left),
            };
            var expectedCoordinatesX = new Dictionary<string, double>
            {
                {connectionPoints[0].UniqueIdentifier, 0},
                {connectionPoints[1].UniqueIdentifier, 0},
                {connectionPoints[2].UniqueIdentifier, 0}
            };
            return new TheoryData<List<ConnectionPoint>, Dictionary<string, double>>
            {
                {connectionPoints, expectedCoordinatesX}
            };
        }

        public static TheoryData<List<ConnectionPoint>, Dictionary<string, double>> AllRightTableSide()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(0, 1, SchemaPal.Enums.TableSide.Right),
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Right),
                new ConnectionPoint(2, 1, SchemaPal.Enums.TableSide.Right),
            };
            var expectedCoordinatesX = new Dictionary<string, double>
            {
                {connectionPoints[0].UniqueIdentifier, 290},
                {connectionPoints[1].UniqueIdentifier, 290},
                {connectionPoints[2].UniqueIdentifier, 290}
            };
            return new TheoryData<List<ConnectionPoint>, Dictionary<string, double>>
            {
                {connectionPoints, expectedCoordinatesX}
            };
        }

        public static TheoryData<List<ConnectionPoint>, Dictionary<string, double>> NoTableSide()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(0, 1, SchemaPal.Enums.TableSide.None),
            };
            var expectedCoordinatesX = new Dictionary<string, double>();
            return new TheoryData<List<ConnectionPoint>, Dictionary<string, double>>
            {
                {connectionPoints, expectedCoordinatesX}
            };
        }

        public static TheoryData<List<ConnectionPoint>, Dictionary<string, double>> MixedTableSides()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(0, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Right),
                new ConnectionPoint(2, 1, SchemaPal.Enums.TableSide.None),
            };
            var expectedCoordinatesX = new Dictionary<string, double>
            {
                {connectionPoints[0].UniqueIdentifier, 0},
                {connectionPoints[1].UniqueIdentifier, 290},
            };
            return new TheoryData<List<ConnectionPoint>, Dictionary<string, double>>
            {
                {connectionPoints, expectedCoordinatesX}
            };
        }
        public static TheoryData<List<ConnectionPoint>, Dictionary<string, double>> NoConnectionPoints()
        {
            var expectedCoordinatesX = new Dictionary<string, double>();
            return new TheoryData<List<ConnectionPoint>, Dictionary<string, double>>
            {
                {null, expectedCoordinatesX}
            };
        }

        [Theory]
        [InlineData(10, 20, 1, 1, TableSide.Left, -40)]
        [InlineData(2, 27, 1, 0, TableSide.Right, 47)]
        [InlineData(55, 78, 5, 1, TableSide.None, 62.666666666666664)]
        public void GivenRelationshipData_WhenCalculatingMidPointX_ThenCalculateCorrectlyAccordingToTheOverlapSide(
            double startX,
            double startY, 
            int count, 
            int index,
            TableSide overlapSide, 
            double expectedResult)
        {
            var coordinatesCalculator = new CoordinatesCalculator();
            var midPointX = coordinatesCalculator.CalculateMidPointX(
                (startX,
                startY,
                count,
                index),
                overlapSide);

            midPointX.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(TableWithMultipleColumns))]
        [MemberData(nameof(MissingParameters))]
        public void GivenTableAndConnectionPoints_WhenCalculatingEdgePointY_ThenReturnCorrectResult(
            Table table, 
            List<ConnectionPoint> connectionPoints,
             int columnId, 
             double expectedEdgePointY) 
        {
            var coordinatesCalculator = new CoordinatesCalculator();
            var edgePointY = coordinatesCalculator.CalculateEdgePointY(
                table, 
                connectionPoints, 
                columnId);
            
            edgePointY.Should().Be(expectedEdgePointY);
        }

        public static TheoryData<Table, List<ConnectionPoint>, int, double> TableWithMultipleColumns()
        {
            var table = new Table
            {
                Id = 2,
                Columns = new List<Column>
                {
                    new Column(0, "Key", 2),
                    new Column(1, "Connection", 2),
                    new Column(2, "Point", 2)
                }
            };

            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(2, 0, TableSide.Left),
                new ConnectionPoint(2, 1, TableSide.Right),
                new ConnectionPoint(2, 2, TableSide.None),
                new ConnectionPoint(1, 4, TableSide.Left)
            };

            return new TheoryData<Table, List<ConnectionPoint>, int, double>
            {
                { table, connectionPoints, 0, 2.5},
                { table, connectionPoints, 4, 0},
                { table, connectionPoints, 2, 2.5}
            };
        }

        public static TheoryData<Table, List<ConnectionPoint>, int, double> MissingParameters()
        {
            var table = new Table
            {
                Id = 2,
                Columns = new List<Column>
                {
                    new Column(0, "Key", 2),
                    new Column(1, "Connection", 2),
                }
            };

            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(2, 0, TableSide.Left),
            };

            return new TheoryData<Table, List<ConnectionPoint>, int, double>
            {
                { null, connectionPoints, 0, 0},
                { table, connectionPoints, 2, 0},
                { table, null, 4, 0}
            };
        }
    }
}