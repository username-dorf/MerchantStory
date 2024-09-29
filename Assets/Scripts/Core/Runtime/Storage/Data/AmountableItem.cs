using System;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace Core.Storage
{
    public interface IAmountableItem : IItem
    {
        public int Amount { get; }
    }

    [Serializable]
    public struct AmountableItem : IAmountableItem
    {
        [PrimaryKey]
        [JsonProperty]
        [field: SerializeField]
        public string ID { get; private set; }

        [JsonProperty] [field: SerializeField] public int Amount { get; private set; }

        public AmountableItem(string id, int amount)
        {
            ID = id;
            Amount = amount;
        }
    }
}