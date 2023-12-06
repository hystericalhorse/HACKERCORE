using System;

[Serializable]
public class PlayerData
{
    public string Nickname = "NEO";

    public int Level = 1;
    public int Kills = 0;

    public int Health = 128; // Representated by binary?
    public int MaxHealth = 128;
    public int Stamina = 1; // 
    public int Shield = 4; // Subnet Mask

    public int Respawns = 0;

    public float invAfterHit = 0.25f;
    public float invAfterShieldHit = 1;

    public float Speed = 10;

    // TODO
    // Weapons
    // Upgrades
}
