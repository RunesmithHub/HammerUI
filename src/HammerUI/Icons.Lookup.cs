using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Avalonia.Media;

namespace HammerUI;

public static partial class Icons
{
    private static readonly Lazy<Registry> Lookup = new(Registry.Create);
    private static readonly Dictionary<string, string[]> Registered = new(StringComparer.Ordinal);
    private static readonly Dictionary<string, string> AddedAliases = new(StringComparer.Ordinal);

    /// <summary>Gets the canonical kebab-case names of every icon, such as <c>list-tree</c> or <c>volume</c>, including registered ones.</summary>
    public static IReadOnlyList<string> Names
    {
        get
        {
            lock (Cache)
                return [.. Lookup.Value.Names.Concat(Registered.Keys.Where(k => !Lookup.Value.Properties.ContainsKey(Normalize(k)))).Order(StringComparer.Ordinal)];
        }
    }

    /// <summary>Finds an icon by name, ignoring case, dashes and underscores; accepts aliases such as <c>script</c>, <c>delete</c> or <c>close</c>.</summary>
    public static Geometry? Find(string? name) => TryFind(name, out var icon) ? icon : null;

    /// <summary>Finds an icon by name; see <see cref="Find"/>.</summary>
    public static bool TryFind(string? name, [NotNullWhen(true)] out Geometry? icon)
    {
        icon = null;
        if (Resolve(name) is not { } key)
            return false;
        if (Lookup.Value.Properties.TryGetValue(key, out var property))
            icon = (Geometry?)property.GetValue(null);
        else
        {
            lock (Cache)
                icon = Get(() => Registered[key], key);
        }

        return icon is not null;
    }

    /// <summary>Whether an icon has this name; see <see cref="Find"/>.</summary>
    /// <remarks>Unlike <see cref="TryFind"/>, this creates no geometry, so any thread may call it.</remarks>
    public static bool Exists(string? name) => Resolve(name) is not null;

    /// <summary>Adds an icon, or replaces a built-in or registered one, from SVG path data on the same 24×24 grid as the built-in icons.</summary>
    /// <param name="name">The icon's name, such as <c>git-branch</c>; a built-in name such as <c>folder</c> replaces that icon.</param>
    /// <param name="paths">One or more SVG path strings, each drawn as its own stroke.</param>
    /// <remarks>Controls that already show the icon keep the old geometry, so register icons before the UI that shows them is created.</remarks>
    public static void Register(string name, params string[] paths)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(paths);
        if (paths.Length == 0)
            throw new ArgumentException("An icon needs at least one path.", nameof(paths));

        var key = Normalize(name);
        lock (Cache)
        {
            Registered[key] = [.. paths];
            Cache.Remove(key);
        }
    }

    /// <summary>Makes <paramref name="alias"/> find the icon named <paramref name="icon"/>, such as <c>remove</c> for <c>trash</c>.</summary>
    public static void AddAlias(string alias, string icon)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        ArgumentException.ThrowIfNullOrWhiteSpace(icon);
        lock (Cache)
            AddedAliases[Normalize(alias)] = Normalize(icon);
    }

    private static string? Resolve(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var registry = Lookup.Value;
        var key = Normalize(name);
        lock (Cache)
        {
            if (AddedAliases.TryGetValue(key, out var added))
                key = added;
            else if (registry.Aliases.TryGetValue(key, out var target))
                key = target;
            return registry.Properties.ContainsKey(key) || Registered.ContainsKey(key) ? key : null;
        }
    }

    private static string Normalize(string name)
    {
        var builder = new StringBuilder(name.Length);
        foreach (var c in name)
        {
            if (c is not ('-' or '_' or ' ' or '.'))
                builder.Append(char.ToLowerInvariant(c));
        }

        return builder.ToString();
    }

    private static string ToKebab(string name)
    {
        var builder = new StringBuilder(name.Length + 4);
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            var previous = i > 0 ? name[i - 1] : '\0';
            var boundary = i > 0 && ((char.IsUpper(c) && !char.IsDigit(previous)) || (char.IsDigit(c) && char.IsLetter(previous)));
            if (boundary)
                builder.Append('-');
            builder.Append(char.ToLowerInvariant(c));
        }

        return builder.ToString();
    }

    private sealed record Registry(Dictionary<string, PropertyInfo> Properties, Dictionary<string, string> Aliases, IReadOnlyList<string> Names)
    {
        private static readonly (string Alias, string Icon)[] AliasTable =
        [
            ("hierarchy", "list-tree"), ("light", "lightbulb"), ("script", "file-code"), ("audio", "volume"), ("sound", "volume"),
            ("mute", "volume-off"), ("rotate", "rotate-cw"), ("rectangle-select", "square-dashed"), ("snap", "magnet"),
            ("console", "terminal"), ("plugin", "plug"), ("build", "package-export"), ("random", "dice"), ("line", "slash"),
            ("navigation", "compass"), ("duplicate", "copy-plus"), ("clipboard-paste", "paste"), ("delete", "trash"),
            ("drag-handle", "grip-vertical"), ("close", "x"), ("skip-to-start", "skip-back"), ("restart", "skip-back"),
            ("step", "step-forward"), ("curve", "spline"), ("gradient", "blend"), ("keyframe", "diamond"), ("gear", "settings"),
            ("warning", "alert-triangle"), ("error", "alert-circle"), ("color", "palette"), ("select", "mouse-pointer"), ("pan", "hand"),
        ];

        public static Registry Create()
        {
            var properties = new Dictionary<string, PropertyInfo>(StringComparer.Ordinal);
            var names = new List<string>();
            foreach (var property in typeof(Icons).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (property.PropertyType != typeof(Geometry))
                    continue;
                properties[Normalize(property.Name)] = property;
                names.Add(ToKebab(property.Name));
            }

            var aliases = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var (alias, icon) in AliasTable)
                aliases[Normalize(alias)] = Normalize(icon);

            names.Sort(StringComparer.Ordinal);
            return new Registry(properties, aliases, names);
        }
    }
}
