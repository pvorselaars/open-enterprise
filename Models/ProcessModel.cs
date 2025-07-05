using OpenEnterprise.Ontology;

namespace OpenEnterprise.Models;

public class ProcessModel(HashSet<ProductionFact> production, HashSet<CommunicationFact> communication)
{
    public bool Act(Role performer, Intention intention, Role addressee, ProductionFact proposition)
    {
        // TODO: assert legal transition

        if (proposition == null)
        {
            proposition = new(Guid.NewGuid());
            production.Add(proposition);
        }

        communication.Add(new CommunicationFact {
            Performer = performer,
            Intention = intention,
            Addressee = addressee,
            Proposition = proposition,
            Time = (ulong)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds});

        return true;
    }

    private void Act(CommunicationFact c) => communication.Add(c);
    public HashSet<CommunicationActs> GetPossibleActs(Role actor)
    {
        var defaultAct = new CommunicationActs(null, [Intention.Request]);
        return [defaultAct, .. communication.Where(c => actor.Is(c.Addressee) || actor.Is(c.Performer))
                                            .GroupBy(c => c.Proposition.Id)
                                            .Select(g => g.OrderByDescending(c => c.Time)
                                            .First())
                                            .Select(c => GetPossibleActs(actor, c))];
    }

    private CommunicationActs GetPossibleActs(Role a, CommunicationFact c)
    {
        List<Intention> possibleResponses = [];

        if (a.Is(c.Performer))
        {
            Intention[] responses = c.Intention switch
            {
                // Intention.Request => [Intention.Cancel],
                Intention.Promise => [Intention.State],
                // Intention.State => [Intention.Cancel],
                // Intention.Accept => [Intention.Cancel],
                _ => [],
            };

            possibleResponses.AddRange(responses);
        }

        if (a.Is(c.Addressee))
        {
            Intention [] responses = c.Intention switch
            {
                Intention.Request => [Intention.Promise, Intention.Decline],
                // Intention.Cancel => [Intention.Allow, Intention.Refuse],
                Intention.State => [Intention.Accept, Intention.Reject],
                Intention.Reject => [Intention.State, Intention.Stop],
                _ => [],
            };

            possibleResponses.AddRange(responses);
        }

        return new(c.Proposition, possibleResponses);
    }
}