using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{

    [CreateAssetMenu(fileName = "High Score Data", menuName = "Game/High Score Data")]
    public class HighScoreData : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public long timeTicks;
            public int points;
        }

        public List <Entry> highScores = new();
    }

}
