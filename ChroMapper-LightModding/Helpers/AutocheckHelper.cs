﻿using Beatmap.Base;
using ChroMapper_LightModding.BeatmapScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace ChroMapper_LightModding.Helpers
{
    internal class AutocheckHelper
    {
        private Plugin plugin;
        private CriteriaCheck criteriaCheck;
        private FileHelper fileHelper;

        public AutocheckHelper(Plugin plugin, CriteriaCheck criteriaCheck, FileHelper fileHelper)
        {
            this.plugin = plugin;
            this.criteriaCheck = criteriaCheck;
            this.fileHelper = fileHelper;
        }

        public void RunAutoCheckOnInfo()
        {
            plugin.currentMapsetReview.Criteria = criteriaCheck.AutoInfoCheck();
        }

        public void RunAutoCheckOnDiff(string characteristic, int difficultyRank, string difficulty)
        {
            fileHelper.CheckDifficultyReviewsExist();
            var song = plugin.BeatSaberSongContainer.Song;
            BeatSaberSong.DifficultyBeatmap diff = song.DifficultyBeatmapSets.Where(x => x.BeatmapCharacteristicName == characteristic).FirstOrDefault().DifficultyBeatmaps.Where(y => y.Difficulty == difficulty && y.DifficultyRank == difficultyRank).FirstOrDefault();

            BaseDifficulty baseDifficulty = song.GetMapFromDifficultyBeatmap(diff);

            if (baseDifficulty.Notes.Any())
            {
                List<BaseNote> notes = baseDifficulty.Notes.ToList();
                notes = notes.OrderBy(o => o.JsonTime).ToList();

                if (notes.Count > 0)
                {
                    List<BaseNote> bombs = baseDifficulty.Bombs.Cast<BaseNote>().Where(n => n.Type == 3).ToList();
                    bombs = bombs.OrderBy(b => b.JsonTime).ToList();

                    List<BaseObstacle> obstacles = baseDifficulty.Obstacles.ToList();
                    obstacles = obstacles.OrderBy(o => o.JsonTime).ToList();

                    BeatmapScanner.BeatmapScanner.Analyzer(notes, bombs, obstacles, BeatSaberSongContainer.Instance.Song.BeatsPerMinute);

                    plugin.currentMapsetReview.DifficultyReviews.Where(x => x.DifficultyCharacteristic == characteristic && x.DifficultyRank == difficultyRank && x.Difficulty == difficulty).FirstOrDefault().Critera = criteriaCheck.AutoDiffCheck(characteristic, difficultyRank, difficulty);
                }
            }
        }
    }
}
