using JsonDirectoryNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDirectoryNetCore
{
    public interface IJsonDirectory<T>
    {
        public T[] GetDirectoryFromJson(string json);
        public Task<T[]> GetDirectoryFromJsonAsync(string json);
        public Task<IEnumerable<T>> CreateDirectoriesFromJsonAsync(string root, string json);
        Task<string> CreateJsonString(IEnumerable<T> directories);
        Task<IEnumerable<T>> CreateFromNodeType(IEnumerable<T> directories, string root);
    }
}
