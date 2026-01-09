using System.Linq;
using Reqnroll;
using Shouldly;
namespace Raytracer.Tests.Steps;

[Binding]
public class WorldSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [Given(@"^(w) ← world\(\)$")]
    public void GivenWorld(string key)
    {
        World[key] = DefaultsBuilder.World([], []);
    }

    [Then(@"^(w) contains no objects$")]
    public void ThenHasNoObjects(string key)
    {
        var w = World[key];
        w.objects.ShouldBeEmpty();
    }

    [Then(@"^(w) has no light source$")]
    public void ThenHasNoLightSource(string key)
    {
        var w = World[key];
        w.lights.ShouldBeEmpty();
    }

    [When(@"^(w) ← default_world\(\)$")]
    [Given(@"^(w) ← default_world\(\)$")]
    public void DefaultWorld(string key)
    {
        World[key] = DefaultsBuilder.World();
    }

    [Then(@"^(w)\.light = (light)$")]
    public void ThenLightShouldBe(string key, string expectedKey)
    {
        var expected = Light[expectedKey];
        var w = World[key];
        w.lights.ShouldHaveSingleItem();
        w.lights.ShouldContain(expected);
    }

    [Then(@"^(w) contains (s\d)$")]
    public void ThenObjectsShouldContain(string key, string expectedKey)
    {
        var expected = Shape[expectedKey];
        var w = World[key];
        w.objects.ShouldContain(expected);
    }

}

