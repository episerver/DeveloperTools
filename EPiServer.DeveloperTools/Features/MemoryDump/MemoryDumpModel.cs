using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using EPiServer.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPiServer.DeveloperTools.Features.MemoryDump
{
    public class NativeMethods
    {
        [DllImport("dbghelp.dll",
             EntryPoint = "MiniDumpWriteDump",
             CallingConvention = CallingConvention.StdCall,
             CharSet = CharSet.Unicode,
             ExactSpelling = true,
             SetLastError = true)]
        public static extern bool MiniDumpWriteDump(IntPtr hProcess,
                                                    uint processId,
                                                    IntPtr hFile,
                                                    uint dumpType,
                                                    ref MiniDumpExceptionInformation expParam,
                                                    IntPtr userStreamParam,
                                                    IntPtr callbackParam);

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
        [MarshalAs(UnmanagedType.Bool)] public bool ClientPointers;
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
        MiniDumpValidTypeFlags = 0x0003ffff
    }

    public sealed class MiniDump
    {
        public static void WriteDump(string fileName, DumpType typeOfdumpType)
        {
            MiniDumpExceptionInformation info;
            info.ThreadId = NativeMethods.GetCurrentThreadId();
            info.ClientPointers = false;
            info.ExceptionPointers = Marshal.GetExceptionPointers();

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var processId = (uint)Process.GetCurrentProcess().Id;
                var processHandle = Process.GetCurrentProcess().Handle;
                // Feel free to specify different dump types
                //uint dumpType = (uint) (DumpType.MiniDumpNormal | DumpType.MiniDumpWithDataSegs);
                var dumpType = (uint)typeOfdumpType;
                NativeMethods.MiniDumpWriteDump(processHandle,
                                                processId,
                                                fs.SafeFileHandle.DangerousGetHandle(),
                                                dumpType,
                                                ref info,
                                                IntPtr.Zero,
                                                IntPtr.Zero);
            }
        }
    }

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
                if (string.IsNullOrEmpty(SelectedDumpValue))
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
            if (_dumptypes == null)
            {
                _dumptypes = new List<SelectListItem>();
                foreach (var n in Enum.GetNames(typeof(DumpType)))
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
