using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormService.Utilities
{
    static class Files
    {
        public static void createDirectoryIfNotExists(string path) {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}
