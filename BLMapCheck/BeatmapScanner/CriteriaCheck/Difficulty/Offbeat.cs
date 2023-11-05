﻿using BLMapCheck.Classes.Results;
using Parser.Map.Difficulty.V3.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using static BLMapCheck.Classes.Helper.Helper;

namespace BLMapCheck.BeatmapScanner.CriteriaCheck.Difficulty
{
    internal static class Offbeat
    {
        public static readonly float[] AllowedSnap = { 0, 0.125f, 0.167f, 0.25f, 0.333f, 0.375f, 0.5f, 0.625f, 0.667f, 0.75f, 0.833f, 0.875f };

        public static void Check(List<Colornote> notes)
        {
            if (notes.Any())
            {
                var red = notes.Where(n => n.c == 0).ToList();
                var blue = notes.Where(n => n.c == 1).ToList();

                if(red.Count >= 2)
                {
                    for (int i = 0; i < red.Count - 1; i++)
                    {
                        var note = red[i];
                        var precision = (float)Math.Round(note.b % 1, 3);
                        if (!AllowedSnap.Contains(precision) && red[i + 1].b - red[i].b > 0.125)
                        {
                            var reality = RealToFraction(precision, 0.01);
                            CheckResults.Instance.AddResult(new CheckResult()
                            {
                                Characteristic = CriteriaCheckManager.Characteristic,
                                Difficulty = CriteriaCheckManager.Difficulty,
                                Name = "Offbeat Note",
                                Severity = Severity.Info,
                                CheckType = "Offbeat",
                                Description = "Uncommon precision.",
                                ResultData = new() { new("Offbeat", reality.N.ToString() + "/" + reality.D.ToString()) },
                                BeatmapObjects = new() { note }
                            });
                        }
                    }
                }
                if(blue.Count >= 2)
                {
                    for (int i = 0; i < blue.Count - 1; i++)
                    {
                        var note = blue[i];
                        var precision = (float)Math.Round(note.b % 1, 3);
                        if (!AllowedSnap.Contains(precision) && blue[i + 1].b - blue[i].b > 0.125)
                        {
                            var reality = RealToFraction(precision, 0.01);
                            CheckResults.Instance.AddResult(new CheckResult()
                            {
                                Characteristic = CriteriaCheckManager.Characteristic,
                                Difficulty = CriteriaCheckManager.Difficulty,
                                Name = "Offbeat Note",
                                Severity = Severity.Info,
                                CheckType = "Offbeat",
                                Description = "Uncommon precision.",
                                ResultData = new() { new("Offbeat", reality.N.ToString() + "/" + reality.D.ToString()) },
                                BeatmapObjects = new() { note }
                            });
                        }
                    }
                }
            }
        }

    }
}
