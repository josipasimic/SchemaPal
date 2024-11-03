
using FluentAssertions;
using FluentAssertions.Execution;
using SchemaPal.Enums;
using SchemaPal.Helpers.SchemaMakerHelpers;
using SchemaPal.SchemaElements;
using SchemaPal.Services.SchemaMakerServices;

namespace Tests.Services.SchemaMakerServices
{
    public class StyleServiceTest
    {
        [Theory]
        [MemberData(nameof(CreatingConnectionDefaultColor))]
        [MemberData(nameof(SelectingConnectionSelectedColor))]
        [MemberData(nameof(ResetingConnectionDefaultColor))]
        [MemberData(nameof(SelectingSpecificTableConnectionPointsSelectedColor))]
        [MemberData(nameof(CreatingSpecificConnectionPointsDefaultColor))]
        [MemberData(nameof(EventIsNoneColorNotSet))]
        public void GivenDatabaseSchema_WhenUpdatingAConnection_ThenAllPointsHaveExpectedColor(
            DatabaseSchema databaseSchema,
            ConnectionPointColorEvent connectionPointColorEvent,
            List<string> connectionPointIds,
            HashSet<int> tableIds,
            string expectedColor)
        {
            var styleService = new StyleService();
            styleService.SetConnectionPointsColor(databaseSchema, connectionPointColorEvent, connectionPointIds, tableIds);
            var pointsToCheck = connectionPointIds != null && connectionPointIds.Count > 0 
                ? databaseSchema.ConnectionPointColors.Where(x => connectionPointIds.Contains(x.Key))
                : databaseSchema.ConnectionPointColors;
            var areAllPointsExpectedColor = pointsToCheck.All(x => x.Value == expectedColor);

            areAllPointsExpectedColor.Should().BeTrue();
        }

        public static TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string> CreatingConnectionDefaultColor()
        {
             var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(1, 2, SchemaPal.Enums.TableSide.None),
                new ConnectionPoint(1, 3, SchemaPal.Enums.TableSide.Right)
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints
            };

            return new TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string>
            {
                { 
                    databaseSchema, 
                    ConnectionPointColorEvent.Create, 
                    new List<string>(), 
                    new HashSet<int>(), 
                    SchemaMakerConstants.DefaultConnectionPointColor
                }
            };
        }

        public static TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string> SelectingConnectionSelectedColor()
        {
             var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(1, 2, SchemaPal.Enums.TableSide.None),
                new ConnectionPoint(1, 3, SchemaPal.Enums.TableSide.Right)
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints,
            };
            
            return new TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string>
            {
                { 
                    databaseSchema, 
                    ConnectionPointColorEvent.Select, 
                    new List<string>(), 
                    new HashSet<int>(), 
                    SchemaMakerConstants.SelectedConnectionPointColor
                }
            };
        }

        public static TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string> ResetingConnectionDefaultColor()
        {
             var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(1, 2, SchemaPal.Enums.TableSide.None),
                new ConnectionPoint(1, 3, SchemaPal.Enums.TableSide.Right)
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints
            };
            
            return new TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string>
            {
                { 
                    databaseSchema, 
                    ConnectionPointColorEvent.Reset, 
                    new List<string>(), 
                    new HashSet<int>(), 
                    SchemaMakerConstants.DefaultConnectionPointColor
                }
            };
        }

        public static TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string> SelectingSpecificTableConnectionPointsSelectedColor()
        {
             var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, TableSide.Left),
                new ConnectionPoint(1, 2, TableSide.None),
                new ConnectionPoint(1, 3, TableSide.Right),
                new ConnectionPoint(2, 1, TableSide.Left),
                new ConnectionPoint(2, 2, TableSide.None),
                new ConnectionPoint(2, 3, TableSide.None),
                new ConnectionPoint(2, 4, TableSide.Right)
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints
            };

            var tableIds = new HashSet<int> { 2 }; 
            var connectionPointIds = new List<string>
            {
                connectionPoints[3].UniqueIdentifier,
                connectionPoints[4].UniqueIdentifier,
                connectionPoints[5].UniqueIdentifier,
                connectionPoints[6].UniqueIdentifier
            };
            return new TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string>
            {
                { 
                    databaseSchema, 
                    ConnectionPointColorEvent.Select, 
                    connectionPointIds, 
                    tableIds, 
                    SchemaMakerConstants.SelectedConnectionPointColor
                }
            };
        }

       public static TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string> CreatingSpecificConnectionPointsDefaultColor()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, TableSide.Left),
                new ConnectionPoint(1, 2, TableSide.None),
                new ConnectionPoint(1, 3, TableSide.Right),
                new ConnectionPoint(2, 1, TableSide.Left),
                new ConnectionPoint(2, 2, TableSide.None),
                new ConnectionPoint(2, 3, TableSide.None),
                new ConnectionPoint(2, 4, TableSide.Right)
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints
            };

            var connectionPointIds = new List<string>
            {
                connectionPoints[3].UniqueIdentifier,
                connectionPoints[4].UniqueIdentifier,
                connectionPoints[5].UniqueIdentifier,
                connectionPoints[6].UniqueIdentifier
            };

            return new TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string>
            {
                { 
                    databaseSchema, 
                    ConnectionPointColorEvent.Create, 
                    connectionPointIds, 
                    new HashSet<int>(), 
                    SchemaMakerConstants.DefaultConnectionPointColor
                }
            };
        }

        public static TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string> EventIsNoneColorNotSet()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, SchemaPal.Enums.TableSide.Left),
                new ConnectionPoint(1, 2, SchemaPal.Enums.TableSide.None),
                new ConnectionPoint(1, 3, SchemaPal.Enums.TableSide.Right)
            };

            var connectionPointColors = new Dictionary<string, string>
            {
                {connectionPoints[0].UniqueIdentifier, string.Empty},
                {connectionPoints[1].UniqueIdentifier, string.Empty},
                {connectionPoints[2].UniqueIdentifier, string.Empty},
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints,
                ConnectionPointColors = connectionPointColors
            };
            return new TheoryData<DatabaseSchema, ConnectionPointColorEvent, List<string>, HashSet<int>, string>
            {
                { 
                    databaseSchema, 
                    ConnectionPointColorEvent.None, 
                    new List<string>(), 
                    new HashSet<int>(), 
                    string.Empty
                }
            };
        }

        [Fact]
         public void GivenDatabaseScheme_WhenResetingTheConnectionColor_ThenColorOnlyPointsFromTheSelectedTable()
        {
            var connectionPoints = new List<ConnectionPoint>
            {
                new ConnectionPoint(1, 1, TableSide.Left),
                new ConnectionPoint(1, 2, TableSide.None),
                new ConnectionPoint(1, 3, TableSide.Right),
                new ConnectionPoint(2, 1, TableSide.Left),
                new ConnectionPoint(2, 2, TableSide.None),
                new ConnectionPoint(2, 3, TableSide.None),
                new ConnectionPoint(2, 4, TableSide.Right)
            };
            var relationships = new List<Relationship>
            {
                new Relationship {
                    ConnectionPointIds = new RelationshipConnectionPointIds 
                    { 
                        Start = connectionPoints[0].UniqueIdentifier, 
                        End = connectionPoints[3].UniqueIdentifier 
                    }
                }
            };

            var databaseSchema = new DatabaseSchema
            {
                ConnectionPoints = connectionPoints,
                Relationships = relationships
            };

            var connectionPointIds = new List<string>
            {
                connectionPoints[3].UniqueIdentifier,
                connectionPoints[4].UniqueIdentifier,
                connectionPoints[5].UniqueIdentifier,
                connectionPoints[6].UniqueIdentifier
            };
            
            var styleService = new StyleService();
            styleService.SetConnectionPointsColor(databaseSchema, ConnectionPointColorEvent.Reset, connectionPointIds);
            var coloredPoints = connectionPointIds.Where(x => databaseSchema.Relationships.Any(y => y.ConnectionPointIds.Start == x ||  y.ConnectionPointIds.End == x));

            using (new AssertionScope())
            {
                var allTablePointsSelectedColor = databaseSchema.ConnectionPointColors
                    .Where(x => coloredPoints.Contains(x.Key))
                    .All(x => x.Value == SchemaMakerConstants.SelectedConnectionPointColor);
                allTablePointsSelectedColor.Should().BeTrue();

                var allNonTablePointsDefaultColor =  databaseSchema.ConnectionPointColors
                    .Where(x => !coloredPoints.Contains(x.Key))
                    .All(x => x.Value == SchemaMakerConstants.DefaultConnectionPointColor);
                allNonTablePointsDefaultColor.Should().BeTrue();
            }
        }
    }
}