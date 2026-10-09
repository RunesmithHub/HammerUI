using HammerUI.Controls;

namespace HammerUI.Tests.Controls;

public sealed class ToolStripeTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(15, 0)]
    [InlineData(17, 1)]
    [InlineData(60, 2)]
    [InlineData(200, 3)]
    public void LandsAnItemBeforeTheFirstButtonWhoseCenterIsBelowThePointer(double y, int expected) =>
        Assert.Equal(expected, ToolStripe.InsertionIndex([16, 52, 88], y));

    [Fact]
    public void PutsAnItemFirstInAnEmptyGroup() => Assert.Equal(0, ToolStripe.InsertionIndex([], 40));

    [Theory]
    [InlineData(100, ToolStripeGroup.Top)]
    [InlineData(299, ToolStripeGroup.Top)]
    [InlineData(300, ToolStripeGroup.Bottom)]
    [InlineData(480, ToolStripeGroup.Bottom)]
    public void DividesTheGroupsAtTheMiddleOfTheGapBetweenThem(double y, ToolStripeGroup expected) =>
        Assert.Equal(expected, ToolStripe.PickGroup(topGroupBottom: 120, bottomGroupTop: 480, y));

    [Fact]
    public void RaisesAChangeOnlyWhenAValueChanges()
    {
        var item = new ToolStripeItem("problems", "Problems", null);
        var changes = new List<string?>();
        item.PropertyChanged += (_, e) => changes.Add(e.PropertyName);

        item.Badge = "3";
        item.Badge = "3";
        item.IsActive = true;

        Assert.Equal([nameof(ToolStripeItem.Badge), nameof(ToolStripeItem.IsActive)], changes);
    }
}
