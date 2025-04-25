using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

[Serializable]
public class Projectile
{
    public enum Trajectory { Straight, Homing, Spiraling }; //Needs to be revisited for implementation after figuring out out it will be handled by deserialization. 
    // Alternative is pattern matching the string
    public string trajectory;
    public string speed;

    public string lifetime;
    public int sprite;

}
