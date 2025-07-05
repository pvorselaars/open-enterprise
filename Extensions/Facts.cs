using Google.Protobuf;

namespace OpenEnterprise.Ontology;

public partial class ProductionFact
{
    public ProductionFact(Guid id) => Id = ByteString.CopyFrom(id.ToByteArray());
}