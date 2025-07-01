using System;
using EntityBase;
using UnityEngine;

[Serializable]
public class EntityProfile
{
    [SerializeField] private string _name = "Default Entity";
    public string Name => _name;

    [SerializeField] private string _description = "?";
    public string Description => _description;
    

    // public EntityProfile(string name, string description)
    // {
    //     _name = name;
    //     _description = description;
    // }

    public EntityProfile(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public virtual void SetName(string name)
    {
        _name = name;
    }
}