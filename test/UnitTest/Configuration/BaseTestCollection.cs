using Xunit;

namespace UnitTest.Configuration
{
    [CollectionDefinition("Base collection")]
    public abstract class BaseTestCollection : ICollectionFixture<BaseTestFixture>{
        
    }
}