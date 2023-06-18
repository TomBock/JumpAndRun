using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace UI
{

    public class HighScoreDisplay : MonoBehaviour
    {
        public static HighScoreDisplay Instance { get; private set; }

        [SerializeField] private TMP_Text _text;
        [SerializeField] private HighScoreData _highScoreData;
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            UpdateText();
        }

        public void AddResult(TimeSpan time, int points)
        {
            var existing = _highScoreData.highScores.FirstOrDefault(entry => entry.points == points);
            if (existing.points != default)
            {
                if (existing.timeTicks <= time.Ticks)
                {
                    return;
                }
                _highScoreData.highScores.Remove(existing);
            }
            _highScoreData.highScores.Add(new HighScoreData.Entry
            {
                points = points,
                timeTicks = time.Ticks
            });
            UpdateText();
        }

        private void UpdateText()
        {
            var text = new StringBuilder("Highscores:\n");
            
            var data = _highScoreData.highScores;
            data.Sort((entryA, entryB) => entryA.points - entryB.points);
            
            foreach (var entry in data)
            {
                text.AppendLine($"{entry.points} points: {new TimeSpan(entry.timeTicks):mm':'ss':'ff}");
            }
            _text.text = text.ToString();
        }
    }

}
