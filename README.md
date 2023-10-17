# JsonDirectory

A library that creates directories from JSON or a NodeType object and developes in .NetCore. Actually, the library hasn't been completed yet.
It has been developing and we are in the beginning of JsonDirectory.
JsonDirectory library can be added as service with IJsonDirectory interface.

```

IJsonDirectory<NodeType> jsonDirectory = new JsonDirectory<NodeType>();

string rootPath = System.IO.Path.Combine(
    System.IO.Directory.GetCurrentDirectory(), "root-folder"
    );

var nodes = new Model.NodeType[] {};

var _task = jsonDirectory.CreateFromNodeType(nodes, rootPath);
_task.Wait();
IEnumerable<NodeType> createdDirectories = _task.Result;
```



