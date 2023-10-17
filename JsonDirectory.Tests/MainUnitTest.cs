using JsonDirectoryNetCore.Model;

namespace JsonDirectoryNetCore.Tests
{
    public class MainUnitTest
    {
        private string rootPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "json-folders");

        [Fact]
        public void FromJsonTest()
        {
            var nodes = new Model.NodeType[]
            {
                new NodeType
                {
                    Name = "This PC",
                    Children = new NodeType[]
                    {
                        new NodeType()
                        {
                            Name = "Desktop",
                            Children = new NodeType[]
                            {
                                new NodeType()
                                {
                                    Name = "Images"
                                }
                            }
                        },
                        new NodeType()
                        {
                            Name = "Downloads",
                            Children = new NodeType[] {}
                        }
                    }
                }
            };

            IJsonDirectory<NodeType> jsonDirectory = new JsonDirectory<NodeType>();
            var jsonTask = jsonDirectory.CreateFromNodeType(nodes, rootPath);
            jsonTask.Wait();

            var createdNodes = jsonTask.Result;

            Assert.Equivalent(createdNodes, nodes);
        }

        [Fact]
        public void FromNodeTypeTest()
        {
            var json = @"[{""Name"":""This PC"",""Children"":[{""Name"":""3D Objects"",""Children"":[]},{""Name"":""Desktop"",""Children"":[{""Name"":""Images""}]},{""Name"":""Documents"",""Children"":[{""Name"":""Projects"",""Children"":[{""Name"":""Angular""},{""Name"":""Visual Studio""}]}]}]}]";

            IJsonDirectory<NodeType> jsonDirectory = new JsonDirectory<NodeType>();

            var nodes = jsonDirectory.GetDirectoryFromJson(json);

            var jsonTask = jsonDirectory.CreateDirectoriesFromJsonAsync(rootPath, json);
            jsonTask.Wait();

            var createdNodes = jsonTask.Result;

            Assert.Equivalent(createdNodes, nodes);
        }

        [Fact]
        public void FromNodeTypeFailedTest()
        {
            // Fails when an invalid filename specified. For instance, "This PC\\" is not acceptable
            var json = @"[{""Name"":""This PC\\"",""Children"":[{""Name"":""3D Objects"",""Children"":[]},{""Name"":""Desktop"",""Children"":[{""Name"":""Images""}]},{""Name"":""Documents"",""Children"":[{""Name"":""Projects"",""Children"":[{""Name"":""Angular""},{""Name"":""Visual Studio""}]}]}]}]";

            IJsonDirectory<NodeType> jsonDirectory = new JsonDirectory<NodeType>();

            var nodes = jsonDirectory.GetDirectoryFromJson(json);

            var jsonTask = jsonDirectory.CreateDirectoriesFromJsonAsync(rootPath, json);
            jsonTask.Wait();

            var createdNodes = jsonTask.Result;

            Assert.Equivalent(createdNodes, nodes);
        }

    }
}