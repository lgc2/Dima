using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Fixtures;

public class AuthenticatedTestFixture : TestFixtureBase
{
    private readonly TestFixtureBase _fixture;

    public AuthenticatedTestFixture(TestFixtureBase fixture)
    {
        _fixture = fixture;
    }

    public override async Task InitializeAsync()
    {
        var loginRequest = new LoginRequest
        {
            Email = _fixture.RegisterRequest.Email,
            Password = _fixture.RegisterRequest.Password
        };

        var loginResponse = await AccountClient.LoginAsync(loginRequest);
        Assert.Equal(200, (int)loginResponse!.StatusCode);
    }
}
