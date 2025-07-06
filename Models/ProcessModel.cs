using OpenEnterprise.Ontology;

namespace OpenEnterprise.Models;

public class ProcessModel(HashSet<ProductionFact> production, HashSet<CommunicationFact> communication)
{
    public bool Act(CommunicationAct act)
    {
        // TODO: assert legal transition

        if (act.Proposition == null)
        {
            act.Proposition = new(Guid.NewGuid());
            production.Add(act.Proposition);
        }

        communication.Add(new CommunicationFact {
            Act  = act,
            Time = (ulong)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds});

        return true;
    }

    public List<CommunicationAct> GetPossibleActs(Role actor)
    {
        var defaultAct = new CommunicationAct
        {
            Performer = actor,
            Intention = Intention.Request,
            Addressee = new(),
            Proposition = null
        };

        return [defaultAct, .. communication.Where(c => actor.Is(c.Act.Addressee) || actor.Is(c.Act.Performer))
                                            .GroupBy(c => c.Act.Proposition.Id)
                                            .Select(g => g.OrderByDescending(c => c.Time)
                                            .First())
                                            .SelectMany(c => GetPossibleActs(actor, c))];
    }

    private List<CommunicationAct> GetPossibleActs(Role a, CommunicationFact c)
    {
        List<CommunicationAct> possibleResponses = [];

        if (a.Is(c.Act.Performer) && c.Act.Intention == Intention.Promise)
        {

            possibleResponses.Add(new CommunicationAct
            {
                Performer = a,
                Addressee = c.Act.Addressee,
                Proposition = c.Act.Proposition,
                Intention = Intention.State
            });

        }

        if (a.Is(c.Act.Addressee))
        {
            Intention [] intentions = c.Act.Intention switch
            {
                Intention.Request => [Intention.Promise, Intention.Decline],
                // Intention.Cancel => [Intention.Allow, Intention.Refuse],
                Intention.State => [Intention.Accept, Intention.Reject],
                Intention.Reject => [Intention.State, Intention.Stop],
                _ => [],
            };

            foreach (var i in intentions)
            {
                possibleResponses.Add(new CommunicationAct
                {
                    Performer = a,
                    Addressee = c.Act.Performer,
                    Proposition = c.Act.Proposition,
                    Intention = i
                });
            }

        }

        return possibleResponses;
    }
}