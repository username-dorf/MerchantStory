using System;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace Core.Storage
{
    public interface IAmountableItem : IItem
    {
        public uint Amount { get; }
    }

    [Serializable]
    public struct AmountableItem : IAmountableItem
    {
        [PrimaryKey]
        [JsonProperty]
        [field: SerializeField]
        public string ID { get; private set; }

        [JsonProperty] [field: SerializeField] public uint Amount { get; private set; }

        public AmountableItem(string id, uint amount)
        {
            ID = id;
            Amount = amount;
        }
    }
}