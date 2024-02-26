using System;

[Serializable]
public class PlayerData
{
    public string Nickname = "NEO";

    public int Level = 1;
    public int XP = 0;
    public int NextLevel = 0;
    public int Kills = 0;

    public int Health = 128; // Representated by binary?
    public int MaxHealth = 128;
    public int Stamina = 1; // 
    public int Shield = 4; // Subnet Mask
    public float ShieldRegenTime = 10;

    public int Respawns = 0;

    public float InvAfterHit = 0.25f;
    public float InvAfterShieldHit = 1;

    public float Speed = 10;

    // TODO
    // Weapons
    // Upgrades
}
