using UnityEngine;

//Allows for Json Deserialization into a type that supports a string amount which can then be converted to a proper Damage class instance
public class DamageTemp
{
    public string amount;
    public Damage.Type type;
    public DamageTemp()
    {

    }

}
