using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using hw.Helper;

namespace Taabus
{
    static class Extension
    {
        internal static string GetAssemblyVersion<T>()
        {
            return Assembly.GetAssembly(typeof(T))
                .GetCustomAttribute<AssemblyVersionAttribute>()
                .Version;
        }

        internal static string GetAssemblyVersion(this object target)
        {
            return Assembly.GetAssembly(target.GetType())
                .GetCustomAttribute<AssemblyVersionAttribute>()
                .Version;
        }

        internal static void OnFileOpen
            (
            this IFileOpenController controller,
            string title,
            string filter,
            int filterIndex = 2,
            bool checkFileExists = false,
            bool restoreDirectory = true
            )
        {
            var d = new OpenFileDialog
            {
                Title = title,
                RestoreDirectory = restoreDirectory,
                InitialDirectory = controller.FileName == null ? "." : controller.FileName.FileHandle().DirectoryName,
                FileName = controller.FileName == null ? null : controller.FileName.FileHandle().Name,
                Filter = filter,
                CheckFileExists = checkFileExists,
                FilterIndex = filterIndex
            };

            if(d.ShowDialog() != DialogResult.OK)
                return;
            var newFile = d.FileName.FileHandle();
            if(!newFile.Exists)
                newFile.String = controller.CreateEmptyFile;
            controller.FileName = d.FileName;
        }

        public static bool Equals<T>(this IEnumerable<T> x, IEnumerable<T> y, Func<T, T, bool> isEqual)
        {
            var xArray = x.ToArray();
            var yArray = y.ToArray();
            return xArray.Length == yArray.Length
                && !xArray.Where((t, i) => !isEqual(t, yArray[i])).Any();
        }

        public static bool Equals<T>(this IEnumerable<T> x, IEnumerable<T> y)
            where T : IEquatable<T> { return Equals(x, y, (xx, yy) => xx.Equals(yy)); }

        internal static ConstructorInfo GetConstructor<T1>
            (this Type type, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
        {
            return GetConstructor(type, bindingFlags, typeof(T1));
        }

        internal static ConstructorInfo GetConstructor<T1, T2>
            (this Type type, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
        {
            return GetConstructor(type, bindingFlags, typeof(T1), typeof(T2));
        }

        static ConstructorInfo GetConstructor(Type type, params Type[] types) { return type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, types, null); }
        static ConstructorInfo GetConstructor(Type type, BindingFlags bindingFlags, params Type[] types) { return type.GetConstructor(bindingFlags, null, types, null); }
    }

    interface IFileOpenController
    {
        string FileName { get; set; }
        string CreateEmptyFile { get; }
    }
}