using Engine.Files;
using Engine.Resources;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

using DatabaseFile = System.Collections.Generic.Dictionary<string, Engine.Data.Template>;

namespace Engine.Data;

public class Database
{
    private readonly ILogger _logger;
    private readonly FileSystem _files;

    private readonly Dictionary<Resource, DatabaseFile> _data;

    public Database(ILoggerFactory loggerFactory, FileSystem files)
    {
        _logger = loggerFactory.CreateLogger<Database>();
        _files = files;

        _data = [];
    }

    public Template? Get(Resource resource)
    {
        var folder = resource.GetDirectory();
        if (folder == string.Empty)
        {
            throw new Exception($"Invalid resource. A data resource should have at least one '{Resource.Separator}' that is not the first character. (The file part.)");
        }

        if (!_data.TryGetValue(folder, out var data))
        {
            if (!Load(folder, out data))
            {
                _logger.LogWarning("Invalid path. File '{}' does not exist for path '{}'", folder, resource);
                return null;
            }
        }

        var file = resource.GetFile();
        if (!data.TryGetValue(file, out var template))
        {
            _logger.LogWarning("Invalid path. Template '{}' was not found in file '{}' for path '{}'", file, folder, resource);
            template = null;
        }

        return template;
    }

    public bool Load(Resource resource)
    {
        if (!Load(resource, out var data))
        {
            return false;
        }

        _data.Add(resource, data!);
        return true;
    }
    
    private bool Load(Resource resource, [MaybeNullWhen(false)] out DatabaseFile data)
    {
        var path = $"{FileSystemSettings.DataFolder}{resource}";
        if (_files.TryReadJsonAsset(path, out data))
        {
            return true;
        }

        data = null;
        return false;
    }

    public bool Unload(Resource resource)
    {
        return _data.Remove(resource);
    }
}
