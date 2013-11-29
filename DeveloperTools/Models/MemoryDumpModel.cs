using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DeveloperTools.Controllers;
using System.Web.Mvc;
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
                if (String.IsNullOrEmpty(SelectedDumpValue))
                {
                    return DumpType.MiniDumpWithFullMemory;
                }
                DumpType res = DumpType.MiniDumpWithFullMemory;
                Enum.TryParse<DumpType>(SelectedDumpValue, true, out res);
                return res;
            }
        }

        public string SelectedDumpValue
        {
            get;
            set;
        }
        public static IEnumerable<SelectListItem> GetDumpTypes()
        {
            if (_dumptypes == null)
            {
                _dumptypes = new List<SelectListItem>();
                foreach(var n in  Enum.GetNames(typeof(DumpType)))
                {
                    _dumptypes.Add(CreateSelectItem(n));
                }
            }
            return _dumptypes;

        }
        private static SelectListItem CreateSelectItem(String dumpType)
        {
            SelectListItem sel = new SelectListItem();
            sel.Text = dumpType;
            sel.Value = dumpType;
            return sel;
        }
    }


}
