namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Basis-Entity für alle Entitäten mit DSGVO-konformem Soft Delete und Audit Trail
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Audit Trail
    public DateTime ErstelltAm { get; set; } = DateTime.UtcNow;
    public Guid ErstelltVon { get; set; }
    public DateTime? GeändertAm { get; set; }
    public Guid? GeändertVon { get; set; }

    // DSGVO-konformes Soft Delete
    public bool IstGelöscht { get; set; } = false;
    public DateTime? GelöschtAm { get; set; }
    public Guid? GelöschtVon { get; set; }
    public string? LöschGrund { get; set; }

    // Optimistic Concurrency
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    // Soft Delete Helper
    public void SoftDelete(Guid gelöschtVon, string grund = "")
    {
        IstGelöscht = true;
        GelöschtAm = DateTime.UtcNow;
        GelöschtVon = gelöschtVon;
        LöschGrund = grund;
    }

    // Update Helper
    public void MarkAsModified(Guid geändertVon)
    {
        GeändertAm = DateTime.UtcNow;
        GeändertVon = geändertVon;
    }
}

/// <summary>
/// Basis-Entity für Historisierung/Versionierung
/// </summary>
public abstract class BaseHistoryEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OriginalId { get; set; } // Referenz zur ursprünglichen Entity

    // Versionierung
    public int Version { get; set; }
    public DateTime VersionErstelltAm { get; set; } = DateTime.UtcNow;
    public Guid VersionErstelltVon { get; set; }
    public string? ÄnderungsGrund { get; set; }

    // Gültigkeitszeitraum
    public DateTime GültigVon { get; set; } = DateTime.UtcNow;
    public DateTime? GültigBis { get; set; }
}
