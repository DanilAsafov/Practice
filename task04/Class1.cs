using System;

namespace task04;

public interface ISpaceship {
    void MoveForward();
    void Rotate(int angle);
    void Fire();
    int Speed {get;}
    int FirePower {get;}
}


public class Cruiser : ISpaceship {
    public int Speed => 50;
    public int FirePower => 100;
    public int MoveCount {get; set;}
    public int Rotation {get; set;}
    public int FireCount {get; set;}

    public void MoveForward() {
        MoveCount++;
    }

    public void Rotate(int angle) {
        Rotation = (Rotation + angle / 2) % 360;
    }

    public void Fire() {
        FireCount++;
    }
}


public class Fighter : ISpaceship {
    public int Speed => 100;
    public int FirePower => 50;
    public int MoveCount {get; set;}
    public int Rotation {get; set;}
    public int FireCount {get; set;}

    public void MoveForward() {
        MoveCount++;
    }

    public void Rotate(int angle) {
        Rotation = (Rotation + angle) % 360;
    }

    public void Fire() {
        FireCount++;
    }
}
