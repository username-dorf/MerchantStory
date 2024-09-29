using System;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace Core.Storage
{
    public interface IItem
    {
        public string ID { get; }
    }
    
}