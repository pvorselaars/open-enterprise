namespace OpenEnterprise.Ontology;

public partial class CommunicationActs
{
    public CommunicationActs(ProductionFact? proposition, ICollection<Intention> intentions)
    {
        Proposition = proposition;
        Intentions.Add(intentions);
    }
}