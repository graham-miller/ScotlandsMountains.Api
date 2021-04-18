﻿using System;
using System.Collections.Generic;
using ScotlandsMountains.Api.Loader.Models;

namespace ScotlandsMountains.Api.Loader.Pipeline
{
    public class MountainCollector : ICollector<Mountain>
    {
        private readonly IList<Mountain> _items = new List<Mountain>();

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

            _items.Add(context.Mountain);
        }

        public IEnumerable<Mountain> Items => _items;
        
        private Tuple<string,List<string>> GetNameAndAliases(string raw)
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
    }
}