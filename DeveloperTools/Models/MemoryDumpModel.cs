using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DeveloperTools.Controllers;
using EPiServer.Web;

namespace DeveloperTools.Models
{
    [Serializable]
    public class MemoryDumpModel
    {
        private static List<SelectListItem> _dumptypes;

        public MemoryDumpModel()
        {
            SelectedDumpValue = Enum.GetName(typeof(DumpType), DumpType.MiniDumpWithFullMemory);
            FilePath = VirtualPathUtilityEx.RebasePhysicalPath("[appDataPath]\\Dumps");
        }

        public string Name { get; set; }
        public string FilePath { get; set; }

        public DumpType SelectedDumpType
        {
            get
            {
                if(string.IsNullOrEmpty(SelectedDumpValue))
                {
                    return DumpType.MiniDumpWithFullMemory;
                }

                var res = DumpType.MiniDumpWithFullMemory;
                Enum.TryParse(SelectedDumpValue, true, out res);
                return res;
            }
        }

        public string SelectedDumpValue { get; set; }

        public static IEnumerable<SelectListItem> GetDumpTypes()
        {
            if(_dumptypes == null)
            {
                _dumptypes = new List<SelectListItem>();
                foreach (var n in  Enum.GetNames(typeof(DumpType)))
                {
                    _dumptypes.Add(CreateSelectItem(n));
                }
            }

            return _dumptypes;
        }

        private static SelectListItem CreateSelectItem(string dumpType)
        {
            var sel = new SelectListItem
            {
                Text = dumpType,
                Value = dumpType
            };

            return sel;
        }
    }
}
