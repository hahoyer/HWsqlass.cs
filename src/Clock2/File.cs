using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Clock2
{
    sealed class File
    {
        readonly string _name;

        /// <summary>
        ///     constructs a FileInfo
        /// </summary>
        /// <param name="name"> the filename </param>
        internal static File Create(string name) { return new File(name); }

        File(string name) { _name = name; }

        /// <summary>
        ///     considers the file as a string. If file existe it should be a text file
        /// </summary>
        /// <value> the content of the file if existing else null. </value>
        internal string String
        {
            get
            {
                if(System.IO.File.Exists(_name))
                {
                    var f = System.IO.File.OpenText(_name);
                    var result = f.ReadToEnd();
                    f.Close();
                    return result;
                }
                return null;
            }
            set
            {
                var f = System.IO.File.CreateText(_name);
                f.Write(value);
                f.Close();
            }
        }

        public override string ToString() { return FullName; }

        /// <summary>
        ///     Size of file in bytes
        /// </summary>
        long Size { get { return ((FileInfo) FileSystemInfo).Length; } }

        /// <summary>
        ///     Gets the full path of the directory or file.
        /// </summary>
        string FullName { get { return FileSystemInfo.FullName; } }

        /// <summary>
        ///     Gets the name of the directory or file without path.
        /// </summary>
        public string Name { get { return FileSystemInfo.Name; } }

        /// <summary>
        ///     Gets a value indicating whether a file exists.
        /// </summary>
        bool Exists { get { return FileSystemInfo.Exists; } }

        /// <summary>
        ///     Delete the file
        /// </summary>
        public void Delete() { System.IO.File.Delete(_name); }

        /// <summary>
        ///     returns true if it is a directory
        /// </summary>
        bool IsDirectory { get { return Directory.Exists(_name); } }

        FileSystemInfo _fileInfoCache;

        FileSystemInfo FileSystemInfo
        {
            get
            {
                if(_fileInfoCache == null)
                    if(IsDirectory)
                        _fileInfoCache = new DirectoryInfo(_name);
                    else
                        _fileInfoCache = new FileInfo(_name);
                return _fileInfoCache;
            }
        }
    }
}