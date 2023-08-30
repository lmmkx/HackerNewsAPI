using Microsoft.AspNetCore.Mvc.Testing;

namespace HackerNewsAPI.IntegrationTests
{
    public class TestWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
    }
}
