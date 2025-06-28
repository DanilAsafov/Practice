using Xunit;
using task04;

namespace task04tests;

public class SpaceshipTests {
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats() {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats() {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser() {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldHaveStrongerFirePowerThanCruiser() {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }

    [Fact]
    public void Cruiser_CanMoveAndRotate() {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        cruiser.Rotate(30);
        Assert.Equal(1, cruiser.MoveCount);
        Assert.Equal(15, cruiser.Rotation);
    }

    [Fact]
    public void Fighter_CanMoveAndRotate() {
        var fighter = new Fighter();
        fighter.MoveForward();
        fighter.Rotate(30);
        Assert.Equal(1, fighter.MoveCount);
        Assert.Equal(30, fighter.Rotation);
    }

    [Fact]
    public void Cruiser_CanShoot() {
        var cruiser = new Cruiser();
        cruiser.Fire();
        Assert.Equal(1, cruiser.FireCount);
    }

    [Fact]
    public void Fighter_CanShoot() {
        var fighter = new Fighter();
        fighter.Fire();
        Assert.Equal(1, fighter.FireCount);
    }
}
