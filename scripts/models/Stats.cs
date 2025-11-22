public class Stats
{
    public int health { get; set; }
    public int stamina { get; set; }
    public int strength { get; set; }
    public int defense { get; set; }


    public Stats(int _health, int _stamina, int _strength, int _defense)
    {
        this.health = _health;
        this.stamina = _stamina;
        this.strength = _strength;
        this.defense = _defense;
    }
}