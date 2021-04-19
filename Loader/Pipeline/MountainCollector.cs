﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class MountainCollector : ICollector<Mountain>
    {
        public void CollectFrom(CollectorContext context)
        {
            context.Mountain.Id = Guid.NewGuid().ToString("D");

            var nameAndAliases = GetNameAndAliases(context.Raw["Name"]);
            context.Mountain.Name = nameAndAliases.Item1;
            context.Mountain.Aliases = nameAndAliases.Item2;

            context.Mountain.Location = new Location
            {
                Type = "Point",
                Coordinates = new[]
                {
                    double.Parse(context.Raw["Longitude"]),
                    double.Parse(context.Raw["Latitude"])
                }
            };
            context.Mountain.GridRef = context.Raw["Grid ref"];
            context.Mountain.Height = double.Parse(context.Raw["Metres"]);
            context.Mountain.DobihId = int.Parse(context.Raw["Number"]);
            context.Mountain.Prominence = new Prominence
            {
                Value = double.Parse(context.Raw["Drop"]),
                MeasuredFrom = context.Raw["Col grid ref"],
                MeasureFromHeight = double.Parse(context.Raw["Col height"])
            };
            context.Mountain.Features = context.Raw["Feature"];
            context.Mountain.Observations = context.Raw["Observations"];
                
            SetParent(context);

            _mountainsByDobihId.Add(context.Mountain.DobihId, context.Mountain);
        }

        public IList<Mountain> Items => _mountainsByDobihId.Select(x => x.Value).ToList();

        private static Tuple<string,List<string>> GetNameAndAliases(string raw)
        {
            var name = string.Empty;
            var alias = string.Empty;
            var aliases = new List<string>();
            var inAlias = false;

            foreach (var c in raw)
            {
                if (inAlias)
                {
                    if (c == ']')
                    {
                        aliases.Add(alias.Trim());
                        inAlias = false;
                    }
                    else
                    {
                        alias += c;
                    }

                }
                else
                {
                    if (c == '[')
                    {
                        inAlias = true;
                        alias = string.Empty;
                    }
                    else
                    {
                        name += c;
                    }
                }
            }

            while (name.Contains("  "))
            {
                name = name.Replace("  ", " ");
            }

            return new Tuple<string, List<string>>(name.Trim(), aliases);
        }

        private void SetParent(CollectorContext context)
        {
            var parentSmcId = context.Raw["Parent (SMC)"];
            var parentMaId = context.Raw["Parent (Ma)"];
            var parentId = int.Parse(string.IsNullOrWhiteSpace(parentSmcId) ? parentMaId : parentSmcId);

            var hasNoParent = parentId == 0;
            if (hasNoParent) return;

            var hasSelfAsParent = parentId == context.Mountain.DobihId;
            if (hasSelfAsParent) return;

            if (!_mountainsByDobihId.ContainsKey(parentId)) return;

            context.Mountain.Parent = new MountainSummary(_mountainsByDobihId[parentId]);
        }

        private readonly Dictionary<int, Mountain> _mountainsByDobihId = new Dictionary<int, Mountain>();
    }
}