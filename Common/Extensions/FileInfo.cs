using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class FileInfoExtensionMethods
	{
		public static bool IsAlmostEqual(this FileInfo fileInfo, FileInfo other, int byteThreshold = 0)
		{
			if (fileInfo.Length != other.Length)
				return false;

			var lhs = File.ReadAllBytes(fileInfo.FullName);
			var rhs = File.ReadAllBytes(other.FullName);

			int i = 0;
			int x = 0;
			while (i < fileInfo.Length)
			{
				if (lhs[i] != rhs[i])
					x++;

				i++;
			}

			return byteThreshold >= x;
		}

		public static FileInfo AddFileNamePrefix(this FileInfo fileInfo, String prefix)
		{
			return new FileInfo(fileInfo.DirectoryName + "\\" + prefix + fileInfo.Name);
		}

		public static FileInfo AddFileNameSuffix(this FileInfo fileInfo, String suffix)
		{
			return new FileInfo(fileInfo.DirectoryName + "\\" + fileInfo.Name.Replace(fileInfo.Extension, suffix + fileInfo.Extension));
		}

		public static FileInfo ChangeFilename(this FileInfo fileInfo, String name)
		{
			return new FileInfo(fileInfo.DirectoryName + "\\" + name + fileInfo.Extension);
		}

		public static FileInfo ChangeExtension(this FileInfo fileInfo, String extension)
		{
			return new FileInfo(fileInfo.DirectoryName + "\\" + fileInfo.Name.Replace(fileInfo.Extension, "." + extension.TrimStart('.')));
		}

		public static FileInfo ChangeDirectory(this FileInfo fileInfo, DirectoryInfo directory)
		{
			return new FileInfo(directory.FullName + "\\" + fileInfo.Name);
		}
	}
}
