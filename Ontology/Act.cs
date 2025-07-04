namespace OpenEnterprise.Ontology;

public record CommunicationActs(ProductionFact? Proposition, Intention[] Intentions)
{
    public override string ToString() => $"A({Proposition}, I: {Intentions})";
}