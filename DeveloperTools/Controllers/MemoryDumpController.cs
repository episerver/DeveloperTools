using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Shell.Navigation;
using DeveloperTools.Models;
using EPiServer.Framework.Initialization;
using StructureMap;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EPiServer.Web;

namespace DeveloperTools.Controllers
{
    public class NativeMethods
    {
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, IntPtr hFile, uint dumpType, ref MiniDumpExceptionInformation expParam, IntPtr userStreamParam, IntPtr callbackParam);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess", ExactSpelling = true)]
        public static extern IntPtr GetCurrentProcess();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MiniDumpExceptionInformation
    {
        public uint ThreadId;
        public IntPtr ExceptionPointers;
        [MarshalAs(UnmanagedType.Bool)]
        public bool ClientPointers;
    }

    [Flags]
    public enum DumpType : uint
    {
        MiniDumpNormal = 0x00000000,
        MiniDumpWithDataSegs = 0x00000001,
        MiniDumpWithFullMemory = 0x00000002,
        MiniDumpWithHandleData = 0x00000004,
        MiniDumpFilterMemory = 0x00000008,
        MiniDumpScanMemory = 0x00000010,
        MiniDumpWithUnloadedModules = 0x00000020,
        MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
        MiniDumpFilterModulePaths = 0x00000080,
        MiniDumpWithProcessThreadData = 0x00000100,
        MiniDumpWithPrivateReadWriteMemory = 0x00000200,
        MiniDumpWithoutOptionalData = 0x00000400,
        MiniDumpWithFullMemoryInfo = 0x00000800,
        MiniDumpWithThreadInfo = 0x00001000,
        MiniDumpWithCodeSegs = 0x00002000,
        MiniDumpWithoutAuxiliaryState = 0x00004000,
        MiniDumpWithFullAuxiliaryState = 0x00008000,
        MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
        MiniDumpIgnoreInaccessibleMemory = 0x00020000,
        MiniDumpValidTypeFlags = 0x0003ffff,
    };

    public sealed class MiniDump
    {
        public static void WriteDump(string fileName)
        {
            MiniDumpExceptionInformation info;
            info.ThreadId = NativeMethods.GetCurrentThreadId();
            info.ClientPointers = false;
            info.ExceptionPointers = Marshal.GetExceptionPointers();

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {

                uint processId = (uint)Process.GetCurrentProcess().Id;
                IntPtr processHandle = NativeMethods.GetCurrentProcess();
                IntPtr processHandle2 = Process.GetCurrentProcess().Handle;
                // Feel free to specify different dump types
                //uint dumpType = (uint) (DumpType.MiniDumpNormal | DumpType.MiniDumpWithDataSegs);
                uint dumpType = (uint)DumpType.MiniDumpWithFullMemory;
                NativeMethods.MiniDumpWriteDump(processHandle2,
                                                processId,
                                                fs.SafeFileHandle.DangerousGetHandle(),
                                                dumpType,
                                                ref info,
                                                IntPtr.Zero,
                                                IntPtr.Zero);
            }
        }
    }

    public class MemoryDumpController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            string path = VirtualPathUtilityEx.RebasePhysicalPath("[appDataPath]\\Dumps");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string timeforfileName = DateTime.Now.ToString().Replace('/', '_').Replace(':', '_');
            string fileName = string.Concat(path, Process.GetCurrentProcess().ProcessName + "_" +timeforfileName, ".dmp");
            MiniDump.WriteDump(fileName);
            MemoryDumpModel mdm = new MemoryDumpModel();
            mdm.Name = Process.GetCurrentProcess().ProcessName;
            mdm.Path = fileName;

            return View(mdm);
        }
    }
}
