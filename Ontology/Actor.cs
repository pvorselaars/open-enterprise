namespace OpenEnterprise.Ontology;

public class Role(Guid id = default)
{
    public Guid Id { get; } = id;

    public override string ToString() => $"Role: {Id}";
}

public class Actor(Guid id, Facts facts) : Role(id)
{
    readonly Role other = id == Guid.Empty ? new(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")) : new(Guid.Empty);

    public void Act(ProductionFact proposition, Intention intention)
    {

        if (proposition == null)
        {
            proposition = new();
            facts.Production.Add(proposition);
        }

        facts.Communication.Add(new CommunicationFact(this, intention, other, proposition, DateTime.Now));
    }

    public ICollection<CommunicationActs> GetPossibleActs()
    {
        var defaultAct = new CommunicationActs(null, [Intention.Request]);
        return [defaultAct, .. facts.Communication.Where(c => c.Addressee.Id == Id || c.Performer.Id == Id).GroupBy(c => c.Proposition.Id).Select(g => g.OrderByDescending(c => c.Time).First()).Select(GetPossibleActs)];
    }

    public CommunicationActs GetPossibleActs(CommunicationFact c)
    {
        Intention[] responses = [];

        if (c.Performer.Id == Id)
        {
            responses = c.Intention switch
            {
                // Intention.Request => [Intention.Cancel],
                Intention.Promise => [Intention.State],
                // Intention.State => [Intention.Cancel],
                // Intention.Accept => [Intention.Cancel],
                _ => [],
            };
        }

        if (c.Addressee.Id == Id)
        {
            responses = c.Intention switch
            {
                Intention.Request => [Intention.Promise, Intention.Decline],
                // Intention.Cancel => [Intention.Allow, Intention.Refuse],
                Intention.State => [Intention.Accept, Intention.Reject],
                Intention.Reject => [Intention.State, Intention.Stop],
                _ => [],
            };
        }

        return new(c.Proposition, responses);
    }
}
