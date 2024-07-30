// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;

if (args.Length < 1)
{
    throw new ArgumentException("KILL LA KILL -IF-: File Package (.pac) Tool\n" +
        "Usage:\n" +
        "\tExtract: FilePackage <file>\n" +
        "\tArchive: FilePackage <directory>");
}

if (File.Exists(args[0]))
{
    var buffer = File.ReadAllBytes(args[0]);
    if (GAME_CFilePackage.Magic != BitConverter.ToUInt32(buffer, 0))
    {
        throw new InvalidDataException("The provided file is not a KILL LA KILL -IF- FilePackage!");
    }

    GAME_CFilePackage filePackage = new()
    {
        Version = BitConverter.ToUInt32(buffer, 4),
        EntryCount = BitConverter.ToUInt32(buffer, 12),
        EntryStart = BitConverter.ToInt32(buffer, 16),
        Entries = [],
    };

    int entryStart = filePackage.EntryStart;

    for (int i = 0; i < filePackage.EntryCount; i++)
    {
        int nameLength = 0;
        while (nameLength < 66)
        {
            if (buffer[entryStart + 16 + nameLength] == 0) break;
            nameLength++;
        }

        SFileEntry entry = new()
        {
            Magic = BitConverter.ToUInt32(buffer, entryStart),
            EntrySize = BitConverter.ToUInt32(buffer, entryStart + 4),
            Size = BitConverter.ToUInt32(buffer, entryStart + 8),
            Offset = BitConverter.ToUInt32(buffer, entryStart + 12),
            Name = Encoding.UTF8.GetString(buffer, entryStart + 16, nameLength)
        };

        entry.Data = buffer.Skip((int)entry.Offset).Take((int)entry.Size).ToArray();

        filePackage.Entries.Add(entry);
        entryStart += 0x90;
    }

    var directory = Directory.CreateDirectory(Path.GetFileNameWithoutExtension(args[0]));

    List<Meta_FileEntry> entries = new List<Meta_FileEntry>();
    filePackage.Entries.ForEach(delegate (SFileEntry entry)
    {
        entries.Add(new Meta_FileEntry { Magic = (int)entry.Magic, EntrySize = (int)entry.EntrySize, Name = entry.Name });
    });

    var meta = new Meta_FilePackage()
    {
        Version = (int)filePackage.Version,
        EntryStart = filePackage.EntryStart,
        Entries = entries,
    };

    var options = new JsonSerializerOptions { WriteIndented = true };
    string metaJson = JsonSerializer.Serialize(meta, options);

    File.WriteAllText(directory + "\\meta.json", metaJson);

    foreach (var entry in filePackage.Entries)
    {
        File.WriteAllBytes(directory + "\\" + entry.Name, entry.Data);
    }
}
else if (Directory.Exists(args[0]))
{
    var metaJson = File.ReadAllText(args[0] + "\\meta.json");
    var meta = JsonSerializer.Deserialize<Meta_FilePackage>(metaJson);

    var filePackage = new GAME_CFilePackage()
    {
        Version = (uint)meta.Version,
        EntryStart = meta.EntryStart,
        EntryCount = (uint)meta.Entries.Count,
        Entries = [],
    };

    for (int i = 0; i < filePackage.EntryCount; i++)
    {
        var entry = new SFileEntry
        {
            Magic = (uint)meta.Entries[i].Magic,
            EntrySize = (uint)meta.Entries[i].EntrySize,
            Name = meta.Entries[i].Name,
        };

        entry.Data = File.ReadAllBytes(args[0] + "\\" + entry.Name);
        entry.Size = (uint)entry.Data.Length;

        filePackage.Entries.Add(entry);
    }

    using var stream = File.Open(args[0] + ".pac", FileMode.Create);
    using var writer = new BinaryWriter(stream, Encoding.ASCII, false);

    writer.Write(GAME_CFilePackage.Magic);
    writer.Write(filePackage.Version);
    writer.Write(0);
    writer.Write(filePackage.EntryCount);
    writer.Write(filePackage.EntryStart);

    for (int i = 20; i < filePackage.EntryStart; i++)
    {
        writer.Write((byte)0);
    }

    uint dataSize = 0;

    foreach (var entry in filePackage.Entries)
    {
        var offset = (uint)(filePackage.EntryStart + filePackage.EntryCount * 0x90 + dataSize);

        writer.Write(entry.Magic);
        writer.Write(entry.EntrySize);
        writer.Write(entry.Size);
        writer.Write(offset);
        writer.Write(Encoding.ASCII.GetBytes(entry.Name));
        for (var i = 0x10 + entry.Name.Length; i < 0x90; i++)
        {
            writer.Write((byte)0);
        }

        dataSize += entry.Size;
        while (dataSize % 8 != 0)
        {
            dataSize++;
        }
        dataSize += 8;
    }

    for (int i = 0; i < filePackage.EntryCount; i++)
    {
        writer.Write(filePackage.Entries[i].Data);
        while (writer.BaseStream.Length % 8 != 0)
        {
            writer.Write((byte)0);
        }

        writer.Write((long)0);
    }
}
else
{
    throw new FileNotFoundException("Argument is not a valid file or directory!\n" +
        "Usage:\n" +
        "\tExtract: FilePackage <file>\n" +
        "\tArchive: FilePackage <directory>");
}

struct SFileEntry
{
    public uint Magic;
    public uint EntrySize;
    public uint Size;
    public uint Offset;
    public string Name;

    public byte[] Data;
}

class GAME_CFilePackage
{
    public const uint Magic = 0x504B4744; // PKGD

    public uint Version;
    public uint EntryCount;
    public int EntryStart;

    public List<SFileEntry>? Entries;
}

struct Meta_FileEntry
{
    public int Magic { get; set; }
    public int EntrySize { get; set; }
    public string Name { get; set; }
}

class Meta_FilePackage
{
    public int Version { get; set; }
    public int EntryStart { get; set; }
    public List<Meta_FileEntry>? Entries { get; set; }
}