using System.Collections.Generic;
using Nishit.Emums;
using UnityEngine;

namespace Nishit.Class
{
    [CreateAssetMenu(
            fileName = "CardDatabase",
            menuName = "Card Matching/Card Database",
            order = 1
        )]
    public class CardDatabase : ScriptableObject
    {
        public List<CardData> cards;

        private Dictionary<CardValues, Sprite> _cardDict;

        public void Init()
        {
            _cardDict = new Dictionary<CardValues, Sprite>();
            foreach (var card in cards)
            {
                if (!_cardDict.ContainsKey(card.cardValue))
                    _cardDict.Add(card.cardValue, card.cardImage);
            }
        }

        public Sprite GetCardSprite(CardValues value)
        {
            if (_cardDict == null)
                Init();

            return _cardDict.TryGetValue(value, out var sprite) ? sprite : null;
        }
    }
}