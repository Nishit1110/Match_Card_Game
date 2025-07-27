using System.Collections;
using System.Collections.Generic;
using Nishit.Emums;
using UnityEngine;

namespace Nishit.Class
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Card Matching/Card Data", order = 0)]
    public class CardData : ScriptableObject
    {
        public CardValues cardValue;
        public Sprite cardImage;
    }
}
