using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Core;
using System.Web.Mvc;
using System.Globalization;
using System.Reflection;

namespace DeveloperTools.Models
{
    [Serializable]
    public class LoggerSettings
    {
        static List<SelectListItem> _levels;
        public LoggerSettings()
        {
            LevelValue = Level.All.ToString();
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MaxValue;
        }
        public Level Level
        {
            get
            {
                if (String.IsNullOrEmpty(LevelValue))
                {
                    return Level.All;
                }
                return (Level)typeof(Level).GetFields(BindingFlags.Static | BindingFlags.Public).Where(p=>String.Equals(p.Name, LevelValue, StringComparison.OrdinalIgnoreCase)).First().GetValue(null);
            }
        }
        public String LevelValue { get; set; }
        public String LoggerName { get; set; }
        public String ThreadName { get; set; }
        public String TypeOfException { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String UserName { get; set; }


        public static IEnumerable<SelectListItem> GetLevels()
        {
            if (_levels == null)
            {
                _levels = new List<SelectListItem>();
                _levels.Add(CreateSelectItem(Level.All));
                _levels.Add(CreateSelectItem(Level.Critical));
                _levels.Add(CreateSelectItem(Level.Error));
                _levels.Add(CreateSelectItem(Level.Fatal));
                _levels.Add(CreateSelectItem(Level.Debug));
                _levels.Add(CreateSelectItem(Level.Info));
                _levels.Add(CreateSelectItem(Level.Warn));
            }
            return _levels;

        }

        private static SelectListItem CreateSelectItem(Level level)
        {
            SelectListItem sel = new SelectListItem();
            sel.Text = level.DisplayName;
            sel.Value = level.ToString();
            return sel;
        }
    }
}
