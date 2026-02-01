using Godot;
using System;

[GlobalClass]
public partial class Weapons : Resource
{
    [Export] public string WeaponName;

    /// <summary>
    /// Relative position of Weopon Mesh from the origin of Weopon
    /// </summary>
    [Export] public Vector3 MeshRelativePosition; 

    [Export] public Vector3 MeshRotation;

    [Export] public Mesh WeaponMesh;
    
    [Export] public float Damage;
    /// <summary>
    /// Ammos Fired per second
    /// </summary>
    [Export] public float RateOfFire; 
    [Export] public float Magazine;
    [Export] public float ReloadSpeed;
}
