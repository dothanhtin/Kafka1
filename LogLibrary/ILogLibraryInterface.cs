using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogLibrary
{
    public interface ILogLibraryInterface
    {
        Task WriteAsync(string message, string keySearch);
    }
}
