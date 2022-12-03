using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPiServer.DeveloperTools.Features.LogViewer
{
    [Serializable]
    public class LoggerSettings
    {
        static List<SelectListItem> _levels;

        public LoggerSettings()
        {
            LevelValue = Level.Critical.ToString();
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MaxValue;
        }

        public Level Level
        {
            get
            {
                //if(string.IsNullOrEmpty(LevelValue))
                //{
                //    return Level.All;
                //}
                return
                    (Level)
                    typeof(Level)
                        .GetFields(BindingFlags.Static | BindingFlags.Public)
                        .First(p => string.Equals(p.Name, LevelValue, StringComparison.OrdinalIgnoreCase))
                        .GetValue(null);
            }
        }

        public string LevelValue { get; set; }
        public string LoggerName { get; set; }
        public string ThreadName { get; set; }
        public string TypeOfException { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserName { get; set; }

        public static IEnumerable<SelectListItem> GetLevels()
        {
            return _levels ?? (_levels = new List<SelectListItem>
                               {
                                   //CreateSelectItem(Level.All),
                                   CreateSelectItem(Level.Critical),
                                   CreateSelectItem(Level.Error),
                                   //CreateSelectItem(Level.Fatal),
                                   CreateSelectItem(Level.Debug),
                                   //CreateSelectItem(Level.Info),
                                   //CreateSelectItem(Level.Warn)
                               });
        }

        private static SelectListItem CreateSelectItem(Level level)
        {
            var sel = new SelectListItem
            {
                Text = level.ToString(),
                Value = level.ToString()
            };

            return sel;
        }
    }
}
