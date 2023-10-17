using JsonDirectoryNetCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Reflection;
using JsonDirectoryNetCore.Model.Attribute;

namespace JsonDirectoryNetCore
{
    public class JsonDirectory<T> : IJsonDirectory<T> where T : BaseNodeType<T>
    {
        private readonly Type nodeType;

        public JsonDirectory()
        {
            nodeType = typeof(T);
        }

        public T[] GetDirectoryFromJson(string json)
        {
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                var result = JsonSerializer.Deserialize<T[]>(stream);
                if (result == null)
                {
                    throw new InvalidDataException();
                }
                return result;
            }
        }
        public async Task<T[]> GetDirectoryFromJsonAsync(string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var result = await JsonSerializer.DeserializeAsync<T[]>(stream);
                if (result == null)
                {
                    throw new InvalidDataException();
                }
                return result;
            }
        }
        public async Task<IEnumerable<T>> CreateDirectoriesFromJsonAsync(string root, string json)
        {
            if (Directory.Exists(root) == false)
            {
                Directory.CreateDirectory(root);
            }

            var directories = await GetDirectoryFromJsonAsync(json);

            var created = new List<T>();

            return await CreateNodeDirectory(directories, root, created);
        }

        public async Task<string> CreateJsonString(IEnumerable<T> directories)
        {
            Stream stream = await CreateJsonStream(directories);
            stream.Seek(0, SeekOrigin.Begin);

            using (StreamReader streamReader = new StreamReader(stream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public async Task<IEnumerable<T>> CreateFromNodeType(IEnumerable<T> directories, string root)
        {
            var created = new List<T>();

            return await CreateNodeDirectory(directories, root, created);
        }

        private async Task<Stream> CreateJsonStream(IEnumerable<T> directories)
        {
            using (var stream = new MemoryStream())
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(stream, directories);
                return stream;
            }
        }

        private async Task<IEnumerable<T>> CreateNodeDirectory(IEnumerable<T> directories, string root, ICollection<T> createdDirectories)
        {
            foreach (var directory in directories)
            {
                string currentDirectory = Path.Combine(root, directory.Name);

                DirectoryInfo info;
                if (Directory.Exists(currentDirectory) == false)
                {
                    try
                    {
                        info = Directory.CreateDirectory(currentDirectory);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Directory creation error: " + ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                    }
                }
                else
                {
                    info = new DirectoryInfo(currentDirectory);
                }

                var instance = Activator.CreateInstance<T>();

                PropertyInfo nameProperty = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<NameAttribute>() != null).First();

                PropertyInfo childrenProperty = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<ChildrenAttribute>() != null).First();

                if (nameProperty == null || childrenProperty == null)
                {
                    throw new ArgumentNullException($"Name and Children properties must be implemented with {typeof(NameAttribute).Name}, {typeof(ChildrenAttribute).Name}");
                }

                nameProperty.SetValue(instance, info.Name);

                IEnumerable<T> children = await CreateNodeDirectory(directory.Children, currentDirectory, (ICollection<T>)childrenProperty.GetValue(instance));

                if (childrenProperty != null)
                {
                    childrenProperty.SetValue(instance, children);
                }

                createdDirectories.Add(instance);
            }

            return createdDirectories;
        }
    }
}