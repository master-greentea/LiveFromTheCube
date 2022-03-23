using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    [HelpURL("https://bennykok.gitbook.io/rhythm-game-starter/hierarchy-overview/stats-system")]
    public class StatsSystem : MonoBehaviour
    {
        [Comment("Responsible for advance stats' config and events.", order = 0)]
        [Title("Hit Level Config", false, 2, order = 1)]
        [Tooltip("Config the hit distance difference for each level, such as Perfect,Ok etc")]
        public HitLevelList levels;

        [Title("Events", 2)]
        [CollapsedEvent]
        public StringEvent onComboStatusUpdate;
        [CollapsedEvent]
        public StringEvent onScoreUpdate;
        [CollapsedEvent]
        public StringEvent onMaxComboUpdate;
        [CollapsedEvent]
        public StringEvent onMissedUpdate;

        #region RUNTIME_FIELD
        [NonSerialized] public int combo;
        [NonSerialized] public int maxCombo;
        [NonSerialized] public int missed;
        [NonSerialized] public int score;

        int comboMultiplier = 10; 
        #endregion

        // CUSTOM, added tuning for how much score you lose if you miss
        public int scoreLostWhenMissed;

        [Serializable]
        public class HitLevelList : ReorderableList<HitLevel> { }

        [Serializable]
        public class HitLevel
        {
            public string name;
            public float threshold;
            [HideInInspector]
            public int count;
            public float scorePrecentage = 1;
            public StringEvent onCountUpdate;
        }

        public void AddMissed(int addMissed)
        {
            missed += addMissed;
            onMissedUpdate.Invoke(missed.ToString());
            score -= scoreLostWhenMissed;

            if (score < 0) score = 0; //score clamp
        }

        void Start()
        {
            UpdateScoreDisplay();

            if (TryGetComponent<SongManager>(out var manager))
            {
                manager.onSongStartPlay.AddListener(() =>
                {
                    score = 0;
                    missed = 0;
                    UpdateScoreDisplay();
                });
            }
        }

        public void AddCombo(int addCombo, float deltaDiff, int addScore)
        {
            // print(deltaDiff);
            // combo += (addCombo*comboMultiplier);
            combo += (addCombo);
            if (combo > maxCombo)
            {
                maxCombo = combo;
                onMaxComboUpdate.Invoke(maxCombo.ToString());
            }

            for (int i = 0; i < levels.values.Count; i++)
            {
                var x = levels.values[i];
                if (deltaDiff <= x.threshold)
                {
                    x.count++;
                    score += (int)(addScore * x.scorePrecentage);
                    x.onCountUpdate.Invoke(x.count.ToString());
                    UpdateScoreDisplay();
                    onComboStatusUpdate.Invoke(x.name);
                    // print(x.name);
                    return;
                }
            }

            //When no level matched
            onComboStatusUpdate.Invoke("");

        }

        public void UpdateScoreDisplay()
        {
            onScoreUpdate.Invoke(score.ToString());
        }

        public void ClearEverything()
        {
            combo = 0;
            maxCombo = 0;
            missed = 0;
            score = 0;
            foreach (HitLevel hitLevel in levels)
            {
                hitLevel.count = 0;
                hitLevel.onCountUpdate.Invoke(hitLevel.count.ToString());
            }

            UpdateScoreDisplay();
            onMissedUpdate.Invoke(missed.ToString());
            onMaxComboUpdate.Invoke(maxCombo.ToString());
            onComboStatusUpdate.Invoke("");
        }
    }
}