using Reqnroll;
using Shouldly;
using Tuple = Raytracer.Math.Tuple.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class RaySteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [Given(@"^(origin) ← (point.*)$")]
    [Given(@"^(direction) ← (vector.*)$")]
    public void GivenTuple(string tupleKey, Tuple tuple)
    {
        Tuple[tupleKey] = tuple;
    }

    [Given(@"^(r) ← ray\((point.*), (vector.*)\)$")]
    public void GivenRay(string rayKey, Tuple origin, Tuple direction)
    {
        Ray[rayKey] = Geometry.Ray.create(origin, direction);
    }

    [When(@"^(r) ← ray\((origin), (direction)\)$")]
    public void WhenCreatingRay(string rayKey, string originKey, string directionKey)
    {
        var origin = Tuple[originKey];
        var direction = Tuple[directionKey];
        var r = Geometry.Ray.create(origin, direction);
        Ray[rayKey] = r;
    }

    [Then(@"^(r).origin = (origin)$")]
    public void ThenOriginShouldBe(string rayKey, string expectedKey)
    {
        var r = Ray[rayKey];
        var expected = Tuple[expectedKey];
        r.origin.ShouldBe(expected);
    }
    
    [Then(@"^(r).direction = (direction)$")]
    public void ThenDirectionShouldBe(string rayKey, string expectedKey)
    {
        var r = Ray[rayKey];
        var expected = Tuple[expectedKey];
        r.direction.ShouldBe(expected);
    }

    [Then(@"^position\((r), (.*)\) = (point.*)$")]
    public void ThenPositionShouldBe(string rayKey, double distance, Tuple expected)
    {
        var r = Ray[rayKey];
        var actual = Geometry.Ray.positionFrom(r, distance);
        actual.ShouldBe(expected);
    }

    [When(@"^(r2) ← transform\((r), (m)\)$")]
    public void WhenTransforming(string key, string rayKey, string transformKey)
    {
        var r = Ray[rayKey];
        var m = Transformation[transformKey];

        Ray[key] = Geometry.Ray.transformationOf(r, m);
    }

    [Then(@"^(r|r2)\.origin = (point.*)$")]
    public void ThenOriginShouldBe(string rayKey, Tuple expected)
    {
        var r = Ray[rayKey];
        r.origin.ShouldBe(expected);
    }    

    [Then(@"^(r|r2)\.direction = (vector.*)$")]
    public void ThenDirectionShouldBe(string rayKey, Tuple expected)
    {
        var r = Ray[rayKey];
        r.direction.ShouldBe(expected);
    }
}