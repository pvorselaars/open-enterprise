namespace OpenEnterprise.Ontology;

public enum Intention
{
    Request,
    Promise,
    Decline,
    Quit,
    State,
    Reject,
    Accept,
    Cancel,
    Allow,
    Refuse,
    Stop
}

public class ProductionFact
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public override bool Equals(object? obj)
    {
        if (obj is not ProductionFact other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"Fact: {Id}";
};


public record CommunicationFact(Role Performer, Intention Intention, Role Addressee, ProductionFact Proposition, DateTime Time);

public class Facts
{
    public HashSet<CommunicationFact> Communication = [];
    public HashSet<ProductionFact> Production = [];

    public bool Assert(ProductionFact proposition)
    {
        if (proposition == null) return false;

        var c = Communication.Where(c => c.Proposition.Id == proposition.Id).OrderByDescending(c => c.Time).FirstOrDefault();

        return c != null && c.Intention == Intention.Accept;
    }
}