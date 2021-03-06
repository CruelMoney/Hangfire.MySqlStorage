using System;
using System.Linq;
using Xunit;

namespace Hangfire.MySql.Tests
{
    public class MySqlStorageTests : DatabaseTestFixture
    {
        private readonly MySqlStorageOptions _options;

        public MySqlStorageTests()
        {
            _options = new MySqlStorageOptions { PrepareSchemaIfNecessary = false };
        }

        [Fact]
        public void Ctor_ThrowsAnException_WhenConnectionStringIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MySqlStorage((string)null));

            Assert.Equal("nameOrConnectionString", exception.ParamName);
        }

        [Fact]
        public void Ctor_ThrowsAnException_WhenOptionsValueIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MySqlStorage("hello", null));

            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public void Ctor_CanCreateSqlServerStorage_WithExistingConnection()
        {
            using (var connection = ConnectionUtils.CreateConnection())
            {
                var storage = new MySqlStorage(connection);

                Assert.NotNull(storage);
            }
        }

        [Fact]
        public void GetConnection_ReturnsNonNullInstance()
        {
            var storage = CreateStorage();
            using (var connection = (MySqlStorageConnection)storage.GetConnection())
            {
                Assert.NotNull(connection);
            }
        }

        [Fact]
        public void GetComponents_ReturnsAllNeededComponents()
        {
            var storage = CreateStorage();

            var components = storage.GetComponents();

            var componentTypes = components.Select(x => x.GetType()).ToArray();
            Assert.Contains(typeof(ExpirationManager), componentTypes);
        }

        [Fact]
        public void GetMonitoringApi_ReturnsNonNullInstance()
        {
            var storage = CreateStorage();
            var api = storage.GetMonitoringApi();
            Assert.NotNull(api);
        }

        private MySqlStorage CreateStorage()
        {
            return new MySqlStorage(
                ConnectionUtils.GetConnectionString(),
                _options);
        }
    }
}
