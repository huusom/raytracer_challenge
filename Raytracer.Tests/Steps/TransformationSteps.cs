using Reqnroll;
using Shouldly;
using Matrix = Raytracer.Math.Matrix.M4.T;
using Tuple = Raytracer.Math.Tuple.T;

namespace Raytracer.Tests.Steps;

[Binding]
public class TransformationSteps(ScenarioContext ctx) : StepsBase(ctx)
{
    [StepArgumentTransformation(@"translation\((.*), (.*), (.*)\)")]
    public static Matrix ToTranslation(double x, double y, double z) => Geometry.Transformation.translation(x, y, z);

    [StepArgumentTransformation(@"scaling\((.*), (.*), (.*)\)")]
    public static Matrix ToScaling(double x, double y, double z) => Geometry.Transformation.scaling(x, y, z);

    [StepArgumentTransformation(@"shearing\((.*), (.*), (.*), (.*), (.*), (.*)\)")]
    public static Matrix ToShearing(double xy, double xz, double yx, double yz, double zx, double zy) => Geometry.Transformation.shearing(xy, xz, yx, yz, zx, zy);

    [StepArgumentTransformation(@"rotation_x\((.*)\)")]
    public static Matrix ToRotationX(double radians) => Geometry.Transformation.rotation_x(System.Math.PI / radians);

    [StepArgumentTransformation(@"rotation_y\((.*)\)")]
    public static Matrix ToRotationY(double radians) => Geometry.Transformation.rotation_y(System.Math.PI / radians);

    [StepArgumentTransformation(@"rotation_z\((.*)\)")]
    public static Matrix ToRotationZ(double radians) => Geometry.Transformation.rotation_z(System.Math.PI / radians);

    [StepArgumentTransformation(@"(π / \d|\-?√\d/\d|√\d+)")]
    public static double ToDouble(string arg) => Parser.parseFloat(arg);

    [Given(@"^(transform|C|m|t) ← (translation.*)$")]
    [Given(@"^(transform|B|m) ← (scaling\([^)]*\))$")]
    [Given(@"^(transform) ← (shearing.*)$")]
    [Given(@"(half_quarter|full_quarter|A) ← (rotation_x.*)")]
    [Given(@"(half_quarter|full_quarter) ← (rotation_y.*)")]
    [Given(@"(half_quarter|full_quarter) ← (rotation_z.*)")]
    public void GivenTranslation(string transformationKey, Matrix translation)
    {
        Transformation[transformationKey] = translation;
    }

    [Given(@"^(inv) ← inverse\((transform|half_quarter)\)$")]
    public void GivenInverseTransformation(string transformKey1, string transformKey2)
    {
        Transformation[transformKey1] = Math.Matrix.M4.inverse(Transformation[transformKey2]);
    }

    [Then(@"^(transform|inv|half_quarter|full_quarter|T) \* (p) = (point.*)$")]
    [Then(@"^(transform|inv) \* (v) = (vector.*)$")]
    public void ThenTransformationMultiplicationsShouldBe(string transformationKey, string tupleKey, Tuple expected)
    {
        var t = Transformation[transformationKey];
        var p = Tuple[tupleKey];
        var actual = t * p;

        actual.ShouldBe(expected);
    }

    [Then(@"^(transform) \* (v) = (v)$")]
    public void ThenTransformationMultiplicationsShouldBe(string transformationKey, string tupleKey, string expectedKey)
    {
        var t = Transformation[transformationKey];
        var p = Tuple[tupleKey];
        var actual = t * p;
        var expected = Tuple[expectedKey];

        actual.ShouldBe(expected);
    }

    [When(@"^(p2|p3|p4) ← (A|B|C) \* (p|p2|p3)$")]
    public void WhenTransformationMultiplication(string tupleKey1, string transformKey, string tupleKey2)
    {
        var t = Transformation[transformKey];
        var p = Tuple[tupleKey2];
        Tuple[tupleKey1] = t * p;
    }

    [When(@"^T ← C \* B \* A$")]
    public void WhenChainingTransformations()
    {
        var c = Transformation["C"];
        var b = Transformation["B"];
        var a = Transformation["A"];
        var t = c * b * a;
        Transformation["T"] = t;
    }

    [Then(@"^(p2|p3|p4) = (point.*)$")]
    public void ThenPointShouldBe(string tupleKey, Tuple expected)
    {
        var actual = Tuple[tupleKey];
        actual.ShouldBe(expected);
    }

    [Given(@"^(m) ← (scaling.*) \* (rotation_z.*)$")]
    public void GivenTransformationAsProduct(string key, Matrix scaling, Matrix rotation)
    {
        Transformation[key] = scaling * rotation;
    }


}
