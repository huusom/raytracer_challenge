using System.Drawing;
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

    [Given(@"^(shape|inner|outer) ← the (first|second) object in (w$)")]
    public void GivenShapeFromWorld(string key, string place, string worldKey)
    {
        var w = World[worldKey];

        switch (place)
        {
            case "first":
                Shape[key] = w.objects[0];
                break;
            case "second":
                Shape[key] = w.objects[1];
                break;
        }
    }

    [Given(@"^(w)\.light ← point_light\((point.*), (color.*)\)$")]
    public void GivenPointLight(string key, Math.Tuple.T position, Graphics.Color.T intensity)
    {
        var w = World[key];
        var l = Scene.Light.pointLightOf(position, intensity);

        World[key] = Scene.World.create(w.objects, [l]);
    }


    [When(@"^(c) ← shade_hit\((w), (comps)\)$")]
    public void WhenShadeHit(string key, string worldKey, string compsKey)
    {
        var w = World[worldKey];
        var comps = Comps[compsKey];
        var c = Scene.World.lightningFrom(w, comps);

        Color[key] = c;
    }

    [When(@"^(c) ← color_at\((w), (r)\)$")]
    public void WhenColorAt(string key, string worldKey, string rayKey)
    {
        var w = World[worldKey];
        var r = Ray[rayKey];

        var c = Scene.World.colorFrom(w, r);

        Color[key] = c;
    }

    [Then(@"^is_shadowed\((w), (p)\) is (true|false)$")]
    public void ThenIsShadowedShouldBe(string worldKey, string pointKey, bool expected)
    {
        var w = World[worldKey];
        var p = Tuple[pointKey];

        var actual = Scene.World.shadowFrom(w, p);
        actual.ShouldBe(expected);
    }

    [Given(@"^(s1|s2) is added to (w)$")]
    public void GivenAddedShape(string shapeKey, string key)
    {
        var s = Shape[shapeKey];
        var w = World[key];

        World[key] = Scene.World.appendShape(w, s);
    }

}
