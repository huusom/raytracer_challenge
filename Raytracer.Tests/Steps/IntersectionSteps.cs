using Reqnroll;
using Shouldly;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Raytracer.Tests.Steps;

[Binding]
public class IntersectionSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [When(@"^(i) ← intersection\((.*), (s)\)$")]
    [Given(@"^(i1|i2|i3|i4) ← intersection\((.*), (s)\)$")]
    [Given(@"^(i) ← intersection\((.*), (shape|s2)\)$")]
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

    [When(@"^(xs) ← intersect\((s), (r)\)$")]
    public void WhenCalculatingIntersection(string intersectKey, string shapeKey, string rayKey)
    {
        var s = Shape[shapeKey];
        var r = Ray[rayKey];
        var xs = Geometry.Intersection.intersect(s, r);

        XS[intersectKey] = xs;
    }

    [Then(@"^(xs).count = (\d)$")]
    public void ThenIntersectionCountShouldBe(string intersectKey, int expected)
    {
        var xs = XS[intersectKey];
        var actual = xs.Length;

        actual.ShouldBe(expected);
    }

    [Then(@"^(xs) is empty$")]
    public void ThenXsShouldBeEmpty(string key)
    {
        var xs = XS[key];

        xs.ShouldBeEmpty();
    }

    [Then(@"^(xs)\[(\d)\] = (.*)")]
    [Then(@"^(xs)\[(\d)\]\.t = (.*)")]
    public void ThenIntersctionValueShouldBe(string intersectKey, int index, double expected)
    {
        var xs = XS[intersectKey];
        var actual = xs[index];
        actual.t.ShouldBe(expected);
    }

    [Then(@"^(xs)\[(\d)\].object = (s|p)$")]
    public void ThenIntersectionObjectShouldBe(string intersectKey, int index, string shapeKey)
    {
        var xs = XS[intersectKey];
        var expected = Shape[shapeKey];
        var actual = xs[index];

        actual.@object.ShouldBe(expected);
    }

    [When(@"^(xs) ← intersect_world\((w), (r)\)$")]
    public void WhenIntersectWorld(string key, string worldKey, string rayKey)
    {
        var w = World[worldKey];
        var r = Ray[rayKey];

        var xs = Raytracer.Scene.World.intersect(w, r);

        XS[key] = xs.ToArray();
    }

    [When(@"^(comps) ← prepare_computations\((i), (r)\)$")]
    public void WhenPrepareComputations(string key, string intersectKey, string rayKey)
    {
        var i = I[intersectKey];
        var r = Ray[rayKey];

        var c = Raytracer.Geometry.Intersection.prepare(i, r);
        Comps[key] = c;
    }

    [Then(@"^(comps)\.t = (i)\.t$")]
    public void ThenCompsTShouldBe(string key, string expectedKey)
    {
        var comps = Comps[key];
        var i = I[expectedKey];

        comps.t.ShouldBe(i.t);
    }

    [Then(@"^(comps)\.object = (i)\.object$")]
    public void ThenCompsObjectShouldBe(string key, string expectedKey)
    {
        var comps = Comps[key];
        var i = I[expectedKey];

        comps.@object.ShouldBe(i.@object);
    }

    [Then(@"^(comps)\.inside = (false|true)$")]
    public void ThenCompsInsideShouldBe(string key, bool expected)
    {
        var comps = Comps[key];

        comps.inside.ShouldBe(expected);
    }

    [Then(@"(comps)\.point = (point.*)$")]
    public void ThenCompsPointShouldBe(string key, Math.Tuple.T expected)
    {
        var comps = Comps[key];

        comps.point.ShouldBe(expected);
    }

    [Then(@"(comps)\.eyev = (vector.*)$")]
    public void ThenCompsEyeShouldBe(string key, Math.Tuple.T expected)
    {
        var comps = Comps[key];

        comps.eye.ShouldBe(expected);
    }

    [Then(@"^(comps)\.normalv = (vector.*)$")]
    public void ThenCompsNormalShouldBe(string key, Math.Tuple.T expected)
    {
        var comps = Comps[key];

        comps.normal.ShouldBe(expected);
    }


    [Then(@"^(comps)\.over_point.z < -EPSILON/2$")]
    public void ThenCompsOverZShouldBe(string key)
    {
        var comps = Comps[key];
        comps.over.z.ShouldBeLessThan(-Library.epsilon / 2);
    }

    [Then(@"^(comps)\.point\.z > comps\.over_point\.z$")]
    public void ThenCompsPointZShouldBe(string key)
    {
        var comps = Comps[key];
        comps.point.z.ShouldBeGreaterThan(comps.over.z);
    }

}