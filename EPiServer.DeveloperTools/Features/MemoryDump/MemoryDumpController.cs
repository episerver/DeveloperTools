//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Runtime.InteropServices;
//using DeveloperTools.Models;
//using EPiServer.DeveloperTools.Features.Common;
//using EPiServer.Web;
//using Microsoft.AspNetCore.Mvc;

//namespace EPiServer.DeveloperTools.Features.MemoryDump
//{
//    public class MemoryDumpController : DeveloperToolsController
//    {
//        public ActionResult Index()
//        {
//            return View(new MemoryDumpModel());
//        }

//        [HttpPost, ActionName("Index")]
//        public ActionResult DumpMemory(MemoryDumpModel memoryDumpModel)
//        {
//            if (string.IsNullOrEmpty(memoryDumpModel.FilePath))
//            {
//                memoryDumpModel.FilePath = VirtualPathUtilityEx.RebasePhysicalPath("[appDataPath]\\Dumps");
//            }
//            if (!Directory.Exists(memoryDumpModel.FilePath))
//            {
//                Directory.CreateDirectory(memoryDumpModel.FilePath);
//            }

//            var timeforfileName = DateTime.Now.ToString().Replace('/', '_').Replace(':', '_');
//            var name = string.Concat(memoryDumpModel.FilePath.LastIndexOf('/') == -1 ? memoryDumpModel.FilePath + '\\' : memoryDumpModel.FilePath,
//                                     Process.GetCurrentProcess().ProcessName + "_" + timeforfileName,
//                                     ".dmp");
//            MiniDump.WriteDump(name, memoryDumpModel.SelectedDumpType);
//            memoryDumpModel.Name = name;

//            return View(memoryDumpModel);
//        }
//    }
//}
