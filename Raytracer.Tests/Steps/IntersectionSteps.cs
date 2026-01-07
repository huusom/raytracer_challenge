using Reqnroll;
using Shouldly;

namespace Raytracer.Tests.Steps;

[Binding]
public class IntersectionSteps(ScenarioContext ctx) : StepsBase(ctx)
{


    [When(@"^(i) ← intersection\((.*), (s)\)$")]
    [Given(@"^(i1|i2|i3|i4) ← intersection\((.*), (s)\)$")]
    public void GivenIntersection(string key, double t, string shapeKey)
    {
        var s = Shape[shapeKey];
        I[key] = Geometry.Intersection.create(s, t);
    }

    [Then(@"^(i)\.t = (.*)$")]
    public void ThenTShouldBe(string key, double expected)
    {
        var actual = I[key];
        actual.t.ShouldBe(expected);
    }

    [Then(@"^(i)\.object = (s)$")]
    public void ThenObjectShouldBe(string key, string shapeKey)
    {
        var expected = Shape[shapeKey];
        var actual = I[key];
        actual.@object.ShouldBe(expected);
    }

    [When(@"^(xs) ← intersections\((i1), (i2)\)$")]
    [Given(@"^(xs) ← intersections\((i2), (i1)\)$")]
    public void CreatingIntersections(string xsKey, string key1, string key2)
    {
        var i1 = I[key1];
        var i2 = I[key2];

        XS[xsKey] = [i1, i2];
    }

    [Given(@"^(xs) ← intersections\((i1), (i2), (i3), (i4)\)$")]
    public void CreatingIntersections(string xsKey, string key1, string key2, string key3, string key4)
    {
        var i1 = I[key1];
        var i2 = I[key2];
        var i3 = I[key3];
        var i4 = I[key4];

        XS[xsKey] = [i1, i2, i3, i4];
    }

    [When(@"^(i) ← hit\((xs)\)$")]
    public void WhenCalculatingHit(string key, string xsKey)
    {
        var xs = XS[xsKey];
        var i = Geometry.Intersection.hit(xs);
        if (i is not null)
            I[key] = i.Value;
    }

    [Then(@"^(i) = (i1|i2|i4)$")]
    public void ThenIntersectionShouldBe(string key, string expectedKey)
    {
        var actual = I[key];
        var expected = I[expectedKey];

        actual.ShouldBe(expected);
    }

    [Then(@"^(i) is nothing$")]
    public void ThenIntersectionShouldBeNothing(string key)
    {
        ctx.ShouldNotContainKey(key);
    }
}