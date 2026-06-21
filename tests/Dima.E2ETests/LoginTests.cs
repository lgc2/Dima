using Dima.E2ETests.Fixture;
using Microsoft.Playwright;

namespace Dima.E2ETests;

public class LoginTests : IClassFixture<TestFixtureBase>
{
    private readonly TestFixtureBase _testFixture;
    private readonly IPage _page;

    public LoginTests(TestFixtureBase testFixture)
    {
        _testFixture = testFixture;
        _page = _testFixture.Page.Result;
    }

    [Fact]
    public async Task ShouldLoginSuccessfully()
    {
        await _page.GotoAsync($"{_testFixture.TestSettings.ApplicationUrl}login");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "E-mail" }).FillAsync("teste@balta.io");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Senha" }).FillAsync("Passw0rd@");
        await _page.GetByRole(AriaRole.Button, new() { Name = "ENTRAR" }).ClickAsync();
        await Assertions.Expect(_page.GetByRole(AriaRole.Heading, new() { Name = "Resumo Financeiro" }))
            .ToBeVisibleAsync();
    }
}
