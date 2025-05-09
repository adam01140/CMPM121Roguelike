using UnityEngine;

public class Damage
{
    public int amount;
    public int baseDam;
    public enum Type
    {
        PHYSICAL, ARCANE, NATURE, FIRE, ICE, DARK, LIGHT
    }
    public enum EventType
    {
        SINGLE, DOT, HEAL
    }
    public Type type;
    public EventType eventType;
    public Damage(int amount, Type type, EventType eventType = EventType.SINGLE)
    {
        this.amount = amount;
        this.baseDam = amount;
        this.type = type;
        this.eventType = eventType;
    }

    public static Type TypeFromString(string type)
    {
        string t = type.ToLower();
        if (t == "arcane") return Type.ARCANE;
        if (t == "nature") return Type.NATURE;
        if (t == "fire") return Type.FIRE;
        if (t == "ice") return Type.ICE;
        if (t == "dark") return Type.DARK;
        if (t == "light") return Type.LIGHT;
        return Type.PHYSICAL;
    }
}
