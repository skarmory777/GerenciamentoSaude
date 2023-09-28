using Shouldly;
using System.Data.SqlClient;
using Xunit;

namespace SW10.SWMANAGER.Tests.General
{
    public class ConnectionString_Tests
    {
        [Fact]
        public void SqlConnectionStringBuilder_Test()
        {
            var csb = new SqlConnectionStringBuilder("Server=localhost; Database=SWMANAGER; Trusted_Connection=True;");
            csb["Database"].ShouldBe("SWMANAGER");
        }
    }
}
